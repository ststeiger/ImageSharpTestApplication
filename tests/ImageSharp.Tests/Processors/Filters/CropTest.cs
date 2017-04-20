﻿// <copyright file="CropTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;

    using Xunit;

    public class CropTest : FileTestBase
    {
        [Fact]
        public void ImageShouldApplyCropSampler()
        {
            string path = this.CreateOutputDirectory("Crop");

            foreach (TestFile file in Files)
            {
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{file.FileName}"))
                {
                    image.Crop(image.Width / 2, image.Height / 2).Save(output);
                }
            }
        }
    }
}