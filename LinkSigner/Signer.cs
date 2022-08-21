using System.Text.Json;
using System.Text.Json.Nodes;

namespace LinkSigner;

public static class Signer
{
    private static string MakeUploadPolicy(string domain, string prefix, int secondsValid)
    {
        var expiryTime = DateTime.UtcNow + TimeSpan.FromSeconds(secondsValid);
        var timestamp = (int) (expiryTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
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