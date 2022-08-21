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
    public static string ProduceUrl()
    {
        throw new NotImplementedException();
    }
}