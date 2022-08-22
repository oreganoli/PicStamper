using PicStamper.Exceptions;
using SkiaSharp;

namespace PicStamper;

/// <summary>
/// Class responsible for stamping an image.
/// </summary>
public static class ImageStamper
{
    public static void StampImage(Stream imageData, string message, Stream outStream)
    {
        var bitmap = SKBitmap.Decode(imageData);
        if (bitmap == null)
        {
            throw new InvalidPicException();
        }

        var canvas = new SKCanvas(bitmap);
        var typeface = SKTypeface.FromFile("BebasNeue-Regular.ttf");
        var font = new SKFont(typeface, 128f);
        var strokePaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            TextAlign = SKTextAlign.Right,
            StrokeWidth = 5f
        };
        var fillPaint = new SKPaint()
        {
            Color = SKColors.Coral,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Right,
        };
        canvas.DrawText(message, bitmap.Width, bitmap.Height, font, fillPaint);
        canvas.DrawText(message, bitmap.Width, bitmap.Height, font, strokePaint);
        if (!bitmap.Encode(outStream, SKEncodedImageFormat.Jpeg, 90))
        {
            throw new ImageEncodingException();
        }
    }
}