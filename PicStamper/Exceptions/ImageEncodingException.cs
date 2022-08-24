namespace PicStamper.Exceptions;

public class ImageEncodingException : Exception
{
    public override string Message => "Could not encode image.";
}