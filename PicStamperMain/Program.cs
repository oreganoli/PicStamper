using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using PicStamper;
using PicStamperMain;

// AWS setup.
var creds = new BasicAWSCredentials(Config.GetEnvVar("AWS_ACCESS_KEY_ID"), Config.GetEnvVar("AWS_SECRET_ACCESS_KEY"));
var s3Client = new AmazonS3Client(creds, RegionEndpoint.EUCentral1);

// Extract job ID.
var jobID = args.First();
// List all files for job ID.

var files = await s3Client.ListObjectsV2Async(new ListObjectsV2Request
{
    BucketName = Config.GetEnvVar("INTAKE_BUCKET"),
    Prefix = jobID
});

var tasks = files.S3Objects.Select(each =>
{
    var task = Task.Run(async () =>
    {
        // Download file.
        var objectResponse = await s3Client.GetObjectAsync(each.BucketName, each.Key);
        var memStream = new MemoryStream();
        await using var resStream = objectResponse.ResponseStream;
        await using var fileStream = new FileStream($"{each.Key.Split("/").Last().Replace(".", "_output.")}", FileMode.Create);
        // Copy to memory.
        await resStream.CopyToAsync(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        var timestamp = TimestampExtractor.Timestamp(memStream) ?? DateTime.UtcNow.ToShortDateString();
        memStream.Seek(0, SeekOrigin.Begin);
        ImageStamper.StampImage(memStream, timestamp, fileStream);
    });
    return task;
});
await Task.WhenAll(tasks);
