﻿// <copyright file="ErrorDiffuser.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Dithering
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The base class for performing effor diffusion based dithering.
    /// </summary>
    public abstract class ErrorDiffuser : IErrorDiffuser
    {
        /// <summary>
        /// The vector to perform division.
        /// </summary>
        private readonly Vector4 divisorVector;

        /// <summary>
        /// The matrix width
        /// </summary>
        private readonly int matrixHeight;

        /// <summary>
        /// The matrix height
        /// </summary>
        private readonly int matrixWidth;

        /// <summary>
        /// The offset at which to start the dithering operation.
        /// </summary>
        private readonly int startingOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorDiffuser"/> class.
        /// </summary>
        /// <param name="matrix">The dithering matrix.</param>
        /// <param name="divisor">The divisor.</param>
        protected ErrorDiffuser(Fast2DArray<float> matrix, byte divisor)
        {
            Guard.NotNull(matrix, nameof(matrix));
            Guard.MustBeGreaterThan(divisor, 0, nameof(divisor));

            this.Matrix = matrix;
            this.matrixWidth = this.Matrix.Width;
            this.matrixHeight = this.Matrix.Height;
            this.divisorVector = new Vector4(divisor);

            this.startingOffset = 0;
            for (int i = 0; i < this.matrixWidth; i++)
            {
                // Good to disable here as we are not comparing matematical output.
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (matrix[0, i] != 0)
                {
                    this.startingOffset = (byte)(i - 1);
                    break;
                }
            }
        }

        /// <inheritdoc />
        public Fast2DArray<float> Matrix { get; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dither<TColor>(PixelAccessor<TColor> pixels, TColor source, TColor transformed, int x, int y, int width, int height)
            where TColor : struct, IPixel<TColor>
        {
            // Assign the transformed pixel to the array.
            pixels[x, y] = transformed;

            // Calculate the error
            Vector4 error = source.ToVector4() - transformed.ToVector4();

            // Loop through and distribute the error amongst neighbouring pixels.
            for (int row = 0; row < this.matrixHeight; row++)
            {
                int matrixY = y + row;

                for (int col = 0; col < this.matrixWidth; col++)
                {
                    int matrixX = x + (col - this.startingOffset);

                    if (matrixX > 0 && matrixX < width && matrixY > 0 && matrixY < height)
                    {
                        float coefficient = this.Matrix[row, col];

                        // Good to disable here as we are not comparing matematical output.
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (coefficient == 0)
                        {
                            continue;
                        }

                        Vector4 coefficientVector = new Vector4(coefficient);
                        Vector4 offsetColor = pixels[matrixX, matrixY].ToVector4();
                        Vector4 result = ((error * coefficientVector) / this.divisorVector) + offsetColor;
                        result.W = offsetColor.W;

                        TColor packed = default(TColor);
                        packed.PackFromVector4(result);
                        pixels[matrixX, matrixY] = packed;
                    }
                }
            }
        }
    }
}