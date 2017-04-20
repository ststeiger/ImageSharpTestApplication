﻿// <copyright file="DrawBeziers.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Numerics;

    using BenchmarkDotNet.Attributes;

    using CoreColor = ImageSharp.Color;
    using CoreImage = ImageSharp.Image;
    using CorePoint = ImageSharp.Point;

    public class DrawBeziers : BenchmarkBase
    {
        [Benchmark(Baseline = true, Description = "System.Drawing Draw Beziers")]
        public void DrawPathSystemDrawing()
        {
            using (Bitmap destination = new Bitmap(800, 800))
            {

                using (Graphics graphics = Graphics.FromImage(destination))
                {
                    graphics.InterpolationMode = InterpolationMode.Default;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    Pen pen = new Pen(System.Drawing.Color.HotPink, 10);
                    graphics.DrawBeziers(pen, new[] {
                        new PointF(10, 500),
                        new PointF(30, 10),
                        new PointF(240, 30),
                        new PointF(300, 500)
                    });
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    destination.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
        }

        [Benchmark(Description = "ImageSharp Draw Beziers")]
        public void DrawLinesCore()
        {
            using (CoreImage image = new CoreImage(800, 800))
            {
                image.DrawBeziers(
                    CoreColor.HotPink,
                    10,
                    new[] {
                        new Vector2(10, 500),
                        new Vector2(30, 10),
                        new Vector2(240, 30),
                        new Vector2(300, 500)
                    });

                using (MemoryStream ms = new MemoryStream())
                {
                    image.SaveAsBmp(ms);
                }
            }
        }
    }
}