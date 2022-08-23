namespace PicStamperMain;

public class EnvVarException: Exception
{
    private string VariableName { get; }

    public EnvVarException(string variableName)
    {
        VariableName = variableName;
    }

    public override string Message => $"The environment variable {VariableName} was required, but contained no value.";
}