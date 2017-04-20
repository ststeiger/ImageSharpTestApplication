﻿// <copyright file="BlackWhiteTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;

    using Xunit;

    public class BlackWhiteTest : FileTestBase
    {
        [Fact]
        public void ImageShouldApplyBlackWhiteFilter()
        {
            string path = this.CreateOutputDirectory("BlackWhite");

            foreach (TestFile file in Files)
            {
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{file.FileName}"))
                {
                    image.BlackWhite().Save(output);
                }
            }
        }
    }
}