# Convert PDF

## Installation

You need to install the latest version of [GhostScript](https://www.ghostscript.com/download/gsdnld.html) before you can
convert a pdf using Magick.NET.

Make sure you only install the version of GhostScript with the same platform. If you use the 64-bit version of Magick.NET
you should also install the 64-bit version of Ghostscript. You can use the 32-bit version together with the 64-version but
you will get a better performance if you keep the platforms the same.

## Convert PDF to multiple images

```C#
var settings = new MagickReadSettings();
// Settings the density to 300 dpi will create an image with a better quality
settings.Density = new Density(300, 300);

using (var images = new MagickImageCollection())
{
    // Add all the pages of the pdf file to the collection
    images.Read("c:\path\to\Snakeware.pdf", settings);

    var page = 1;
    foreach (var image in images)
    {
        // Write page to file that contains the page number
        image.Write("c:\path\to\Snakeware.Page" + page + ".png");
        // Writing to a specific format works the same as for a single image
        image.Format = MagickFormat.Ptif;
        image.Write("c:\path\to\Snakeware.Page" + page + ".tif");
        page++;
    }
}
```

## Convert PDF to one image

```C#
var settings = new MagickReadSettings();
// Settings the density to 300 dpi will create an image with a better quality
settings.Density = new Density(300);

using (var images = new MagickImageCollection())
{
    // Add all the pages of the pdf file to the collection
    images.Read("c:\path\to\Snakeware.pdf", settings);

    // Create new image that appends all the pages horizontally
    using (var horizontal = images.AppendHorizontally())
    {
        // Save result as a png
        horizontal.Write("c:\path\to\Snakeware.horizontal.png");
    }

    // Create new image that appends all the pages vertically
    using (var vertical = images.AppendVertically())
    {
        // Save result as a png
        vertical.Write("c:\path\to\Snakeware.vertical.png");
    }
}
```

## Create a PDF from two images

```C#
using (var collection = new MagickImageCollection())
{
    // Add first page
    collection.Add(new MagickImage("c:\path\to\SnakewarePage1.jpg"));
    // Add second page
    collection.Add(new MagickImage("c:\path\to\SnakewarePage2.jpg"));

    // Create pdf file with two pages
    collection.Write("c:\path\to\Snakeware.pdf");
}
```

## Create a PDF from a single image

```C#
// Read image from file
using (var image = new MagickImage("c:\path\to\Snakeware.jpg"))
{
    // Create pdf file with a single page
    image.Write("c:\path\to\Snakeware.pdf");
}
```

## Read a single page from a PDF

```C#
using (var collection = new MagickImageCollection())
{
    var settings = new MagickReadSettings();
    settings.FrameIndex = 0; // First page
    settings.FrameCount = 1; // Number of pages

    // Read only the first page of the pdf file
    collection.Read("c:\path\to\Snakeware.pdf", settings);

    // Clear the collection
    collection.Clear();

    settings.FrameCount = 2; // Number of pages

    // Read the first two pages of the pdf file
    collection.Read("c:\path\to\Snakeware.pdf", settings);
}
```
