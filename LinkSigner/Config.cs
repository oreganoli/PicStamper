namespace LinkSigner;

public static class Config
{
    private static string PemKey { get; }
    private static string InputBucket { get; }
    private static string KeyPairId { get; }

    private static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new EnvVarException(name);
    }
    static Config()
    {
        PemKey = GetEnvVar("STAMPER_PEM_KEY");
        InputBucket = GetEnvVar("STAMPER_INPUT_BUCKET");
        KeyPairId = GetEnvVar("STAMPER_KEY_PAIR_ID");
    }
}