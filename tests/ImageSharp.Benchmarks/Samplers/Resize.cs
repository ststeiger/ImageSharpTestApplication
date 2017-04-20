﻿// <copyright file="Resize.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks
{
    using System.Drawing;
    using System.Drawing.Drawing2D;

    using BenchmarkDotNet.Attributes;
    using CoreSize = ImageSharp.Size;
    using CoreImage = ImageSharp.Image;

    public class Resize : BenchmarkBase
    {
        [Benchmark(Baseline = true, Description = "System.Drawing Resize")]
        public Size ResizeSystemDrawing()
        {
            using (Bitmap source = new Bitmap(2000, 2000))
            {
                using (Bitmap destination = new Bitmap(400, 400))
                {
                    using (Graphics graphics = Graphics.FromImage(destination))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(source, 0, 0, 400, 400);
                    }

                    return destination.Size;
                }
            }
        }

        [Benchmark(Description = "ImageSharp Resize")]
        public CoreSize ResizeCore()
        {
            using (CoreImage image = new CoreImage(2000, 2000))
            {
                image.Resize(400, 400);
                return new CoreSize(image.Width, image.Height);
            }
        }

        [Benchmark(Description = "ImageSharp Compand Resize")]
        public CoreSize ResizeCoreCompand()
        {
            using (CoreImage image = new CoreImage(2000, 2000))
            {
                image.Resize(400, 400, true);
                return new CoreSize(image.Width, image.Height);
            }
        }
    }
}
