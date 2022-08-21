using System.Buffers.Text;
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

    private static string UrlSafeBase64(string input)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input))
            .Replace("+", "-")
            .Replace("=", "_")
            .Replace("/", "~");
    }
    public static string ProduceUrl(string domain, string prefix, string filename, int secondsValid, string keyPairId)
    {
        var urlSafePolicy = UrlSafeBase64(MakeUploadPolicy(domain, prefix, secondsValid));
        return $"https://{domain}/{prefix}/{filename}?Policy={urlSafePolicy}&Key-Pair-Id={keyPairId}";
    }
}