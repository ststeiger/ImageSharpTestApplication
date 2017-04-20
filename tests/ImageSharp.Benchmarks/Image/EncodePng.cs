﻿// <copyright file="EncodePng.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks.Image
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using BenchmarkDotNet.Attributes;

    using ImageSharp.Formats;
    using ImageSharp.Quantizers;

    using CoreImage = ImageSharp.Image;

    public class EncodePng : BenchmarkBase
    {
        // System.Drawing needs this.
        private Stream bmpStream;
        private Image bmpDrawing;
        private CoreImage bmpCore;

        [Params(false)]
        public bool LargeImage { get; set; }

        [Params(false)]
        public bool UseOctreeQuantizer { get; set; }

        [Setup]
        public void ReadImages()
        {
            if (this.bmpStream == null)
            {
                string path = this.LargeImage
                                  ? "../ImageSharp.Tests/TestImages/Formats/Jpg/baseline/jpeg420exif.jpg"
                                  : "../ImageSharp.Tests/TestImages/Formats/Bmp/Car.bmp";
                this.bmpStream = File.OpenRead(path);
                this.bmpCore = new CoreImage(this.bmpStream);
                this.bmpStream.Position = 0;
                this.bmpDrawing = Image.FromStream(this.bmpStream);
            }
        }

        [Cleanup]
        public void Cleanup()
        {
            this.bmpStream.Dispose();
            this.bmpCore.Dispose();
            this.bmpDrawing.Dispose();
        }

        [Benchmark(Baseline = true, Description = "System.Drawing Png")]
        public void PngSystemDrawing()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                this.bmpDrawing.Save(memoryStream, ImageFormat.Png);
            }
        }

        [Benchmark(Description = "ImageSharp Png")]
        public void PngCore()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Quantizer<ImageSharp.Color> quantizer = this.UseOctreeQuantizer
                                                            ? (Quantizer<ImageSharp.Color>)
                                                            new OctreeQuantizer<ImageSharp.Color>()
                                                            : new PaletteQuantizer<ImageSharp.Color>();

                PngEncoderOptions options = new PngEncoderOptions() { Quantizer = quantizer };
                this.bmpCore.SaveAsPng(memoryStream, options);
            }
        }
    }
}
