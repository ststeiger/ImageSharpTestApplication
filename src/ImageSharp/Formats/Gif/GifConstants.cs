﻿// <copyright file="GifConstants.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Formats
{
    using System.Text;

    /// <summary>
    /// Constants that define specific points within a gif.
    /// </summary>
    internal static class GifConstants
    {
        /// <summary>
        /// The file type.
        /// </summary>
        public const string FileType = "GIF";

        /// <summary>
        /// The file version.
        /// </summary>
        public const string FileVersion = "89a";

        /// <summary>
        /// The extension block introducer <value>!</value>.
        /// </summary>
        public const byte ExtensionIntroducer = 0x21;

        /// <summary>
        /// The graphic control label.
        /// </summary>
        public const byte GraphicControlLabel = 0xF9;

        /// <summary>
        /// The application extension label.
        /// </summary>
        public const byte ApplicationExtensionLabel = 0xFF;

        /// <summary>
        /// The application identification.
        /// </summary>
        public const string ApplicationIdentification = "NETSCAPE2.0";

        /// <summary>
        /// The application block size.
        /// </summary>
        public const byte ApplicationBlockSize = 0x0b;

        /// <summary>
        /// The comment label.
        /// </summary>
        public const byte CommentLabel = 0xFE;

        /// <summary>
        /// The name of the property inside the image properties for the comments.
        /// </summary>
        public const string Comments = "Comments";

        /// <summary>
        /// The maximum comment length.
        /// </summary>
        public const int MaxCommentLength = 1024 * 8;

        /// <summary>
        /// The image descriptor label <value>,</value>.
        /// </summary>
        public const byte ImageDescriptorLabel = 0x2C;

        /// <summary>
        /// The plain text label.
        /// </summary>
        public const byte PlainTextLabel = 0x01;

        /// <summary>
        /// The image label introducer <value>,</value>.
        /// </summary>
        public const byte ImageLabel = 0x2C;

        /// <summary>
        /// The terminator.
        /// </summary>
        public const byte Terminator = 0;

        /// <summary>
        /// The end introducer trailer <value>;</value>.
        /// </summary>
        public const byte EndIntroducer = 0x3B;

        /// <summary>
        /// Gets the default encoding to use when reading comments.
        /// </summary>
        public static Encoding DefaultEncoding { get; } = Encoding.GetEncoding("ASCII");
    }
}
