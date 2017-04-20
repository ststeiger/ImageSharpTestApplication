
# <img src="build/icons/imagesharp-logo-heading.png" alt="ImageSharp"/>

**ImageSharp** is a new cross-platform 2D graphics API designed to allow the processing of images without the use of `System.Drawing`. 

> **ImageSharp is still in early stages (alpha) but progress has been pretty quick. As such, please do not use on production environments until the library reaches release candidate status. Pre-release downloads are available from the [MyGet package repository](https://www.myget.org/gallery/imagesharp).**

[![GitHub license](https://img.shields.io/badge/license-Apache%202-blue.svg)](https://raw.githubusercontent.com/JimBobSquarePants/ImageSharp/master/APACHE-2.0-LICENSE.txt)
[![GitHub issues](https://img.shields.io/github/issues/JimBobSquarePants/ImageSharp.svg)](https://github.com/JimBobSquarePants/ImageSharp/issues)
[![GitHub stars](https://img.shields.io/github/stars/JimBobSquarePants/ImageSharp.svg)](https://github.com/JimBobSquarePants/ImageSharp/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/JimBobSquarePants/ImageSharp.svg)](https://github.com/JimBobSquarePants/ImageSharp/network)
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/ImageSharp/General?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Twitter](https://img.shields.io/twitter/url/https/github.com/JimBobSquarePants/ImageSharp.svg?style=social)](https://twitter.com/intent/tweet?hashtags=imagesharp,dotnet,oss&text=ImageSharp.+A+new+cross-platform+2D+graphics+API+in+C%23&url=https%3a%2f%2fgithub.com%2fJimBobSquarePants%2fImageSharp&via=james_m_south)


|             |Build Status|Code Coverage|
|-------------|:----------:|:-----------:|
|**Linux/Mac**|[![Build Status](https://travis-ci.org/JimBobSquarePants/ImageSharp.svg)](https://travis-ci.org/JimBobSquarePants/ImageSharp)|[![Code coverage](https://codecov.io/gh/JimBobSquarePants/ImageSharp/branch/master/graph/badge.svg)](https://codecov.io/gh/JimBobSquarePants/ImageSharp)|
|**Windows**  |[![Build Status](https://ci.appveyor.com/api/projects/status/hu6d1gdpxdw0q360/branch/master?svg=true)](https://ci.appveyor.com/project/JamesSouth/imagesharp/branch/master)|[![Code coverage](https://codecov.io/gh/JimBobSquarePants/ImageSharp/branch/master/graph/badge.svg)](https://codecov.io/gh/JimBobSquarePants/ImageSharp)|


### Installation
At present the code is pre-release but when ready it will be available on [Nuget](http://www.nuget.org). 

**Pre-release downloads**

We already have a [MyGet package repository](https://www.myget.org/gallery/imagesharp) - for bleeding-edge / development NuGet releases.

### Packages

The **ImageSharp** library is made up of multiple packages.

Packages include:
- **ImageSharp**
  - Contains the Image classes, Colors, Primitives, Configuration, and other core functionality.
  - The IImageFormat interface, Jpeg, Png, Bmp, and Gif formats.
  - Transform methods like Resize, Crop, Skew, Rotate - Anything that alters the dimensions of the image.
  - Non-transform methods like Gaussian Blur, Pixelate, Edge Detection - Anything that maintains the original image dimensions.

- **ImageSharp.Drawing**
  - Brushes and various drawing algorithms, including drawing images.
  - Various vector drawing methods for drawing paths, polygons etc.
  - Text drawing.

### Manual build

If you prefer, you can compile ImageSharp yourself (please do and help!), you'll need:

- [Visual Studio 2017 (or above)](https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes)
- The [.NET Core SDK Installer](https://www.microsoft.com/net/core#windows) - Non VSCode link.

Alternatively on Linux you can use:

- [Visual Studio Code](https://code.visualstudio.com/) with [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
- [.Net Core](https://www.microsoft.com/net/core#linuxubuntu)

To clone it locally click the "Clone in Windows" button above or run the following git commands.

```bash
git clone https://github.com/JimBobSquarePants/ImageSharp
```

### Features

There's plenty there and more coming. Check out the [current features](features.md)!

### API 

Without the constraints of `System.Drawing` We have been able to develop something much more flexible, easier to code against, and much, much less prone to memory leaks. Gone are system-wide process-locks. Images and processors are thread safe usable in parallel processing utilizing all the availables cores. 

Many `Image` methods are also fluent.

Here's an example of the code required to resize an image using the default Bicubic resampler then turn the colors into their grayscale equivalent using the BT709 standard matrix.

On platforms supporting netstandard 1.3+
```csharp
using (Image image = new Image("foo.jpg"))
{
    image.Resize(image.Width / 2, image.Height / 2)
         .Grayscale()
         .Save("bar.jpg"); // automatic encoder selected based on extension.
}
```
on netstandard 1.1 - 1.2
```csharp
using (FileStream stream = File.OpenRead("foo.jpg"))
using (FileStream output = File.OpenWrite("bar.jpg"))
using (Image image = new Image(stream))
{
    image.Resize(image.Width / 2, image.Height / 2)
         .Grayscale()
         .Save(output);
}
```

Individual processors can be initialised and apply processing against images. This allows nesting which brings the potential for powerful combinations of processing methods:

```csharp
new BrightnessProcessor(50).Apply(sourceImage, sourceImage.Bounds);
```

Setting individual pixel values is perfomed as follows:

```csharp
using (image = new Image(400, 400)
using (var pixels = image.Lock())
{
    pixels[200, 200] = Color.White;
}
```

For advanced usage the `Image<TColor>` and `PixelAccessor<TColor>` classes are available allowing developers to implement their own color models in the same manner as Microsoft XNA Game Studio and MonoGame. 

All in all this should allow image processing to be much more accessible to developers which has always been my goal from the start.

### How can you help?

Please... Spread the word, contribute algorithms, submit performance improvements, unit tests. 

Performance is a biggie, if you know anything about the new vector types and can apply some fancy new stuff with that it would be awesome. 

There's a lot of developers out there who could write this stuff a lot better and faster than I and I would love to see what we collectively can come up with so please, if you can help in any way it would be most welcome and benificial for all.

### The ImageSharp Team

Grand High Eternal Dictator
- [James Jackson-South](https://github.com/jimbobsquarepants)

Core Team
- [Dirk Lemstra](https://github.com/dlemstra)
- [Jeavon Leopold](https://github.com/jeavon)
- [Anton Firsov](https://github.com/antonfirsov)
- [Olivia Ifrim](https://github.com/olivif)
- [Scott Williams](https://github.com/tocsoft)
