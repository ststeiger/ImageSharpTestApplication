﻿// <copyright file="ColorConversionTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Tests.Drawing
{
    using System;
    using System.IO;

    using ImageSharp.Drawing;
    using ImageSharp.Drawing.Brushes;

    using Xunit;

    public class FillPatternBrushTests : FileTestBase
    {
        private void Test(string name, Color background, IBrush<Color> brush, Color[,] expectedPattern)
        {
            string path = this.CreateOutputDirectory("Fill", "PatternBrush");
            using (Image image = new Image(20, 20))
            {
                image
                    .Fill(background)
                    .Fill(brush);

                using (FileStream output = File.OpenWrite($"{path}/{name}.png"))
                {
                    image.Save(output);
                }

                using (PixelAccessor<Color> sourcePixels = image.Lock())
                {
                    // lets pick random spots to start checking
                    Random r = new Random();
                    int xStride = expectedPattern.GetLength(1);
                    int yStride = expectedPattern.GetLength(0);
                    int offsetX = r.Next(image.Width / xStride) * xStride;
                    int offsetY = r.Next(image.Height / yStride) * yStride;
                    for (int x = 0; x < xStride; x++)
                    {
                        for (int y = 0; y < yStride; y++)
                        {
                            int actualX = x + offsetX;
                            int actualY = y + offsetY;
                            Color expected = expectedPattern[y, x]; // inverted pattern
                            Color actual = sourcePixels[actualX, actualY];
                            if (expected != actual)
                            {
                                Assert.True(false, $"Expected {expected} but found {actual} at ({actualX},{actualY})");
                            }
                        }
                    }
                }
                using (FileStream output = File.OpenWrite($"{path}/{name}x4.png"))
                {
                    image.Resize(80, 80).Save(output);
                }
            }
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithPercent10()
        {
            this.Test("Percent10", Color.Blue, Brushes.Percent10(Color.HotPink, Color.LimeGreen),
                new[,]
                {
                { Color.HotPink , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.HotPink , Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen, Color.LimeGreen}
            });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithPercent10Transparent()
        {
            Test("Percent10_Transparent", Color.Blue, Brushes.Percent10(Color.HotPink),
            new Color[,] {
                { Color.HotPink , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.HotPink , Color.Blue},
                { Color.Blue, Color.Blue, Color.Blue, Color.Blue}
            });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithPercent20()
        {
            Test("Percent20", Color.Blue, Brushes.Percent20(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.HotPink , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.HotPink , Color.LimeGreen},
                { Color.HotPink , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.HotPink , Color.LimeGreen}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithPercent20_transparent()
        {
            Test("Percent20_Transparent", Color.Blue, Brushes.Percent20(Color.HotPink),
           new Color[,] {
                { Color.HotPink , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.HotPink , Color.Blue},
                { Color.HotPink , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.HotPink , Color.Blue}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithHorizontal()
        {
            Test("Horizontal", Color.Blue, Brushes.Horizontal(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.LimeGreen , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.HotPink, Color.HotPink, Color.HotPink , Color.HotPink},
                { Color.LimeGreen , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen , Color.LimeGreen}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithHorizontal_transparent()
        {
            Test("Horizontal_Transparent", Color.Blue, Brushes.Horizontal(Color.HotPink),
           new Color[,] {
                { Color.Blue , Color.Blue, Color.Blue, Color.Blue},
                { Color.HotPink, Color.HotPink, Color.HotPink , Color.HotPink},
                { Color.Blue , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.Blue , Color.Blue}
           });
        }



        [Fact]
        public void ImageShouldBeFloodFilledWithMin()
        {
            Test("Min", Color.Blue, Brushes.Min(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.LimeGreen , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen , Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen , Color.LimeGreen},
                { Color.HotPink, Color.HotPink, Color.HotPink , Color.HotPink}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithMin_transparent()
        {
            Test("Min_Transparent", Color.Blue, Brushes.Min(Color.HotPink),
           new Color[,] {
                { Color.Blue , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue , Color.Blue, Color.Blue, Color.Blue},
                { Color.Blue, Color.Blue, Color.Blue , Color.Blue},
                { Color.HotPink, Color.HotPink, Color.HotPink , Color.HotPink},
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithVertical()
        {
            Test("Vertical", Color.Blue, Brushes.Vertical(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithVertical_transparent()
        {
            Test("Vertical_Transparent", Color.Blue, Brushes.Vertical(Color.HotPink),
           new Color[,] {
                { Color.Blue, Color.HotPink, Color.Blue, Color.Blue},
                { Color.Blue, Color.HotPink, Color.Blue, Color.Blue},
                { Color.Blue, Color.HotPink, Color.Blue, Color.Blue},
                { Color.Blue, Color.HotPink, Color.Blue, Color.Blue}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithForwardDiagonal()
        {
            Test("ForwardDiagonal", Color.Blue, Brushes.ForwardDiagonal(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.HotPink, Color.LimeGreen, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.HotPink, Color.LimeGreen},
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen, Color.HotPink}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithForwardDiagonal_transparent()
        {
            Test("ForwardDiagonal_Transparent", Color.Blue, Brushes.ForwardDiagonal(Color.HotPink),
           new Color[,] {
                { Color.HotPink, Color.Blue,    Color.Blue,    Color.Blue},
                { Color.Blue,    Color.HotPink, Color.Blue,    Color.Blue},
                { Color.Blue,    Color.Blue,    Color.HotPink, Color.Blue},
                { Color.Blue,    Color.Blue,    Color.Blue,    Color.HotPink}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithBackwardDiagonal()
        {
            Test("BackwardDiagonal", Color.Blue, Brushes.BackwardDiagonal(Color.HotPink, Color.LimeGreen),
           new Color[,] {
                { Color.LimeGreen, Color.LimeGreen, Color.LimeGreen, Color.HotPink},
                { Color.LimeGreen, Color.LimeGreen, Color.HotPink, Color.LimeGreen},
                { Color.LimeGreen, Color.HotPink, Color.LimeGreen, Color.LimeGreen},
                { Color.HotPink, Color.LimeGreen, Color.LimeGreen, Color.LimeGreen}
           });
        }

        [Fact]
        public void ImageShouldBeFloodFilledWithBackwardDiagonal_transparent()
        {
            Test("BackwardDiagonal_Transparent", Color.Blue, Brushes.BackwardDiagonal(Color.HotPink),
           new Color[,] {
                { Color.Blue, Color.Blue,    Color.Blue,    Color.HotPink},
                { Color.Blue,    Color.Blue, Color.HotPink,    Color.Blue},
                { Color.Blue,    Color.HotPink,    Color.Blue, Color.Blue},
                { Color.HotPink,    Color.Blue,    Color.Blue,    Color.Blue}
           });
        }


    }
}
