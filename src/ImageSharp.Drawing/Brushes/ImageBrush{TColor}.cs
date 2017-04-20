﻿// <copyright file="ImageBrush{TColor}.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Drawing.Brushes
{
    using System;
    using System.Numerics;

    using Processors;

    /// <summary>
    /// Provides an implementation of an image brush for painting images within areas.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    public class ImageBrush<TColor> : IBrush<TColor>
    where TColor : struct, IPixel<TColor>
    {
        /// <summary>
        /// The image to paint.
        /// </summary>
        private readonly IImageBase<TColor> image;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBrush{TColor}"/> class.
        /// </summary>
        /// <param name="image">The image.</param>
        public ImageBrush(IImageBase<TColor> image)
        {
            this.image = image;
        }

        /// <inheritdoc />
        public BrushApplicator<TColor> CreateApplicator(PixelAccessor<TColor> sourcePixels, RectangleF region)
        {
            return new ImageBrushApplicator(this.image, region);
        }

        /// <summary>
        /// The image brush applicator.
        /// </summary>
        private class ImageBrushApplicator : BrushApplicator<TColor>
        {
            /// <summary>
            /// The source pixel accessor.
            /// </summary>
            private readonly PixelAccessor<TColor> source;

            /// <summary>
            /// The y-length.
            /// </summary>
            private readonly int yLength;

            /// <summary>
            /// The x-length.
            /// </summary>
            private readonly int xLength;

            /// <summary>
            /// The offset.
            /// </summary>
            private readonly Vector2 offset;

            /// <summary>
            /// Initializes a new instance of the <see cref="ImageBrushApplicator"/> class.
            /// </summary>
            /// <param name="image">
            /// The image.
            /// </param>
            /// <param name="region">
            /// The region.
            /// </param>
            public ImageBrushApplicator(IImageBase<TColor> image, RectangleF region)
            {
                this.source = image.Lock();
                this.xLength = image.Width;
                this.yLength = image.Height;
                this.offset = new Vector2((float)Math.Max(Math.Floor(region.Top), 0), (float)Math.Max(Math.Floor(region.Left), 0));
            }

            /// <summary>
            /// Gets the color for a single pixel.
            /// </summary>
            /// <param name="x">The x.</param>
            /// <param name="y">The y.</param>
            /// <returns>
            /// The color
            /// </returns>
            public override TColor this[int x, int y]
            {
                get
                {
                    Vector2 point = new Vector2(x, y);

                    // Offset the requested pixel by the value in the rectangle (the shapes position)
                    point = point - this.offset;
                    x = (int)point.X % this.xLength;
                    y = (int)point.Y % this.yLength;

                    return this.source[x, y];
                }
            }

            /// <inheritdoc />
            public override void Dispose()
            {
                this.source.Dispose();
            }
        }
    }
}