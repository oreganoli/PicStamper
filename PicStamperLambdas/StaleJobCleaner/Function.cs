using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;

// ReSharper disable once CheckNamespace
namespace StaleJobCleaner;

public class Function
{
    private const long MaxJobAge = 600;

    public async Task<string> Handler(ILambdaContext ctx)
    {
        var dbClient = new AmazonDynamoDBClient();
        var boundaryTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - MaxJobAge;
        var staleJobs = await dbClient.ScanAsync(new ScanRequest("PicStamperJobTable")
        {
            ProjectionExpression = "jobId",
            FilterExpression = "createdAt < :ts",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":ts", new AttributeValue { N = boundaryTimestamp.ToString() } }
            }
        });
        if (staleJobs.Count < 1) return "Found no stale jobs, exiting.";
        var staleJobIds = staleJobs.Items.Select(each => each["jobId"].S)
            .Select(each => new WriteRequest(new DeleteRequest(new Dictionary<string, AttributeValue>
                { { "jobId", new AttributeValue(each) } })));
        await dbClient.BatchWriteItemAsync(new BatchWriteItemRequest
        {
            RequestItems = new Dictionary<string, List<WriteRequest>>
            {
                { "PicStamperJobTable", staleJobIds.ToList() }
            }
        });
        return $"Deleted {staleJobs.Count} stale jobs";
    }
}