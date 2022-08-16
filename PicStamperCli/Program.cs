using PicStamper;
// Extract file paths.
var filenames = args;
// Use PicStamper.
foreach (var path in filenames)
{
    var timestamp = TimestampExtractor.TimestampFromFile(path);
    using var inStream = new FileStream(path, FileMode.Open);
    using var outStream = new FileStream(path + "_output.jpg", FileMode.Create);
    ImageStamper.StampImage(inStream, timestamp, outStream);
}