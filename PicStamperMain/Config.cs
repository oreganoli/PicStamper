using System.Text;

namespace PicStamperMain;

internal static class Config
{
    static Config()
    {
        IntakeBucket = GetEnvVar("INTAKE_BUCKET");
        OutputBucket = GetEnvVar("OUTPUT_BUCKET");
        OutputDomain = GetEnvVar("OUTPUT_DOMAIN");
        KeyPairId = GetEnvVar("STAMPER_KEY_PAIR_ID");
        PemKey = GetEnvVar("STAMPER_PEM_KEY");
        // convert pem key from base64
        PemKey = Encoding.ASCII.GetString(Convert.FromBase64String(PemKey));
    }

    public static string IntakeBucket { get; }
    public static string OutputBucket { get; }
    public static string OutputDomain { get; }
    public static string PemKey { get; }
    public static string KeyPairId { get; }

    private static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new EnvVarException(name);
    }
}