namespace PicStamperMain;

public class EnvVarException : Exception
{
    public EnvVarException(string variableName)
    {
        VariableName = variableName;
    }

    private string VariableName { get; }

    public override string Message => $"The environment variable {VariableName} was required, but contained no value.";
}