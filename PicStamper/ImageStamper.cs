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
        var typeface = SKTypeface.FromFamilyName(null);
        var font = new SKFont(typeface, 64f);
        var strokePaint = new SKPaint()
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            TextAlign = SKTextAlign.Right,
        };
        var fillPaint = new SKPaint()
        {
            Color = SKColors.White,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            TextAlign = SKTextAlign.Right,
        };
        canvas.DrawText(message, bitmap.Width, bitmap.Height - font.Size, font, fillPaint);
        canvas.DrawText(message, bitmap.Width, bitmap.Height - font.Size, font, strokePaint);
        if (!bitmap.Encode(outStream, SKEncodedImageFormat.Jpeg, 90))
        {
            throw new ImageEncodingException();
        }
    }
}