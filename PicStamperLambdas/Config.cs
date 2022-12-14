using System.Text;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace PicStamperLambdas;

public static class Config
{
    static Config()
    {
        IntakeDomain = GetEnvVar("INTAKE_DOMAIN");
        KeyPairId = GetEnvVar("STAMPER_KEY_PAIR_ID");
        PemKey = GetEnvVar("STAMPER_PEM_KEY");
        // convert pem key from base64
        PemKey = Encoding.ASCII.GetString(Convert.FromBase64String(PemKey));
    }

    public static string IntakeDomain { get; }
    public static string PemKey { get; }
    public static string KeyPairId { get; }

    private static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new EnvVarException(name);
    }
}