﻿// <copyright file="RotateTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.IO;
    using Processing;
    using Xunit;

    public class RotateTest : FileTestBase
    {
        public static readonly TheoryData<float> RotateFloatValues
            = new TheoryData<float>
        {
             170 ,
            -170 ,
        };

        public static readonly TheoryData<RotateType> RotateEnumValues
            = new TheoryData<RotateType>
        {
            RotateType.None,
            RotateType.Rotate90,
            RotateType.Rotate180,
            RotateType.Rotate270
        };

        [Theory]
        [MemberData(nameof(RotateFloatValues))]
        public void ImageShouldApplyRotateSampler(float value)
        {
            string path = this.CreateOutputDirectory("Rotate");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName(value);
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.Rotate(value).Save(output);
                }
            }
        }

        [Theory]
        [MemberData(nameof(RotateEnumValues))]
        public void ImageShouldApplyRotateSampler(RotateType value)
        {
            string path = this.CreateOutputDirectory("Rotate");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName(value);
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.Rotate(value).Save(output);
                }
            }
        }
    }
}