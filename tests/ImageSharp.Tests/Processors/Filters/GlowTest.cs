﻿// <copyright file="GlowTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;

    using Xunit;

    public class GlowTest : FileTestBase
    {
        [Fact]
        public void ImageShouldApplyGlowFilter()
        {
            string path = this.CreateOutputDirectory("Glow");

            foreach (TestFile file in Files)
            {
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{file.FileName}"))
                {
                    image.Glow().Save(output);
                }
            }
        }

        [Fact]
        public void ImageShouldApplyGlowFilterColor()
        {
            string path = this.CreateOutputDirectory("Glow");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName("Color");
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.Glow(Color.HotPink).Save(output);
                }
            }
        }

        [Fact]
        public void ImageShouldApplyGlowFilterRadius()
        {
            string path = this.CreateOutputDirectory("Glow");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName("Radius");
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.Glow(image.Width / 4F).Save(output);
                }
            }
        }

        [Fact]
        public void ImageShouldApplyGlowFilterInBox()
        {
            string path = this.CreateOutputDirectory("Glow");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName("InBox");
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.Glow(new Rectangle(image.Width / 4, image.Height / 4, image.Width / 2, image.Height / 2))
                        .Save(output);
                }
            }
        }
    }
}