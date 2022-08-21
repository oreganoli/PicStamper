namespace PicStamperLinkIssuer;

public static class Config
{
    public static string Domain { get; }
    public static string PemKey { get; }
    public static string KeyPairId { get; }

    private static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new EnvVarException(name);
    }
    static Config()
    {
        Domain = GetEnvVar("STAMPER_DOMAIN");
        PemKey = GetEnvVar("STAMPER_PEM_KEY");
        KeyPairId = GetEnvVar("STAMPER_KEY_PAIR_ID");
    }
}