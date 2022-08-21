namespace LinkSigner;

public static class Config
{
    public static string PemKey { get; }
    public static string InputBucket { get; }

    private static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new EnvVarException(name);
    }
    static Config()
    {
        PemKey = GetEnvVar("STAMPER_PEM_KEY");
        InputBucket = GetEnvVar("STAMPER_INPUT_BUCKET");
    }
}