﻿// <copyright file="DetectEdgesTest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using Processing;
    using System.IO;

    using Xunit;

    public class DetectEdgesTest : FileTestBase
    {
        public static readonly TheoryData<EdgeDetection> DetectEdgesFilters
        = new TheoryData<EdgeDetection>
        {
            EdgeDetection.Kayyali,
            EdgeDetection.Kirsch,
            EdgeDetection.Lapacian3X3,
            EdgeDetection.Lapacian5X5,
            EdgeDetection.LaplacianOfGaussian,
            EdgeDetection.Prewitt,
            EdgeDetection.RobertsCross,
            EdgeDetection.Robinson,
            EdgeDetection.Scharr,
            EdgeDetection.Sobel
        };

        [Theory]
        [MemberData(nameof(DetectEdgesFilters))]
        public void ImageShouldApplyDetectEdgesFilter(EdgeDetection detector)
        {
            string path = this.CreateOutputDirectory("DetectEdges");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName(detector);
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.DetectEdges(detector).Save(output);
                }
            }
        }

        [Theory]
        [MemberData(nameof(DetectEdgesFilters))]
        public void ImageShouldApplyDetectEdgesFilterInBox(EdgeDetection detector)
        {
            string path = this.CreateOutputDirectory("DetectEdges");

            foreach (TestFile file in Files)
            {
                string filename = file.GetFileName(detector + "-InBox");
                using (Image image = file.CreateImage())
                using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                {
                    image.DetectEdges(detector, new Rectangle(image.Width / 4, image.Height / 4, image.Width / 2, image.Height / 2))
                         .Save(output);
                }
            }
        }
    }
}