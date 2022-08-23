using System.Security.Cryptography;
using Amazon.Lambda.Core;
using PicStamperLambdas;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
// ReSharper disable once CheckNamespace
namespace UrlIssuer;
public class Function
{
    public Dictionary<string, string> FunctionHandler(ILambdaContext _ctx)
    {
        var dict = new Dictionary<string, string>();
        // Generate the job ID.
        var jobId = (Random.Shared.Next() % 1_000_000).ToString().PadLeft(6, '0');
        // Create the upload URL.
        var rsa = RSA.Create();
        rsa.ImportFromPem(Config.PemKey);
        var signer = new CloudfrontUrlSigner(rsa, Config.Domain, jobId, Config.KeyPairId);
        var url = signer.ProduceUrl("PLACEHOLDER", 3600, false);
        dict.Add("url", url);
        dict.Add("jobId", jobId);
        return dict;
    }
}