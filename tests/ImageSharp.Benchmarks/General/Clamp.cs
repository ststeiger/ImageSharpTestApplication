﻿// <copyright file="Clamp.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageSharp.Benchmarks.General
{
    using System;
    using System.Runtime.CompilerServices;

    using BenchmarkDotNet.Attributes;

    public class Clamp
    {
        [Params(-1, 0, 255, 256)]
        public int Value { get; set; }

        [Benchmark(Baseline = true, Description = "Maths Clamp")]
        public byte ClampMaths()
        {
           int value = this.Value;
           return (byte)Math.Min(Math.Max(byte.MinValue, value), byte.MaxValue);
        }

        [Benchmark(Description = "No Maths Clamp")]
        public byte ClampNoMaths()
        {
           int value = this.Value;
           value = value >= byte.MaxValue ? byte.MaxValue : value;
           return (byte)(value <= byte.MinValue ? byte.MinValue : value);
        }

        [Benchmark(Description = "No Maths No Equals Clamp")]
        public byte ClampNoMathsNoEquals()
        {
           int value = this.Value;
           value = value > byte.MaxValue ? byte.MaxValue : value;
           return (byte)(value < byte.MinValue ? byte.MinValue : value);
        }

        [Benchmark(Description = "No Maths Clamp No Ternary")]
        public byte ClampNoMathsNoTernary()
        {
            int value = this.Value;

            if (value >= byte.MaxValue)
            {
                return byte.MaxValue;
            }

            if (value <= byte.MinValue)
            {
                return byte.MinValue;
            }

            return (byte)value;
        }

        [Benchmark(Description = "No Maths No Equals Clamp No Ternary")]
        public byte ClampNoMathsEqualsNoTernary()
        {
           int value = this.Value;

           if (value > byte.MaxValue)
           {
               return byte.MaxValue;
           }

           if (value < byte.MinValue)
           {
               return byte.MinValue;
           }

           return (byte)value;
        }

        [Benchmark(Description = "Clamp using Bitwise Abs")]
        public byte ClampBitwise()
        {
            int x = this.Value;
            int absmax = byte.MaxValue - x;
            x = (x + byte.MaxValue - AbsBitwiseVer(ref absmax)) >> 1;
            x = (x + byte.MinValue + AbsBitwiseVer(ref x)) >> 1;

            return (byte)x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int AbsBitwiseVer(ref int x)
        {
            int y = x >> 31;
            return (x ^ y) - y;
        }
    }
}
