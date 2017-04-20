﻿// <copyright file="ExifValueTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests
{
    using System.Linq;
    using Xunit;

    public class ExifValueTests
    {
        private static ExifValue GetExifValue()
        {
            ExifProfile profile;
            using (Image image = TestFile.Create(TestImages.Jpeg.Baseline.Floorplan).CreateImage())
            {
                profile = image.MetaData.ExifProfile;
            }

            Assert.NotNull(profile);

            return profile.Values.First();
        }

        [Fact]
        public void IEquatable()
        {
            ExifValue first = GetExifValue();
            ExifValue second = GetExifValue();

            Assert.True(first == second);
            Assert.True(first.Equals(second));
            Assert.True(first.Equals((object)second));
        }

        [Fact]
        public void Properties()
        {
            ExifValue value = GetExifValue();

            Assert.Equal(ExifDataType.Ascii, value.DataType);
            Assert.Equal(ExifTag.GPSDOP, value.Tag);
            Assert.Equal(false, value.IsArray);
            Assert.Equal("Windows Photo Editor 10.0.10011.16384", value.ToString());
            Assert.Equal("Windows Photo Editor 10.0.10011.16384", value.Value);
        }
    }
}
