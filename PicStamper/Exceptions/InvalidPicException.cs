namespace PicStamper.Exceptions;

public class InvalidPicException: Exception
{
    public override string Message => "The data provided could not be decoded into an image.";
}