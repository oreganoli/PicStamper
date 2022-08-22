namespace PicStamperMain;

static class Config
{
    public static string GetEnvVar(string name)
    {
        return Environment.GetEnvironmentVariable(name) ?? throw new Exception($"The env var {name} is required");
    }    
}