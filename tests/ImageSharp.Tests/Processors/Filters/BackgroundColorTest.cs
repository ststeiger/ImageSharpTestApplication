﻿// <copyright file="BackgroundColorTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;

    using Xunit;

    public class BackgroundColorTest : FileTestBase
    {
        [Fact]
        public void ImageShouldApplyBackgroundColorFilter()
        {
            string path = this.CreateOutputDirectory("BackgroundColor");

            foreach (TestFile file in Files)
            {
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{file.FileName}"))
                {
                    image.BackgroundColor(Color.HotPink).Save(output);
                }
            }
        }
    }
}