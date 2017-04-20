﻿// <copyright file="RobertsCrossProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Processing.Processors
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The Roberts Cross operator filter.
    /// <see href="http://en.wikipedia.org/wiki/Roberts_cross"/>
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    [SuppressMessage("ReSharper", "StaticMemberInGenericType", Justification = "We want to use only one instance of each array field for each generic type.")]
    public class RobertsCrossProcessor<TColor> : EdgeDetector2DProcessor<TColor>
        where TColor : struct, IPixel<TColor>
    {
        /// <summary>
        /// The horizontal gradient operator.
        /// </summary>
        private static readonly Fast2DArray<float> RobertsCrossX =
            new float[,]
            {
                { 1, 0 },
                { 0, -1 }
            };

        /// <summary>
        /// The vertical gradient operator.
        /// </summary>
        private static readonly Fast2DArray<float> RobertsCrossY =
            new float[,]
            {
                { 0, 1 },
                { -1, 0 }
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="RobertsCrossProcessor{TColor}"/> class.
        /// </summary>
        public RobertsCrossProcessor()
            : base(RobertsCrossX, RobertsCrossY)
        {
        }
    }
}
