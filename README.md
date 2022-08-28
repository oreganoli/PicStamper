# PicStamper

A .NET Core application for drawing date stamps on photographs, as one would get from some digital cameras, based on EXIF date and time data.

# Design
PicStamper is built out of a core library and several AWS Lambda functions exposed through an API Gateway. The core library exposes an API that utilizes the MetadataExtractor and SkiaSharp libraries to extract date/time data from a photograph in a short DD.MM.YYYY format and apply a visible stamp in a picture's lower right corner.

In a typical run, a user will visit the application's website and open a new batch job. This causes an auxiliary Lambda function to produce a unique ID for the job, as well as a signed CloudFront upload URL pointed at the backing S3 bucket for picture uploads. Having chosen pictures to be stamped, the user can asynchronously trigger the main function, which downloads the uploaded pictures, applies the necessary operations to them, puts them into a ZIP archive and uploads said archive to S3. Finally, the application receives a signed download link to the ready archive.

# Live instance

PicStamper is deployed live and available to use at [picstamper.oreganoli.xyz](https://picstamper.oreganoli.xyz/).

# Example output
![Picture processed by PicStamper](example_output.jpg)
