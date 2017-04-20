﻿// <copyright file="GifDecoderOptions.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats
{
    using System.Text;

    /// <summary>
    /// Encapsulates the options for the <see cref="GifDecoder"/>.
    /// </summary>
    public sealed class GifDecoderOptions : DecoderOptions, IGifDecoderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GifDecoderOptions"/> class.
        /// </summary>
        public GifDecoderOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GifDecoderOptions"/> class.
        /// </summary>
        /// <param name="options">The options for the decoder.</param>
        private GifDecoderOptions(IDecoderOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the encoding that should be used when reading comments.
        /// </summary>
        public Encoding TextEncoding { get; set; } = GifConstants.DefaultEncoding;

        /// <summary>
        /// Converts the options to a <see cref="IGifDecoderOptions"/> instance with a cast
        /// or by creating a new instance with the specfied options.
        /// </summary>
        /// <param name="options">The options for the decoder.</param>
        /// <returns>The options for the <see cref="GifDecoder"/>.</returns>
        internal static IGifDecoderOptions Create(IDecoderOptions options)
        {
            return options as IGifDecoderOptions ?? new GifDecoderOptions(options);
        }
    }
}
