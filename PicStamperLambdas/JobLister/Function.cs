using Amazon.DynamoDBv2;
// ReSharper disable once CheckNamespace
namespace JobLister;

public class Function
{
    public async Task<List<JobModel>> Handler()
    {
        var dbClient = new AmazonDynamoDBClient();
        var jobs = await dbClient.ScanAsync("PicStamperJobTable",
            new List<string> { "jobId", "createdAt", "status", "downloadLink" });
        return jobs.Items.Select(item => new JobModel
        {
            JobId = item["jobId"].S,
            CreatedAt = int.Parse(item["createdAt"].N),
            DownloadLink = item["downloadLink"].S,
            Status = item["status"].S
        }).ToList();
    } 
}