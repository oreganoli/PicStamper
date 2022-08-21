using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LinkSigner;

/// <summary>
/// Class responsible for producing signed CloudFront URLs.
/// </summary>
public class CloudfrontUrlSigner
{
    /// <param name="rsa">An <see cref="RSA"/> object to produce URL signatures with.</param>
    /// <param name="domain">The domain for the URLs produced.</param>
    /// <param name="prefix">The subfolder (S3 key prefix) into which to place per-filename access links.</param>
    /// <param name="keyPairId">The Amazon ID for the key pair used to sign URLs.</param>
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
        // this is terrible, but I can't get C# 11 features to work in my dev environment
        string policyString =
            "{'Statement':[{'Resource':'https://DOMAIN/PREFIX/*','Condition':{'DateLessThan':{'AWS:EpochTime':'TIMESTAMP'}}}]}"
                .Replace("DOMAIN", _domain)
                .Replace("PREFIX", _prefix)
                .Replace("TIMESTAMP", timestamp.ToString());
        return policyString;
    }

    private static string UrlSafeBase64(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("=", "_")
            .Replace("/", "~");
    }
    /// <summary>
    /// Produce a signed Cloudfront URL for the given filename, valid for a number of seconds from now.
    /// </summary>
    /// <param name="filename">The filename (underlying S3 key) to grant access to.></param>
    /// <param name="secondsValid">Seconds until the URL expires.</param>
    /// <returns>A HTTPS signed URL.</returns>
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