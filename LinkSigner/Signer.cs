using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LinkSigner;

public static class Signer
{
    private static string MakeUploadPolicy(string domain, string prefix, int secondsValid)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + secondsValid;
        string policyString = $$"""
        {
            "Statement": [
                {
                    "Resource": "https://{{domain}}/{{prefix}}/*",
                    "Condition": {
                        "DateLessThan": {
                            "AWS:EpochTime": "{{timestamp}}"
                        }
                    }
                }
            ]
        } 
        """;
        // minify the JSON
        policyString = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(policyString));
        return policyString;
    }

    private static string UrlSafeBase64(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("=", "_")
            .Replace("/", "~");
    }
    public static string ProduceUrl(string domain, string prefix, string filename, int secondsValid, string keyPairId)
    {
        var policyString = MakeUploadPolicy(domain, prefix, secondsValid);
        var policyBuffer = Encoding.ASCII.GetBytes(policyString);
        var urlSafePolicy = UrlSafeBase64(policyBuffer);
        return $"https://{domain}/{prefix}/{filename}?Policy={urlSafePolicy}&Key-Pair-Id={keyPairId}";
    }
}