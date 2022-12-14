using Amazon;
using Amazon.CloudFront;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.S3;
using Amazon.S3.Model;
using PicStamper;
using SharpCompress.Archives.Zip;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace PicStamperMain;

public class Function
{
    public async Task<string> Handler(string jobId, ILambdaContext ctx)
    {
        // List all files for job ID.
        var s3Client = new AmazonS3Client(RegionEndpoint.EUCentral1);
        var files = await s3Client.ListObjectsV2Async(new ListObjectsV2Request
        {
            BucketName = Config.IntakeBucket,
            Prefix = jobId
        });
        var tasks = files.S3Objects.Select(each =>
        {
            var task = Task.Run(async () =>
            {
                // make s3 client
                // ReSharper disable once VariableHidesOuterVariable
                var s3Client = new AmazonS3Client(RegionEndpoint.EUCentral1);
                // Download file.
                var objectResponse = await s3Client.GetObjectAsync(each.BucketName, each.Key);
                var memStream = new MemoryStream();
                var filename = objectResponse.Key.Split("/").Last();
                await using var resStream = objectResponse.ResponseStream;
                // Copy to memory.
                await resStream.CopyToAsync(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                var timestamp = TimestampExtractor.Timestamp(memStream) ?? DateTime.UtcNow.ToShortDateString();
                memStream.Seek(0, SeekOrigin.Begin);
                var outputStream = new MemoryStream();
                ImageStamper.StampImage(memStream, timestamp, outputStream);
                return (filename, outputStream);
            });
            return task;
        });
        var results = await Task.WhenAll(tasks);
        using var zipArchive = ZipArchive.Create();
        using var zipMemStream = new MemoryStream();
        foreach (var (filename, data) in results) zipArchive.AddEntry(filename, data);
        zipArchive.SaveTo(zipMemStream);

        await s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = Config.OutputBucket,
            Key = $"{jobId}.zip",
            InputStream = zipMemStream
        });

        var url = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
            AmazonCloudFrontUrlSigner.Protocol.https,
            Config.OutputDomain,
            new StringReader(Config.PemKey),
            $"{jobId}.zip",
            Config.KeyPairId,
            DateTime.UtcNow + TimeSpan.FromDays(1)
        );

        // Update job in DB.
        var dbClient = new AmazonDynamoDBClient();
        await dbClient.UpdateItemAsync("PicStamperJobTable", new Dictionary<string, AttributeValue>
        {
            { "jobId", new AttributeValue { S = jobId } }
        }, new Dictionary<string, AttributeValueUpdate>
        {
            {
                "status",
                new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = "done" } }
            },
            {
                "downloadLink",
                new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = url } }
            }
        });

        return url;
    }
}