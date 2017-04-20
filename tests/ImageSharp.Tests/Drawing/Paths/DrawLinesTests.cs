﻿
namespace ImageSharp.Tests.Drawing.Paths
{
    using System;
    using System.IO;
    using ImageSharp;
    using ImageSharp.Drawing.Brushes;
    using Processing;
    using System.Collections.Generic;
    using Xunit;
    using ImageSharp.Drawing;
    using System.Numerics;
    using SixLabors.Shapes;
    using ImageSharp.Drawing.Processors;
    using ImageSharp.Drawing.Pens;

    public class DrawLinesTests : IDisposable
    {
        float thickness = 7.2f;
        GraphicsOptions noneDefault = new GraphicsOptions();
        Color color = Color.HotPink;
        SolidBrush brush = Brushes.Solid(Color.HotPink);
        Pen pen = new Pen(Color.Gray, 99.9f);
        Vector2[] points = new Vector2[] {
                    new Vector2(10,10),
                    new Vector2(20,10),
                    new Vector2(20,10),
                    new Vector2(30,10),
                };
        private ProcessorWatchingImage img;

        public DrawLinesTests()
        {
            this.img = new Paths.ProcessorWatchingImage(10, 10);
        }

        public void Dispose()
        {
            img.Dispose();
        }

        [Fact]
        public void CorrectlySetsBrushThicknessAndPoints()
        {
            img.DrawLines(brush, thickness, points);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(GraphicsOptions.Default, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Pen<Color> pen = Assert.IsType<Pen<Color>>(processor.Pen);
            Assert.Equal(brush, pen.Brush);
            Assert.Equal(thickness, pen.Width);
        }

        [Fact]
        public void CorrectlySetsBrushThicknessPointsAndOptions()
        {
            img.DrawLines(brush, thickness, points, noneDefault);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(noneDefault, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Pen<Color> pen = Assert.IsType<Pen<Color>>(processor.Pen);
            Assert.Equal(brush, pen.Brush);
            Assert.Equal(thickness, pen.Width);
        }

        [Fact]
        public void CorrectlySetsColorThicknessAndPoints()
        {
            img.DrawLines(color, thickness, points);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(GraphicsOptions.Default, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Pen<Color> pen = Assert.IsType<Pen<Color>>(processor.Pen);
            Assert.Equal(thickness, pen.Width);

            SolidBrush<Color> brush = Assert.IsType<SolidBrush<Color>>(pen.Brush);
            Assert.Equal(color, brush.Color);
        }

        [Fact]
        public void CorrectlySetsColorThicknessPointsAndOptions()
        {
            img.DrawLines(color, thickness, points, noneDefault);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(noneDefault, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Pen<Color> pen = Assert.IsType<Pen<Color>>(processor.Pen);
            Assert.Equal(thickness, pen.Width);

            SolidBrush<Color> brush = Assert.IsType<SolidBrush<Color>>(pen.Brush);
            Assert.Equal(color, brush.Color);
        }

        [Fact]
        public void CorrectlySetsPenAndPoints()
        {
            img.DrawLines(pen, points);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(GraphicsOptions.Default, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Assert.Equal(pen, processor.Pen);
        }

        [Fact]
        public void CorrectlySetsPenPointsAndOptions()
        {
            img.DrawLines(pen, points, noneDefault);

            Assert.NotEmpty(img.ProcessorApplications);
            DrawPathProcessor<Color> processor = Assert.IsType<DrawPathProcessor<Color>>(img.ProcessorApplications[0].processor);

            Assert.Equal(noneDefault, processor.Options);

            ShapePath path = Assert.IsType<ShapePath>(processor.Path);

            SixLabors.Shapes.Path vector = Assert.IsType<SixLabors.Shapes.Path>(path.Path);
            LinearLineSegment segment = Assert.IsType<LinearLineSegment>(vector.LineSegments[0]);

            Assert.Equal(pen, processor.Pen);
        }
    }
}
