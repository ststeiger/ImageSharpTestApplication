﻿// <copyright file="JpegEncoderTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageSharp.Formats;
using Xunit;
using Xunit.Abstractions;
// ReSharper disable InconsistentNaming

namespace ImageSharp.Tests
{
    using ImageSharp.Formats.Jpg;
    using ImageSharp.Processing;

    public class JpegEncoderTests : MeasureFixture
    {
        public static IEnumerable<string> AllBmpFiles => TestImages.Bmp.All;

        public JpegEncoderTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Theory]
        [WithFile(TestImages.Jpeg.Baseline.Snake, PixelTypes.StandardImageClass, 75, JpegSubsample.Ratio420)]
        [WithFile(TestImages.Jpeg.Baseline.Lake, PixelTypes.StandardImageClass, 75, JpegSubsample.Ratio420)]
        [WithFile(TestImages.Jpeg.Baseline.Snake, PixelTypes.StandardImageClass, 75, JpegSubsample.Ratio444)]
        [WithFile(TestImages.Jpeg.Baseline.Lake, PixelTypes.StandardImageClass, 75, JpegSubsample.Ratio444)]
        public void LoadResizeSave<TColor>(TestImageProvider<TColor> provider, int quality, JpegSubsample subsample)
            where TColor : struct, IPixel<TColor>
        {
            using (Image<TColor> image = provider.GetImage().Resize(new ResizeOptions { Size = new Size(150, 100), Mode = ResizeMode.Max }))
            {
                image.MetaData.Quality = quality;
                image.MetaData.ExifProfile = null; // Reduce the size of the file
                JpegEncoder encoder = new JpegEncoder();
                JpegEncoderOptions options = new JpegEncoderOptions { Subsample = subsample, Quality = quality };

                provider.Utility.TestName += $"{subsample}_Q{quality}";
                provider.Utility.SaveTestOutputFile(image, "png");
                provider.Utility.SaveTestOutputFile(image, "jpg", encoder, options);
            }
        }

        [Theory]
        [WithFileCollection(nameof(AllBmpFiles), PixelTypes.Color | PixelTypes.StandardImageClass | PixelTypes.Argb, JpegSubsample.Ratio420, 75)]
        [WithFileCollection(nameof(AllBmpFiles), PixelTypes.Color | PixelTypes.StandardImageClass | PixelTypes.Argb, JpegSubsample.Ratio444, 75)]
        public void OpenBmp_SaveJpeg<TColor>(TestImageProvider<TColor> provider, JpegSubsample subSample, int quality)
           where TColor : struct, IPixel<TColor>
        {
            using (Image<TColor> image = provider.GetImage())
            {
                ImagingTestCaseUtility utility = provider.Utility;
                utility.TestName += "_" + subSample + "_Q" + quality;

                using (FileStream outputStream = File.OpenWrite(utility.GetTestOutputFileName("jpg")))
                {
                    JpegEncoder encoder = new JpegEncoder();

                    image.Save(outputStream, encoder, new JpegEncoderOptions()
                    {
                      Subsample = subSample,
                      Quality = quality
                    });
                }
            }
        }

        [Fact]
        public void Encode_IgnoreMetadataIsFalse_ExifProfileIsWritten()
        {
            EncoderOptions options = new EncoderOptions()
            {
                IgnoreMetadata = false
            };

            TestFile testFile = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan);

            using (Image input = testFile.CreateImage())
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    input.Save(memStream, new JpegFormat(), options);

                    memStream.Position = 0;
                    using (Image output = new Image(memStream))
                    {
                        Assert.NotNull(output.MetaData.ExifProfile);
                    }
                }
            }
        }

        [Fact]
        public void Encode_IgnoreMetadataIsTrue_ExifProfileIgnored()
        {
            JpegEncoderOptions options = new JpegEncoderOptions()
            {
                IgnoreMetadata = true
            };

            TestFile testFile = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan);

            using (Image input = testFile.CreateImage())
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    input.SaveAsJpeg(memStream, options);

                    memStream.Position = 0;
                    using (Image output = new Image(memStream))
                    {
                        Assert.Null(output.MetaData.ExifProfile);
                    }
                }
            }
        }
    }
}