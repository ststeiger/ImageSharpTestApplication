﻿// <copyright file="ArrayReverse.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks.General
{
    using System;

    using BenchmarkDotNet.Attributes;

    public class ArrayReverse
    {
        [Params(4, 16, 32)]
        public int Count { get; set; }

        byte[] source;

        byte[] destination;

        [Setup]
        public void SetUp()
        {
            this.source = new byte[this.Count];
            this.destination = new byte[this.Count];
        }

        [Benchmark(Baseline = true, Description = "Copy using Array.Reverse()")]
        public void ReverseArray()
        {
            Array.Reverse(this.source, 0, this.Count);
        }

        [Benchmark(Description = "Reverse using loop")]
        public void ReverseLoop()
        {
            this.ReverseBytes(this.source, 0, this.Count);

            //for (int i = 0; i < this.source.Length / 2; i++)
            //{
            //    byte tmp = this.source[i];
            //    this.source[i] = this.source[this.source.Length - i - 1];
            //    this.source[this.source.Length - i - 1] = tmp;
            //}
        }

        public void ReverseBytes(byte[] source, int index, int length)
        {
            int i = index;
            int j = index + length - 1;
            while (i < j)
            {
                byte temp = source[i];
                source[i] = source[j];
                source[j] = temp;
                i++;
                j--;
            }
        }
    }
}
