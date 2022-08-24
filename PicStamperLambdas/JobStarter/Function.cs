using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;

// ReSharper disable once CheckNamespace
namespace JobStarter;

public class Function
{
    public async Task<string> Handler(string jobId, ILambdaContext ctx)
    {
        var lambdaClient = new AmazonLambdaClient();
        var request = new InvokeRequest
        {
            Payload = JsonSerializer.Serialize(jobId),
            FunctionName = "PicStamperMain",
            InvocationType = InvocationType.Event
        };
        // Start job.
        await lambdaClient.InvokeAsync(request);
        // Update job in DB.
        var dbClient = new AmazonDynamoDBClient();
        await dbClient.UpdateItemAsync("PicStamperJobTable", new Dictionary<string, AttributeValue>
        {
            { "jobId", new AttributeValue { S = jobId } }
        }, new Dictionary<string, AttributeValueUpdate>
        {
            {
                "status",
                new AttributeValueUpdate
                    { Action = AttributeAction.PUT, Value = new AttributeValue { S = "inProgress" } }
            }
        });
        return $"Job number {jobId} accepted for processing.";
    }
}