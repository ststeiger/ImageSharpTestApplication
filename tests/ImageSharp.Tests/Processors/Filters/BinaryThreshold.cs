﻿// <copyright file="BinaryThresholdTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;

    using Xunit;

    public class BinaryThresholdTest : FileTestBase
    {
        public static readonly TheoryData<float> BinaryThresholdValues
        = new TheoryData<float>
        {
            .25f ,
            .75f ,
        };

        [Theory]
        [MemberData(nameof(BinaryThresholdValues))]
        public void ImageShouldApplyBinaryThresholdFilter(float value)
        {
            string path = this.CreateOutputDirectory("BinaryThreshold");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName(value);
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.BinaryThreshold(value).Save(output);
                }
            }
        }
    }
}