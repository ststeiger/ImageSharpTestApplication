﻿// <copyright file="ArrayCopy.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>
namespace ImageSharp.Benchmarks.General
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    using BenchmarkDotNet.Attributes;

    [Config(typeof(Config.Short))]
    public class ArrayCopy
    {
        [Params(10, 100, 1000, 10000)]
        public int Count { get; set; }

        byte[] source;

        byte[] destination;

        [Setup]
        public void SetUp()
        {
            this.source = new byte[this.Count];
            this.destination = new byte[this.Count];
        }

        [Benchmark(Baseline = true, Description = "Copy using Array.Copy()")]
        public void CopyArray()
        {
            Array.Copy(this.source, this.destination, this.Count);
        }

        [Benchmark(Description = "Copy using Unsafe<T>")]
        public unsafe void CopyUnsafe()
        {
            fixed (byte* pinnedDestination = this.destination)
            fixed (byte* pinnedSource = this.source)
            {
                Unsafe.CopyBlock(pinnedSource, pinnedDestination, (uint)this.Count);
            }
        }

        [Benchmark(Description = "Copy using Buffer.BlockCopy()")]
        public void CopyUsingBufferBlockCopy()
        {
            Buffer.BlockCopy(this.source, 0, this.destination, 0, this.Count);
        }

        [Benchmark(Description = "Copy using Buffer.MemoryCopy<T>")]
        public unsafe void CopyUsingBufferMemoryCopy()
        {
            fixed (byte* pinnedDestination = this.destination)
            fixed (byte* pinnedSource = this.source)
            {
                Buffer.MemoryCopy(pinnedSource, pinnedDestination, this.Count, this.Count);
            }
        }


        [Benchmark(Description = "Copy using Marshal.Copy<T>")]
        public unsafe void CopyUsingMarshalCopy()
        {
            fixed (byte* pinnedDestination = this.destination)
            {
                Marshal.Copy(this.source, 0, (IntPtr)pinnedDestination, this.Count);
            }
        }
    }
}
