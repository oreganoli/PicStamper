using System.Text.Json;
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
        var request = new InvokeRequest()
        {
            Payload = $"\"{jobId}\"",
            FunctionName = "PicStamperMain",
            InvocationType = InvocationType.Event,
        };
        await lambdaClient.InvokeAsync(request);
        return $"Job number {jobId} accepted for processing.";
    }
}