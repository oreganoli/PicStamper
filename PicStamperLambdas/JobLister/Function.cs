using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

// ReSharper disable once CheckNamespace
namespace JobLister;

public class Function
{
    public async Task<List<JobModel>> Handler()
    {
        var dbClient = new AmazonDynamoDBClient();
        var scanReq = new ScanRequest
            { TableName = "PicStamperJobTable", ProjectionExpression = "jobId, createdAt, status, downloadLink"};
        var jobs = await dbClient.ScanAsync(scanReq);
        return jobs.Items.Select(item => new JobModel
        {
            JobId = item["jobId"].S,
            CreatedAt = int.Parse(item["createdAt"].N),
            DownloadLink = item["downloadLink"].S,
            Status = item["status"].S
        }).ToList();
    } 
}