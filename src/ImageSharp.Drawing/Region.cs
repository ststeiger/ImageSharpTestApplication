﻿// <copyright file="Region.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Drawing
{
    /// <summary>
    /// Represents a region of an image.
    /// </summary>
    public abstract class Region
    {
        /// <summary>
        /// Gets the maximum number of intersections to could be returned.
        /// </summary>
        public abstract int MaxIntersections { get; }

        /// <summary>
        /// Gets the bounding box that entirely surrounds this region.
        /// </summary>
        /// <remarks>
        /// This should always contains all possible points returned from either <see cref="ScanX(int, float[], int, int)"/> or <see cref="ScanY(int, float[], int, int)"/>.
        /// </remarks>
        public abstract Rectangle Bounds { get; }

        /// <summary>
        /// Scans the X axis for intersections.
        /// </summary>
        /// <param name="x">The position along the X axis to find intersections.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="length">The length.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The number of intersections found.</returns>
        public abstract int ScanX(int x, float[] buffer, int length, int offset);

        /// <summary>
        /// Scans the Y axis for intersections.
        /// </summary>
        /// <param name="y">The position along the y axis to find intersections.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="length">The length.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The number of intersections found.</returns>
        public abstract int ScanY(int y, float[] buffer, int length, int offset);
    }
}