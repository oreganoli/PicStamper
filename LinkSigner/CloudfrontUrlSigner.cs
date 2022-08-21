using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LinkSigner;

public class CloudfrontUrlSigner
{
    public CloudfrontUrlSigner(RSA rsa, string domain, string prefix, string keyPairId)
    {
        _rsa = rsa;
        _domain = domain;
        _prefix = prefix;
        _keyPairId = keyPairId;
    }

    private readonly RSA _rsa;
    private readonly string _domain;
    private readonly string _prefix;
    private readonly string _keyPairId;
    private byte[] SignBytes(byte[] input)
    {
        byte[] policyHash;
        using (var cryptoSha1 = SHA1.Create())
        {
            policyHash = cryptoSha1.ComputeHash(input);
        }
        var formatter = new RSAPKCS1SignatureFormatter(_rsa);
        formatter.SetHashAlgorithm("SHA1");
        var signedHash = formatter.CreateSignature(policyHash);
        return signedHash;
    }
    private string MakeUploadPolicy(int secondsValid)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + secondsValid;
        string policyString = $$"""
        {
            "Statement": [
                {
                    "Resource": "https://{{_domain}}/{{_prefix}}/*",
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
    public string ProduceUrl(string filename, int secondsValid)
    {
        var policyString = MakeUploadPolicy(secondsValid);
        var policyBuffer = Encoding.ASCII.GetBytes(policyString);
        var urlSafePolicy = UrlSafeBase64(policyBuffer);
        var signature = SignBytes(policyBuffer);
        var urlSafeSignature = UrlSafeBase64(signature);
        return $"https://{_domain}/{_prefix}/{filename}?Policy={urlSafePolicy}&Signature={urlSafeSignature}&Key-Pair-Id={_keyPairId}";
    }
}