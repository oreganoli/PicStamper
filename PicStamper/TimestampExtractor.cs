using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using PicStamper.Exceptions;

namespace PicStamper;

/// <summary>
///     Class responsible for getting a timestamp out of a photo.
/// </summary>
public static class TimestampExtractor
{
    public static string? Timestamp(Stream stream)
    {
        var directories = ImageMetadataReader.ReadMetadata(stream);
        var exif = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        var dateTime = exif?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal)?
            .Split(" ")
            .First()
            .Replace(":", ".");
        return dateTime;
    }

    public static string TimestampFromFile(string path)
    {
        if (!File.Exists(path) || File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            throw new NoSuchFileException(path);
        var defaultTime = File.GetCreationTime(path);
        var imgFile = new FileStream(path, FileMode.Open);
        return Timestamp(imgFile) ?? defaultTime.ToShortDateString();
    }
}