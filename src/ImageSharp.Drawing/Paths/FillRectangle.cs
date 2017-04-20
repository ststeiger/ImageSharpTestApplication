﻿// <copyright file="FillRectangle.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;

    using Drawing;
    using Drawing.Brushes;

    /// <summary>
    /// Extension methods for the <see cref="Image{TColor}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Fill<TColor>(this Image<TColor> source, IBrush<TColor> brush, Rectangle shape, GraphicsOptions options)
            where TColor : struct, IPixel<TColor>
        {
            return source.Fill(brush, new SixLabors.Shapes.Rectangle(shape.X, shape.Y, shape.Width, shape.Height), options);
        }

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Fill<TColor>(this Image<TColor> source, IBrush<TColor> brush, Rectangle shape)
            where TColor : struct, IPixel<TColor>
        {
            return source.Fill(brush, new SixLabors.Shapes.Rectangle(shape.X, shape.Y, shape.Width, shape.Height));
        }

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Fill<TColor>(this Image<TColor> source, TColor color, Rectangle shape, GraphicsOptions options)
          where TColor : struct, IPixel<TColor>
        {
            return source.Fill(new SolidBrush<TColor>(color), shape, options);
        }

        /// <summary>
        /// Flood fills the image in the shape of the provided polygon with the specified brush..
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Fill<TColor>(this Image<TColor> source, TColor color, Rectangle shape)
            where TColor : struct, IPixel<TColor>
        {
            return source.Fill(new SolidBrush<TColor>(color), shape);
        }
    }
}
