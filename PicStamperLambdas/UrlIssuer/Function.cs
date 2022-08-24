using System.Security.Cryptography;
using Amazon.CloudFront;
using Amazon.Lambda.Core;
using PicStamperLambdas;
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
        var policy = AmazonCloudFrontUrlSigner.BuildPolicyForSignedUrl($"https://{Config.IntakeDomain}/{jobId}/*",
            DateTime.UtcNow + TimeSpan.FromHours(1), null);
        var url = AmazonCloudFrontUrlSigner.SignUrl($"https://{Config.IntakeDomain}/{jobId}/PLACEHOLDER.jpg", Config.KeyPairId,
            new StringReader(Config.PemKey), policy);
        dict.Add("url", url);
        dict.Add("jobId", jobId);
        return dict;
    }
}