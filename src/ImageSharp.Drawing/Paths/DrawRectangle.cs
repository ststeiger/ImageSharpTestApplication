﻿// <copyright file="DrawRectangle.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp
{
    using System;

    using Drawing;
    using Drawing.Brushes;
    using Drawing.Pens;

    /// <summary>
    /// Extension methods for the <see cref="Image{TColor}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Draws the outline of the polygon with the provided pen.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, IPen<TColor> pen, Rectangle shape, GraphicsOptions options)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(pen, new SixLabors.Shapes.Rectangle(shape.X, shape.Y, shape.Width, shape.Height), options);
        }

        /// <summary>
        /// Draws the outline of the polygon with the provided pen.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, IPen<TColor> pen, Rectangle shape)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(pen, shape, GraphicsOptions.Default);
        }

        /// <summary>
        /// Draws the outline of the polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, IBrush<TColor> brush, float thickness, Rectangle shape, GraphicsOptions options)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(new Pen<TColor>(brush, thickness), shape, options);
        }

        /// <summary>
        /// Draws the outline of the polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, IBrush<TColor> brush, float thickness, Rectangle shape)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(new Pen<TColor>(brush, thickness), shape);
        }

        /// <summary>
        /// Draws the outline of the polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="shape">The shape.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, TColor color, float thickness, Rectangle shape, GraphicsOptions options)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(new SolidBrush<TColor>(color), thickness, shape, options);
        }

        /// <summary>
        /// Draws the outline of the polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TColor">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="shape">The shape.</param>
        /// <returns>The <see cref="Image{TColor}"/>.</returns>
        public static Image<TColor> Draw<TColor>(this Image<TColor> source, TColor color, float thickness, Rectangle shape)
           where TColor : struct, IPixel<TColor>
        {
            return source.Draw(new SolidBrush<TColor>(color), thickness, shape);
        }
    }
}
