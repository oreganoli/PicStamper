namespace PicStamper.Exceptions;

public class NoSuchFileException : Exception
{
    public string Path;

    public NoSuchFileException(string path)
    {
        Path = path;
    }

    public override string Message => $"The file {Path} does not exist or is a directory.";
}