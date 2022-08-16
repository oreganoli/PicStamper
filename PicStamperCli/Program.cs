using PicStamper;
// Extract file paths.
var filenames = args;
// Use PicStamper.
foreach (var path in filenames)
{
    Console.WriteLine($"{path} : {TimestampExtractor.TimestampFromFile(path)}");
}