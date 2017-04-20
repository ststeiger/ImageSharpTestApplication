﻿// <copyright file="ColorConversionTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Drawing
{
    using Drawing;
    using ImageSharp.Drawing;
    using ShapePath = SixLabors.Shapes.Path;
    using SixLabors.Shapes;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Numerics;

    using Xunit;

    public class DrawPathTests : FileTestBase
    {
        [Fact]
        public void ImageShouldBeOverlayedByPath()
        {
            string path = this.CreateOutputDirectory("Drawing", "Path");
            using (Image image = new Image(500, 500))
            {
                LinearLineSegment linerSegemnt = new LinearLineSegment(
                    new Vector2(10, 10),
                    new Vector2(200, 150),
                    new Vector2(50, 300));
                BezierLineSegment bazierSegment = new BezierLineSegment(new Vector2(50, 300),
                    new Vector2(500, 500),
                    new Vector2(60, 10),
                    new Vector2(10, 400));

                ShapePath p = new ShapePath(linerSegemnt, bazierSegment);

                using (FileStream output = File.OpenWrite($"{path}/Simple.png"))
                {
                    image
                        .BackgroundColor(Color.Blue)
                        .Draw(Color.HotPink, 5, p)
                        .Save(output);
                }

                using (PixelAccessor<Color> sourcePixels = image.Lock())
                {
                    Assert.Equal(Color.HotPink, sourcePixels[9, 9]);

                    Assert.Equal(Color.HotPink, sourcePixels[199, 149]);

                    Assert.Equal(Color.Blue, sourcePixels[50, 50]);
                }
            }
        }


        [Fact]
        public void ImageShouldBeOverlayedPathWithOpacity()
        {
            string path = this.CreateOutputDirectory("Drawing", "Path");

            Color color = new Color(Color.HotPink.R, Color.HotPink.G, Color.HotPink.B, 150);


            LinearLineSegment linerSegemnt = new LinearLineSegment(
                            new Vector2(10, 10),
                            new Vector2(200, 150),
                            new Vector2(50, 300)
                    );

            BezierLineSegment bazierSegment = new BezierLineSegment(new Vector2(50, 300),
                new Vector2(500, 500),
                new Vector2(60, 10),
                new Vector2(10, 400));

            ShapePath p = new ShapePath(linerSegemnt, bazierSegment);

            using (Image image = new Image(500, 500))
            {
                using (FileStream output = File.OpenWrite($"{path}/Opacity.png"))
                {
                    image
                        .BackgroundColor(Color.Blue)
                        .Draw(color, 10, p)
                        .Save(output);
                }

                //shift background color towards forground color by the opacity amount
                Color mergedColor = new Color(Vector4.Lerp(Color.Blue.ToVector4(), Color.HotPink.ToVector4(), 150f / 255f));

                using (PixelAccessor<Color> sourcePixels = image.Lock())
                {
                    Assert.Equal(mergedColor, sourcePixels[9, 9]);

                    Assert.Equal(mergedColor, sourcePixels[199, 149]);

                    Assert.Equal(Color.Blue, sourcePixels[50, 50]);
                }
            }
        }

    }
}