﻿// <copyright file="FlipProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Processing.Processors
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods that allow the flipping of an image around its center point.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public class FlipProcessor<TColor> : ImageProcessor<TColor>
        where TColor : struct, IPixel<TColor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipProcessor{TColor}"/> class.
        /// </summary>
        /// <param name="flipType">The <see cref="FlipType"/> used to perform flipping.</param>
        public FlipProcessor(FlipType flipType)
        {
            this.FlipType = flipType;
        }

        /// <summary>
        /// Gets the <see cref="FlipType"/> used to perform flipping.
        /// </summary>
        public FlipType FlipType { get; }

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<TColor> source, Rectangle sourceRectangle)
        {
            switch (this.FlipType)
            {
                // No default needed as we have already set the pixels.
                case FlipType.Vertical:
                    this.FlipX(source);
                    break;
                case FlipType.Horizontal:
                    this.FlipY(source);
                    break;
            }
        }

        /// <summary>
        /// Swaps the image at the X-axis, which goes horizontally through the middle
        /// at half the height of the image.
        /// </summary>
        /// <param name="source">The source image to apply the process to.</param>
        private void FlipX(ImageBase<TColor> source)
        {
            int width = source.Width;
            int height = source.Height;
            int halfHeight = (int)Math.Ceiling(source.Height * .5F);

            using (PixelAccessor<TColor> targetPixels = new PixelAccessor<TColor>(width, height))
            {
                using (PixelAccessor<TColor> sourcePixels = source.Lock())
                {
                    Parallel.For(
                        0,
                        halfHeight,
                        this.ParallelOptions,
                        y =>
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    int newY = height - y - 1;
                                    targetPixels[x, y] = sourcePixels[x, newY];
                                    targetPixels[x, newY] = sourcePixels[x, y];
                                }
                            });
                }

                source.SwapPixelsBuffers(targetPixels);
            }
        }

        /// <summary>
        /// Swaps the image at the Y-axis, which goes vertically through the middle
        /// at half of the width of the image.
        /// </summary>
        /// <param name="source">The source image to apply the process to.</param>
        private void FlipY(ImageBase<TColor> source)
        {
            int width = source.Width;
            int height = source.Height;
            int halfWidth = (int)Math.Ceiling(width * .5F);

            using (PixelAccessor<TColor> targetPixels = new PixelAccessor<TColor>(width, height))
            {
                using (PixelAccessor<TColor> sourcePixels = source.Lock())
                {
                    Parallel.For(
                        0,
                        height,
                        this.ParallelOptions,
                        y =>
                            {
                                for (int x = 0; x < halfWidth; x++)
                                {
                                    int newX = width - x - 1;
                                    targetPixels[x, y] = sourcePixels[newX, y];
                                    targetPixels[newX, y] = sourcePixels[x, y];
                                }
                            });
                }

                source.SwapPixelsBuffers(targetPixels);
            }
        }
    }
}