﻿// <copyright file="PackedPixelTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Colors
{
    using System;
    using System.Diagnostics;
    using System.Numerics;

    using Xunit;

    /// <summary>
    /// The packed pixel tests.
    /// </summary>
    /// <remarks>
    /// The "ToVector4" tests should now be covered in <see cref="ColorConstructorTests"/>
    /// and at some point they can be safely removed from here.
    /// </remarks>
    public class PackedPixelTests
    {
        [Fact]
        public void Alpha8()
        {
            // Test the limits.
            Assert.Equal(0x0, new Alpha8(0F).PackedValue);
            Assert.Equal(0xFF, new Alpha8(1F).PackedValue);

            // Test clamping.
            Assert.Equal(0x0, new Alpha8(-1234F).PackedValue);
            Assert.Equal(0xFF, new Alpha8(1234F).PackedValue);

            // Test ordering
            Assert.Equal(124, new Alpha8(124F / 0xFF).PackedValue);
            Assert.Equal(26, new Alpha8(0.1F).PackedValue);

            // Test ordering
            Vector4 vector = new Alpha8(.5F).ToVector4();

            Assert.Equal(vector.X, 0);
            Assert.Equal(vector.Y, 0);
            Assert.Equal(vector.Z, 0);
            Assert.Equal(vector.W, .5F, 2);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Alpha8(.5F).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 0, 0, 0 });

            new Alpha8(.5F).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 0, 0, 0, 128 });

            new Alpha8(.5F).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 0, 0 });

            new Alpha8(.5F).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 0, 0, 128 });
        }

        [Fact]
        public void Argb()
        {
            // Test the limits.
            Assert.Equal((uint)0x0, new Argb(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFFFFFF, new Argb(Vector4.One).PackedValue);

            // Test ToVector4.
            Assert.True(Equal(Vector4.One, new Argb(Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.Zero, new Argb(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.UnitX, new Argb(Vector4.UnitX).ToVector4()));
            Assert.True(Equal(Vector4.UnitY, new Argb(Vector4.UnitY).ToVector4()));
            Assert.True(Equal(Vector4.UnitZ, new Argb(Vector4.UnitZ).ToVector4()));
            Assert.True(Equal(Vector4.UnitW, new Argb(Vector4.UnitW).ToVector4()));

            // Test clamping.
            Assert.True(Equal(Vector4.Zero, new Argb(Vector4.One * -1234.0f).ToVector4()));
            Assert.True(Equal(Vector4.One, new Argb(Vector4.One * +1234.0f).ToVector4()));

            float x = +0.1f;
            float y = -0.3f;
            float z = +0.5f;
            float w = -0.7f;
            Argb argb = new Argb(x, y, z, w);
            Assert.Equal(0x001a0080u, argb.PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            argb.ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 0x1a, 0, 0x80 });

            argb.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 0x1a, 0, 0x80, 0 });

            argb.ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0x80, 0, 0x1a });

            argb.ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0x80, 0, 0x1a, 0 });
        }

        [Fact]
        public void Bgr565()
        {
            // Test the limits.
            Assert.Equal(0x0, new Bgr565(Vector3.Zero).PackedValue);
            Assert.Equal(0xFFFF, new Bgr565(Vector3.One).PackedValue);

            // Test ToVector3.
            Assert.True(Equal(Vector3.One, new Bgr565(Vector3.One).ToVector3()));
            Assert.True(Equal(Vector3.Zero, new Bgr565(Vector3.Zero).ToVector3()));
            Assert.True(Equal(Vector3.UnitX, new Bgr565(Vector3.UnitX).ToVector3()));
            Assert.True(Equal(Vector3.UnitY, new Bgr565(Vector3.UnitY).ToVector3()));
            Assert.True(Equal(Vector3.UnitZ, new Bgr565(Vector3.UnitZ).ToVector3()));

            // Test clamping.
            Assert.True(Equal(Vector3.Zero, new Bgr565(Vector3.One * -1234F).ToVector3()));
            Assert.True(Equal(Vector3.One, new Bgr565(Vector3.One * 1234F).ToVector3()));

            // Make sure the swizzle is correct.
            Assert.Equal(0xF800, new Bgr565(Vector3.UnitX).PackedValue);
            Assert.Equal(0x07E0, new Bgr565(Vector3.UnitY).PackedValue);
            Assert.Equal(0x001F, new Bgr565(Vector3.UnitZ).PackedValue);

            float x = 0.1F;
            float y = -0.3F;
            float z = 0.5F;
            Assert.Equal(6160, new Bgr565(x, y, z).PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Bgr565(x, y, z).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 25, 0, 132 });

            new Bgr565(x, y, z).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 25, 0, 132, 255 });

            new Bgr565(x, y, z).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 132, 0, 25 });

            new Bgr565(x, y, z).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 132, 0, 25, 255 });
        }

        [Fact]
        public void Bgra4444()
        {
            // Test the limits.
            Assert.Equal(0x0, new Bgra4444(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFF, new Bgra4444(Vector4.One).PackedValue);

            // Test ToVector4.
            Assert.True(Equal(Vector4.One, new Bgra4444(Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.Zero, new Bgra4444(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.UnitX, new Bgra4444(Vector4.UnitX).ToVector4()));
            Assert.True(Equal(Vector4.UnitY, new Bgra4444(Vector4.UnitY).ToVector4()));
            Assert.True(Equal(Vector4.UnitZ, new Bgra4444(Vector4.UnitZ).ToVector4()));
            Assert.True(Equal(Vector4.UnitW, new Bgra4444(Vector4.UnitW).ToVector4()));

            // Test clamping.
            Assert.True(Equal(Vector4.Zero, new Bgra4444(Vector4.One * -1234.0f).ToVector4()));
            Assert.True(Equal(Vector4.One, new Bgra4444(Vector4.One * 1234.0f).ToVector4()));

            // Make sure the swizzle is correct.
            Assert.Equal(0x0F00, new Bgra4444(Vector4.UnitX).PackedValue);
            Assert.Equal(0x00F0, new Bgra4444(Vector4.UnitY).PackedValue);
            Assert.Equal(0x000F, new Bgra4444(Vector4.UnitZ).PackedValue);
            Assert.Equal(0xF000, new Bgra4444(Vector4.UnitW).PackedValue);

            float x = 0.1f;
            float y = -0.3f;
            float z = 0.5f;
            float w = -0.7f;
            Assert.Equal(520, new Bgra4444(x, y, z, w).PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Bgra4444(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 34, 0, 136 });

            new Bgra4444(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 34, 0, 136, 0 });

            new Bgra4444(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 136, 0, 34 });

            new Bgra4444(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 136, 0, 34, 0 });
        }

        [Fact]
        public void Bgra5551()
        {
            // Test the limits.
            Assert.Equal(0x0, new Bgra5551(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFF, new Bgra5551(Vector4.One).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.Zero, new Bgra5551(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.One, new Bgra5551(Vector4.One).ToVector4()));

            // Test clamping.
            Assert.Equal(Vector4.Zero, new Bgra5551(Vector4.One * -1234.0f).ToVector4());
            Assert.Equal(Vector4.One, new Bgra5551(Vector4.One * 1234.0f).ToVector4());

            // Test Ordering
            float x = 0x1a;
            float y = 0x16;
            float z = 0xd;
            float w = 0x1;
            Assert.Equal(0xeacd, new Bgra5551(x / 0x1f, y / 0x1f, z / 0x1f, w).PackedValue);
            x = 0.1f;
            y = -0.3f;
            z = 0.5f;
            w = -0.7f;
            Assert.Equal(3088, new Bgra5551(x, y, z, w).PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Bgra5551(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 24, 0, 131 });

            new Bgra5551(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 24, 0, 131, 0 });

            new Bgra5551(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 131, 0, 24 });

            new Bgra5551(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 131, 0, 24, 0 });
        }

        [Fact]
        public void Byte4()
        {
            // Test the limits.
            Assert.Equal((uint)0x0, new Byte4(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFFFFFF, new Byte4(Vector4.One * 255).PackedValue);

            // Test ToVector4.
            Assert.True(Equal(Vector4.One * 255, new Byte4(Vector4.One * 255).ToVector4()));
            Assert.True(Equal(Vector4.Zero, new Byte4(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.UnitX * 255, new Byte4(Vector4.UnitX * 255).ToVector4()));
            Assert.True(Equal(Vector4.UnitY * 255, new Byte4(Vector4.UnitY * 255).ToVector4()));
            Assert.True(Equal(Vector4.UnitZ * 255, new Byte4(Vector4.UnitZ * 255).ToVector4()));
            Assert.True(Equal(Vector4.UnitW * 255, new Byte4(Vector4.UnitW * 255).ToVector4()));

            // Test clamping.
            Assert.True(Equal(Vector4.Zero, new Byte4(Vector4.One * -1234.0f).ToVector4()));
            Assert.True(Equal(Vector4.One * 255, new Byte4(Vector4.One * 1234.0f).ToVector4()));

            // Test ordering
            float x = 0x2d;
            float y = 0x36;
            float z = 0x7b;
            float w = 0x1a;
            Assert.Equal((uint)0x1a7b362d, new Byte4(x, y, z, w).PackedValue);

            x = 127.5f;
            y = -12.3f;
            z = 0.5f;
            w = -0.7f;
            Assert.Equal((uint)128, new Byte4(x, y, z, w).PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Byte4(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 128, 0, 0 });

            new Byte4(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 128, 0, 0, 0 });

            new Byte4(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 0, 128 });

            new Byte4(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 0, 128, 0 });

            Byte4 r = new Byte4();
            r.PackFromBytes(20, 38, 0, 255);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 20, 38, 0, 255 });
        }

        [Fact]
        public void HalfSingle()
        {
            // Test limits
            Assert.Equal(15360, new HalfSingle(1F).PackedValue);
            Assert.Equal(0, new HalfSingle(0F).PackedValue);
            Assert.Equal(48128, new HalfSingle(-1F).PackedValue);

            // Test values
            Assert.Equal(11878, new HalfSingle(0.1F).PackedValue);
            Assert.Equal(46285, new HalfSingle(-0.3F).PackedValue);

            // Test ordering
            float x = .5F;
            Assert.True(Equal(new Vector4(x, 0, 0, 1), new HalfSingle(x).ToVector4()));

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new HalfSingle(x).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 128, 0, 0 });

            new HalfSingle(x).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 128, 0, 0, 255 });

            new HalfSingle(x).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 0, 128 });

            new HalfSingle(x).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 0, 128, 255 });
        }

        [Fact]
        public void HalfVector2()
        {
            // Test PackedValue
            Assert.Equal(0u, new HalfVector2(Vector2.Zero).PackedValue);
            Assert.Equal(1006648320u, new HalfVector2(Vector2.One).PackedValue);
            Assert.Equal(3033345638u, new HalfVector2(0.1f, -0.3f).PackedValue);

            // Test ToVector2
            Assert.True(Equal(Vector2.Zero, new HalfVector2(Vector2.Zero).ToVector2()));
            Assert.True(Equal(Vector2.One, new HalfVector2(Vector2.One).ToVector2()));
            Assert.True(Equal(Vector2.UnitX, new HalfVector2(Vector2.UnitX).ToVector2()));
            Assert.True(Equal(Vector2.UnitY, new HalfVector2(Vector2.UnitY).ToVector2()));

            // Test ordering
            float x = .5F;
            float y = .25F;
            Assert.True(Equal(new Vector4(x, y, 0, 1), new HalfVector2(x, y).ToVector4()));

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new HalfVector2(x, y).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 128, 64, 0 });

            new HalfVector2(x, y).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 128, 64, 0, 255 });

            new HalfVector2(x, y).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 64, 128 });

            new HalfVector2(x, y).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 64, 128, 255 });
        }

        [Fact]
        public void HalfVector4()
        {
            // Test PackedValue
            Assert.Equal(0uL, new HalfVector4(Vector4.Zero).PackedValue);
            Assert.Equal(4323521613979991040uL, new HalfVector4(Vector4.One).PackedValue);
            Assert.Equal(13547034390470638592uL, new HalfVector4(-Vector4.One).PackedValue);
            Assert.Equal(15360uL, new HalfVector4(Vector4.UnitX).PackedValue);
            Assert.Equal(1006632960uL, new HalfVector4(Vector4.UnitY).PackedValue);
            Assert.Equal(65970697666560uL, new HalfVector4(Vector4.UnitZ).PackedValue);
            Assert.Equal(4323455642275676160uL, new HalfVector4(Vector4.UnitW).PackedValue);
            Assert.Equal(4035285078724390502uL, new HalfVector4(0.1f, 0.3f, 0.4f, 0.5f).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.Zero, new HalfVector4(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.One, new HalfVector4(Vector4.One).ToVector4()));
            Assert.True(Equal(-Vector4.One, new HalfVector4(-Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.UnitX, new HalfVector4(Vector4.UnitX).ToVector4()));
            Assert.True(Equal(Vector4.UnitY, new HalfVector4(Vector4.UnitY).ToVector4()));
            Assert.True(Equal(Vector4.UnitZ, new HalfVector4(Vector4.UnitZ).ToVector4()));
            Assert.True(Equal(Vector4.UnitW, new HalfVector4(Vector4.UnitW).ToVector4()));

            // Test ordering
            float x = .25F;
            float y = .5F;
            float z = .75F;
            float w = 1F;

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new HalfVector4(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 64, 128, 191 });

            new HalfVector4(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 64, 128, 191, 255 });

            new HalfVector4(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 191, 128, 64 });

            new HalfVector4(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 191, 128, 64, 255 });
        }

        [Fact]
        public void NormalizedByte2()
        {
            // Test PackedValue
            Assert.Equal(0x0, new NormalizedByte2(Vector2.Zero).PackedValue);
            Assert.Equal(0x7F7F, new NormalizedByte2(Vector2.One).PackedValue);
            Assert.Equal(0x8181, new NormalizedByte2(-Vector2.One).PackedValue);

            // Test ToVector2
            Assert.True(Equal(Vector2.One, new NormalizedByte2(Vector2.One).ToVector2()));
            Assert.True(Equal(Vector2.Zero, new NormalizedByte2(Vector2.Zero).ToVector2()));
            Assert.True(Equal(-Vector2.One, new NormalizedByte2(-Vector2.One).ToVector2()));
            Assert.True(Equal(Vector2.One, new NormalizedByte2(Vector2.One * 1234.0f).ToVector2()));
            Assert.True(Equal(-Vector2.One, new NormalizedByte2(Vector2.One * -1234.0f).ToVector2()));

            // Test ToVector4
            Assert.True(Equal(new Vector4(1, 1, 0, 1), new NormalizedByte2(Vector2.One).ToVector4()));
            Assert.True(Equal(new Vector4(0, 0, 0, 1), new NormalizedByte2(Vector2.Zero).ToVector4()));

            // Test Ordering
            float x = 0.1f;
            float y = -0.3f;
            Assert.Equal(0xda0d, new NormalizedByte2(x, y).PackedValue);
            NormalizedByte2 n = new NormalizedByte2();
            n.PackFromBytes(141, 90, 0, 0);
            Assert.Equal(0xda0d, n.PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new NormalizedByte2(x, y).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 141, 90, 0 });

            new NormalizedByte2(x, y).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 141, 90, 0, 255 });

            new NormalizedByte2(x, y).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 90, 141 });

            new NormalizedByte2(x, y).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 90, 141, 255 });
        }

        [Fact]
        public void NormalizedByte4()
        {
            // Test PackedValue
            Assert.Equal((uint)0x0, new NormalizedByte4(Vector4.Zero).PackedValue);
            Assert.Equal((uint)0x7F7F7F7F, new NormalizedByte4(Vector4.One).PackedValue);
            Assert.Equal(0x81818181, new NormalizedByte4(-Vector4.One).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.One, new NormalizedByte4(Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.Zero, new NormalizedByte4(Vector4.Zero).ToVector4()));
            Assert.True(Equal(-Vector4.One, new NormalizedByte4(-Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.One, new NormalizedByte4(Vector4.One * 1234.0f).ToVector4()));
            Assert.True(Equal(-Vector4.One, new NormalizedByte4(Vector4.One * -1234.0f).ToVector4()));

            // Test Ordering
            float x = 0.1f;
            float y = -0.3f;
            float z = 0.5f;
            float w = -0.7f;
            Assert.Equal(0xA740DA0D, new NormalizedByte4(x, y, z, w).PackedValue);
            NormalizedByte4 n = new NormalizedByte4();
            n.PackFromBytes(141, 90, 192, 39);
            Assert.Equal(0xA740DA0D, n.PackedValue);

            Assert.Equal((uint)958796544, new NormalizedByte4(0.0008f, 0.15f, 0.30f, 0.45f).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new NormalizedByte4(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 141, 90, 192 });

            new NormalizedByte4(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 141, 90, 192, 39 });

            new NormalizedByte4(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 192, 90, 141 });

            new NormalizedByte4(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 192, 90, 141, 39 });

            // http://community.monogame.net/t/normalizedbyte4-texture2d-gives-different-results-from-xna/8012/8
            NormalizedByte4 r = new NormalizedByte4();
            r.PackFromBytes(9, 115, 202, 127);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 9, 115, 202, 127 });

            r.PackedValue = 0xff4af389;
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 9, 115, 202, 127 });
        }

        [Fact]
        public void NormalizedShort2()
        {
            Assert.Equal((uint)0x0, new NormalizedShort2(Vector2.Zero).PackedValue);
            Assert.Equal((uint)0x7FFF7FFF, new NormalizedShort2(Vector2.One).PackedValue);
            Assert.Equal(0x80018001, new NormalizedShort2(-Vector2.One).PackedValue);

            Assert.True(Equal(Vector2.One, new NormalizedShort2(Vector2.One).ToVector2()));
            Assert.True(Equal(Vector2.Zero, new NormalizedShort2(Vector2.Zero).ToVector2()));
            Assert.True(Equal(-Vector2.One, new NormalizedShort2(-Vector2.One).ToVector2()));
            Assert.True(Equal(Vector2.One, new NormalizedShort2(Vector2.One * 1234.0f).ToVector2()));
            Assert.True(Equal(-Vector2.One, new NormalizedShort2(Vector2.One * -1234.0f).ToVector2()));

            Assert.True(Equal(new Vector4(1, 1, 0, 1), (new NormalizedShort2(Vector2.One)).ToVector4()));
            Assert.True(Equal(new Vector4(0, 0, 0, 1), (new NormalizedShort2(Vector2.Zero)).ToVector4()));

            // Test Ordering
            float x = 0.35f;
            float y = -0.2f;
            Assert.Equal(0xE6672CCC, new NormalizedShort2(x, y).PackedValue);
            x = 0.1f;
            y = -0.3f;
            Assert.Equal(3650751693, new NormalizedShort2(x, y).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            NormalizedShort2 n = new NormalizedShort2();
            n.PackFromBytes(141, 90, 0, 0);
            n.ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 141, 90, 0 });

            // TODO: I don't think this can ever pass since the bytes are already truncated.
            // Assert.Equal(3650751693, n.PackedValue);

            new NormalizedShort2(x, y).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 141, 90, 0 });

            new NormalizedShort2(x, y).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 141, 90, 0, 255 });

            new NormalizedShort2(x, y).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 90, 141 });

            new NormalizedShort2(x, y).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 90, 141, 255 });
        }

        [Fact]
        public void NormalizedShort4()
        {
            // Test PackedValue
            Assert.Equal((ulong)0x0, new NormalizedShort4(Vector4.Zero).PackedValue);
            Assert.Equal((ulong)0x7FFF7FFF7FFF7FFF, new NormalizedShort4(Vector4.One).PackedValue);
            Assert.Equal(0x8001800180018001, new NormalizedShort4(-Vector4.One).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.One, new NormalizedShort4(Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.Zero, new NormalizedShort4(Vector4.Zero).ToVector4()));
            Assert.True(Equal(-Vector4.One, new NormalizedShort4(-Vector4.One).ToVector4()));
            Assert.True(Equal(Vector4.One, new NormalizedShort4(Vector4.One * 1234.0f).ToVector4()));
            Assert.True(Equal(-Vector4.One, new NormalizedShort4(Vector4.One * -1234.0f).ToVector4()));

            // Test Ordering
            float x = 0.1f;
            float y = -0.3f;
            float z = 0.5f;
            float w = -0.7f;
            Assert.Equal(0xa6674000d99a0ccd, new NormalizedShort4(x, y, z, w).PackedValue);
            Assert.Equal((ulong)4150390751449251866, new NormalizedShort4(0.0008f, 0.15f, 0.30f, 0.45f).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new NormalizedShort4(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 141, 90, 192 });

            new NormalizedShort4(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 141, 90, 192, 39 });

            new NormalizedShort4(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 192, 90, 141 });

            new NormalizedShort4(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 192, 90, 141, 39 });

            NormalizedShort4 r = new NormalizedShort4();
            r.PackFromBytes(9, 115, 202, 127);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 9, 115, 202, 127 });
        }

        [Fact]
        public void Rg32()
        {
            // Test the limits.
            Assert.Equal((uint)0x0, new Rg32(Vector2.Zero).PackedValue);
            Assert.Equal(0xFFFFFFFF, new Rg32(Vector2.One).PackedValue);

            // Test ToVector2
            Assert.True(Equal(Vector2.Zero, new Rg32(Vector2.Zero).ToVector2()));
            Assert.True(Equal(Vector2.One, new Rg32(Vector2.One).ToVector2()));

            // Test clamping.
            Assert.True(Equal(Vector2.Zero, new Rg32(Vector2.One * -1234.0f).ToVector2()));
            Assert.True(Equal(Vector2.One, new Rg32(Vector2.One * 1234.0f).ToVector2()));

            // Test Ordering
            float x = 0xb6dc;
            float y = 0xA59f;
            Assert.Equal(0xa59fb6dc, new Rg32(x / 0xffff, y / 0xffff).PackedValue);
            x = 0.1f;
            y = -0.3f;
            Assert.Equal((uint)6554, new Rg32(x, y).PackedValue);

            // Test ordering
            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Rg32(x, y).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 25, 0, 0 });

            new Rg32(x, y).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 25, 0, 0, 255 });

            new Rg32(x, y).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 0, 25 });

            new Rg32(x, y).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 0, 25, 255 });
        }

        [Fact]
        public void Rgba1010102()
        {
            // Test the limits.
            Assert.Equal((uint)0x0, new Rgba1010102(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFFFFFF, new Rgba1010102(Vector4.One).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.Zero, new Rgba1010102(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.One, new Rgba1010102(Vector4.One).ToVector4()));

            // Test clamping.
            Assert.True(Equal(Vector4.Zero, new Rgba1010102(Vector4.One * -1234.0f).ToVector4()));
            Assert.True(Equal(Vector4.One, new Rgba1010102(Vector4.One * 1234.0f).ToVector4()));

            // Test Ordering
            float x = 0x2db;
            float y = 0x36d;
            float z = 0x3b7;
            float w = 0x1;
            Assert.Equal((uint)0x7B7DB6DB, new Rgba1010102(x / 0x3ff, y / 0x3ff, z / 0x3ff, w / 3).PackedValue);
            x = 0.1f;
            y = -0.3f;
            z = 0.5f;
            w = -0.7f;
            Assert.Equal((uint)536871014, new Rgba1010102(x, y, z, w).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Rgba1010102(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 25, 0, 128 });

            new Rgba1010102(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 25, 0, 128, 0 });

            new Rgba1010102(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 128, 0, 25 });

            new Rgba1010102(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 128, 0, 25, 0 });

            // Alpha component accuracy will be awful.
            Rgba1010102 r = new Rgba1010102();
            r.PackFromBytes(25, 0, 128, 0);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 25, 0, 128, 0 });
        }

        [Fact]
        public void Rgba64()
        {
            // Test the limits.
            Assert.Equal((ulong)0x0, new Rgba64(Vector4.Zero).PackedValue);
            Assert.Equal(0xFFFFFFFFFFFFFFFF, new Rgba64(Vector4.One).PackedValue);

            // Test ToVector4
            Assert.True(Equal(Vector4.Zero, new Rgba64(Vector4.Zero).ToVector4()));
            Assert.True(Equal(Vector4.One, new Rgba64(Vector4.One).ToVector4()));

            // Test clamping.
            Assert.True(Equal(Vector4.Zero, new Rgba64(Vector4.One * -1234.0f).ToVector4()));
            Assert.True(Equal(Vector4.One, new Rgba64(Vector4.One * 1234.0f).ToVector4()));

            // Test data ordering
            Assert.Equal(0xC7AD8F5C570A1EB8, new Rgba64(((float)0x1EB8) / 0xffff, ((float)0x570A) / 0xffff, ((float)0x8F5C) / 0xffff, ((float)0xC7AD) / 0xffff).PackedValue);
            Assert.Equal(0xC7AD8F5C570A1EB8, new Rgba64(0.12f, 0.34f, 0.56f, 0.78f).PackedValue);

            float x = 0.08f;
            float y = 0.15f;
            float z = 0.30f;
            float w = 0.45f;
            Assert.Equal((ulong)0x73334CCC2666147B, new Rgba64(x, y, z, w).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Rgba64(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 20, 38, 76 });

            new Rgba64(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 20, 38, 76, 115 });

            new Rgba64(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 76, 38, 20 });

            new Rgba64(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 76, 38, 20, 115 });

            Rgba64 r = new Rgba64();
            r.PackFromBytes(20, 38, 76, 115);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 20, 38, 76, 115 });
        }

        [Fact]
        public void Short2()
        {
            // Test the limits.
            Assert.Equal((uint)0x0, new Short2(Vector2.Zero).PackedValue);
            Assert.Equal((uint)0x7FFF7FFF, new Short2(Vector2.One * 0x7FFF).PackedValue);
            Assert.Equal(0x80008000, new Short2(Vector2.One * -0x8000).PackedValue);

            // Test ToVector2.
            Assert.True(Equal(Vector2.One * 0x7FFF, new Short2(Vector2.One * 0x7FFF).ToVector2()));
            Assert.True(Equal(Vector2.Zero, new Short2(Vector2.Zero).ToVector2()));
            Assert.True(Equal(Vector2.One * -0x8000, new Short2(Vector2.One * -0x8000).ToVector2()));
            Assert.True(Equal(Vector2.UnitX * 0x7FFF, new Short2(Vector2.UnitX * 0x7FFF).ToVector2()));
            Assert.True(Equal(Vector2.UnitY * 0x7FFF, new Short2(Vector2.UnitY * 0x7FFF).ToVector2()));

            // Test clamping.
            Assert.True(Equal(Vector2.One * 0x7FFF, new Short2(Vector2.One * 1234567.0f).ToVector2()));
            Assert.True(Equal(Vector2.One * -0x8000, new Short2(Vector2.One * -1234567.0f).ToVector2()));

            // Test ToVector4.
            Assert.True(Equal(new Vector4(0x7FFF, 0x7FFF, 0, 1), (new Short2(Vector2.One * 0x7FFF)).ToVector4()));
            Assert.True(Equal(new Vector4(0, 0, 0, 1), (new Short2(Vector2.Zero)).ToVector4()));
            Assert.True(Equal(new Vector4(-0x8000, -0x8000, 0, 1), (new Short2(Vector2.One * -0x8000)).ToVector4()));

            // Test ordering
            float x = 0x2db1;
            float y = 0x361d;
            Assert.Equal((uint)0x361d2db1, new Short2(x, y).PackedValue);
            x = 127.5f;
            y = -5.3f;
            Assert.Equal(4294639744, new Short2(x, y).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Short2(x, y).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 128, 127, 0 });

            new Short2(x, y).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 128, 127, 0, 255 });

            new Short2(x, y).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 0, 127, 128 });

            new Short2(x, y).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 0, 127, 128, 255 });

            Short2 r = new Short2();
            r.PackFromBytes(20, 38, 0, 255);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 20, 38, 0, 255 });
        }

        [Fact]
        public void Short4()
        {
            // Test the limits.
            Assert.Equal((ulong)0x0, new Short4(Vector4.Zero).PackedValue);
            Assert.Equal((ulong)0x7FFF7FFF7FFF7FFF, new Short4(Vector4.One * 0x7FFF).PackedValue);
            Assert.Equal(0x8000800080008000, new Short4(Vector4.One * -0x8000).PackedValue);

            // Test ToVector4.
            Assert.Equal(Vector4.One * 0x7FFF, new Short4(Vector4.One * 0x7FFF).ToVector4());
            Assert.Equal(Vector4.Zero, new Short4(Vector4.Zero).ToVector4());
            Assert.Equal(Vector4.One * -0x8000, new Short4(Vector4.One * -0x8000).ToVector4());
            Assert.Equal(Vector4.UnitX * 0x7FFF, new Short4(Vector4.UnitX * 0x7FFF).ToVector4());
            Assert.Equal(Vector4.UnitY * 0x7FFF, new Short4(Vector4.UnitY * 0x7FFF).ToVector4());
            Assert.Equal(Vector4.UnitZ * 0x7FFF, new Short4(Vector4.UnitZ * 0x7FFF).ToVector4());
            Assert.Equal(Vector4.UnitW * 0x7FFF, new Short4(Vector4.UnitW * 0x7FFF).ToVector4());

            // Test clamping.
            Assert.Equal(Vector4.One * 0x7FFF, new Short4(Vector4.One * 1234567.0f).ToVector4());
            Assert.Equal(Vector4.One * -0x8000, new Short4(Vector4.One * -1234567.0f).ToVector4());

            // Test Ordering
            float x = 0.1f;
            float y = -0.3f;
            float z = 0.5f;
            float w = -0.7f;
            Assert.Equal(18446462598732840960, new Short4(x, y, z, w).PackedValue);

            x = 11547;
            y = 12653;
            z = 29623;
            w = 193;
            Assert.Equal((ulong)0x00c173b7316d2d1b, new Short4(x, y, z, w).PackedValue);

            byte[] rgb = new byte[3];
            byte[] rgba = new byte[4];
            byte[] bgr = new byte[3];
            byte[] bgra = new byte[4];

            new Short4(x, y, z, w).ToXyzBytes(rgb, 0);
            Assert.Equal(rgb, new byte[] { 172, 177, 243 });

            new Short4(x, y, z, w).ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 172, 177, 243, 128 });

            new Short4(x, y, z, w).ToZyxBytes(bgr, 0);
            Assert.Equal(bgr, new byte[] { 243, 177, 172 });

            new Short4(x, y, z, w).ToZyxwBytes(bgra, 0);
            Assert.Equal(bgra, new byte[] { 243, 177, 172, 128 });

            Short4 r = new Short4();
            r.PackFromBytes(20, 38, 0, 255);
            r.ToXyzwBytes(rgba, 0);
            Assert.Equal(rgba, new byte[] { 20, 38, 0, 255 });
        }

        // Comparison helpers with small tolerance to allow for floating point rounding during computations.
        public static bool Equal(float a, float b)
        {
            return Math.Abs(a - b) < 1e-5;
        }

        public static bool Equal(Vector2 a, Vector2 b)
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y);
        }

        public static bool Equal(Vector3 a, Vector3 b)
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y) && Equal(a.Z, b.Z);
        }

        public static bool Equal(Vector4 a, Vector4 b)
        {
            return Equal(a.X, b.X) && Equal(a.Y, b.Y) && Equal(a.Z, b.Z) && Equal(a.W, b.W);
        }
    }
}