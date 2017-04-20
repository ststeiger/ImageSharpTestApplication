﻿// <copyright file="EdgeDetectorCompassProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Processing.Processors
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a sampler that detects edges within an image using a eight two dimensional matrices.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public abstract class EdgeDetectorCompassProcessor<TColor> : ImageProcessor<TColor>, IEdgeDetectorProcessor<TColor>
        where TColor : struct, IPixel<TColor>
    {
        /// <summary>
        /// Gets the North gradient operator
        /// </summary>
        public abstract Fast2DArray<float> North { get; }

        /// <summary>
        /// Gets the NorthWest gradient operator
        /// </summary>
        public abstract Fast2DArray<float> NorthWest { get; }

        /// <summary>
        /// Gets the West gradient operator
        /// </summary>
        public abstract Fast2DArray<float> West { get; }

        /// <summary>
        /// Gets the SouthWest gradient operator
        /// </summary>
        public abstract Fast2DArray<float> SouthWest { get; }

        /// <summary>
        /// Gets the South gradient operator
        /// </summary>
        public abstract Fast2DArray<float> South { get; }

        /// <summary>
        /// Gets the SouthEast gradient operator
        /// </summary>
        public abstract Fast2DArray<float> SouthEast { get; }

        /// <summary>
        /// Gets the East gradient operator
        /// </summary>
        public abstract Fast2DArray<float> East { get; }

        /// <summary>
        /// Gets the NorthEast gradient operator
        /// </summary>
        public abstract Fast2DArray<float> NorthEast { get; }

        /// <inheritdoc/>
        public bool Grayscale { get; set; }

        /// <inheritdoc/>
        protected override void BeforeApply(ImageBase<TColor> source, Rectangle sourceRectangle)
        {
            if (this.Grayscale)
            {
                new GrayscaleBt709Processor<TColor>().Apply(source, sourceRectangle);
            }
        }

        /// <inheritdoc />
        protected override void OnApply(ImageBase<TColor> source, Rectangle sourceRectangle)
        {
            Fast2DArray<float>[] kernels = { this.North, this.NorthWest, this.West, this.SouthWest, this.South, this.SouthEast, this.East, this.NorthEast };

            int startY = sourceRectangle.Y;
            int endY = sourceRectangle.Bottom;
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;

            // Align start/end positions.
            int minX = Math.Max(0, startX);
            int maxX = Math.Min(source.Width, endX);
            int minY = Math.Max(0, startY);
            int maxY = Math.Min(source.Height, endY);

            // we need a clean copy for each pass to start from
            using (ImageBase<TColor> cleanCopy = new Image<TColor>(source))
            {
                new ConvolutionProcessor<TColor>(kernels[0]).Apply(source, sourceRectangle);

                if (kernels.Length == 1)
                {
                    return;
                }

                int shiftY = startY;
                int shiftX = startX;

                // Reset offset if necessary.
                if (minX > 0)
                {
                    shiftX = 0;
                }

                if (minY > 0)
                {
                    shiftY = 0;
                }

                // Additional runs.
                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 1; i < kernels.Length; i++)
                {
                    using (ImageBase<TColor> pass = new Image<TColor>(cleanCopy))
                    {
                        new ConvolutionProcessor<TColor>(kernels[i]).Apply(pass, sourceRectangle);

                        using (PixelAccessor<TColor> passPixels = pass.Lock())
                        using (PixelAccessor<TColor> targetPixels = source.Lock())
                        {
                            Parallel.For(
                                minY,
                                maxY,
                                this.ParallelOptions,
                                y =>
                                {
                                    int offsetY = y - shiftY;
                                    for (int x = minX; x < maxX; x++)
                                    {
                                        int offsetX = x - shiftX;

                                        // Grab the max components of the two pixels
                                        TColor packed = default(TColor);
                                        packed.PackFromVector4(Vector4.Max(passPixels[offsetX, offsetY].ToVector4(), targetPixels[offsetX, offsetY].ToVector4()));
                                        targetPixels[offsetX, offsetY] = packed;
                                    }
                                });
                        }
                    }
                }
            }
        }
    }
}