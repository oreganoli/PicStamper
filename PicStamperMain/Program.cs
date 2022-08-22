﻿using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using PicStamper;
using PicStamperMain;
using SharpCompress.Archives.Zip;

// AWS setup.
var creds = new BasicAWSCredentials(Config.GetEnvVar("AWS_ACCESS_KEY_ID"), Config.GetEnvVar("AWS_SECRET_ACCESS_KEY"));

// Extract job ID.
var jobId = args.First();
// List all files for job ID.
var s3Client = new AmazonS3Client(creds, RegionEndpoint.EUCentral1);
var files = await s3Client.ListObjectsV2Async(new ListObjectsV2Request
{
    BucketName = Config.GetEnvVar("INTAKE_BUCKET"),
    Prefix = jobId
});



var tasks = files.S3Objects.Select(each =>
{
    var task = Task.Run(async () =>
    {
        // make s3 client
        // ReSharper disable once VariableHidesOuterVariable
        var s3Client = new AmazonS3Client(creds, RegionEndpoint.EUCentral1);
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
await using var zipFileStream = new FileStream($"{jobId}.zip", FileMode.Create);
using var zipArchive = ZipArchive.Create();
using var zipMemStream = new MemoryStream();
foreach (var (filename, data) in results)
{
    zipArchive.AddEntry(filename, data);
}
zipArchive.SaveTo(zipMemStream);

await s3Client.PutObjectAsync(new PutObjectRequest
{
    BucketName = Config.GetEnvVar("OUTPUT_BUCKET"),
    Key = $"{jobId}.zip",
    InputStream = zipMemStream
});