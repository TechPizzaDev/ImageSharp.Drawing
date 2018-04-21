﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;

using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Drawing.Brushes;

using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Drawing;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Drawing
{
    public class FillLinearGradientBrushTests : FileTestBase
    {
        [Fact]
        public void LinearGradientBrushWithEqualColorsReturnsUnicolorImage()
        {
            string path = TestEnvironment.CreateOutputDirectory("Fill", "LinearGradientBrush");
            using (var image = new Image<Rgba32>(10, 10))
            {
                LinearGradientBrush<Rgba32> unicolorLinearGradientBrush = 
                    new LinearGradientBrush<Rgba32>(
                        new SixLabors.Primitives.Point(0, 0),
                        new SixLabors.Primitives.Point(10, 0),
                        new LinearGradientBrush<Rgba32>.ColorStop(0, Rgba32.Red),
                        new LinearGradientBrush<Rgba32>.ColorStop(1, Rgba32.Red));
                
                image.Mutate(x => x.Fill(unicolorLinearGradientBrush));
                image.Save($"{path}/UnicolorGradient.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Assert.Equal(Rgba32.Red, sourcePixels[0, 0]);
                    Assert.Equal(Rgba32.Red, sourcePixels[9, 9]);
                    Assert.Equal(Rgba32.Red, sourcePixels[5, 5]);
                    Assert.Equal(Rgba32.Red, sourcePixels[3, 8]);
                }
            }
        }
        
        [Fact]
        public void HorizontalLinearGradientBrushReturnsUnicolorColumns()
        {
            int width = 500;
            int height = 10;
            int lastColumnIndex = width - 1;

            string path = TestEnvironment.CreateOutputDirectory("Fill", "LinearGradientBrush");
            using (var image = new Image<Rgba32>(width, height))
            {
                LinearGradientBrush<Rgba32> unicolorLinearGradientBrush = 
                    new LinearGradientBrush<Rgba32>(
                        new SixLabors.Primitives.Point(0, 0),
                        new SixLabors.Primitives.Point(500, 0),
                        new LinearGradientBrush<Rgba32>.ColorStop(0, Rgba32.Red),
                        new LinearGradientBrush<Rgba32>.ColorStop(1, Rgba32.Yellow));
                
                image.Mutate(x => x.Fill(unicolorLinearGradientBrush));
                image.Save($"{path}/horizontalRedToYellow.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Rgba32 columnColor23 = sourcePixels[23, 0];
                    Rgba32 columnColor42 = sourcePixels[42, 0];
                    Rgba32 columnColor333 = sourcePixels[333, 0];
                    
                    for (int i = 0; i < height; i++)
                    {
                        // check first and last column, these are known:
                        Assert.Equal(Rgba32.Red, sourcePixels[0, i]);
                        Assert.Equal(Rgba32.Yellow, sourcePixels[lastColumnIndex, i]);
                        
                        // check the random colors:
                        Assert.Equal(columnColor23, sourcePixels[23, i]);
                        Assert.Equal(columnColor42, sourcePixels[42, i]);
                        Assert.Equal(columnColor333, sourcePixels[333, i]);
                    }
                }
            }
        }

        [Fact]
        public void VerticalLinearGradientBrushReturnsUnicolorColumns()
        {
            int width = 10;
            int height = 500;
            int lastRowIndex = height - 1;

            string path = TestEnvironment.CreateOutputDirectory("Fill", "LinearGradientBrush");
            using (var image = new Image<Rgba32>(width, height))
            {
                LinearGradientBrush<Rgba32> unicolorLinearGradientBrush = 
                    new LinearGradientBrush<Rgba32>(
                        new SixLabors.Primitives.Point(0, 0),
                        new SixLabors.Primitives.Point(0, 500),
                        new LinearGradientBrush<Rgba32>.ColorStop(0, Rgba32.Red),
                        new LinearGradientBrush<Rgba32>.ColorStop(1, Rgba32.Yellow));

                image.Mutate(x => x.Fill(unicolorLinearGradientBrush));
                image.Save($"{path}/verticalRedToYellow.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    Rgba32 columnColor23 = sourcePixels[0, 23];
                    Rgba32 columnColor42 = sourcePixels[0, 42];
                    Rgba32 columnColor333 = sourcePixels[0, 333];

                    for (int i = 0; i < width; i++)
                    {
                        // check first and last column, these are known:
                        Assert.Equal(Rgba32.Red, sourcePixels[i, 0]);
                        Assert.Equal(Rgba32.Yellow, sourcePixels[i, lastRowIndex]);

                        // check the random colors:
                        Assert.Equal(columnColor23, sourcePixels[i, 23]);
                        Assert.Equal(columnColor42, sourcePixels[i, 42]);
                        Assert.Equal(columnColor333, sourcePixels[i, 333]);
                    }
                }
            }
        }

        [Theory]
        [InlineData(0, 0, 499, 499)]
        [InlineData(0, 499, 499, 0)]
        [InlineData(499, 499, 0, 0)]
        [InlineData(499, 0, 0, 499)]
        public void DiagonalLinearGradientBrushReturnsUnicolorColumns(
            int startX, int startY, int endX, int endY)
        {
            int size = 500;
            int lastIndex = size - 1;

            string path = TestEnvironment.CreateOutputDirectory("Fill", "LinearGradientBrush");
            using (var image = new Image<Rgba32>(size, size))
            {
                LinearGradientBrush<Rgba32> unicolorLinearGradientBrush = 
                    new LinearGradientBrush<Rgba32>(
                        new SixLabors.Primitives.Point(startX, startY),
                        new SixLabors.Primitives.Point(endX, endY),
                        new LinearGradientBrush<Rgba32>.ColorStop(0, Rgba32.Red),
                        new LinearGradientBrush<Rgba32>.ColorStop(1, Rgba32.Yellow));

                image.Mutate(x => x.Fill(unicolorLinearGradientBrush));
                image.Save($"{path}/diagonalRedToYellowFrom{startX}_{startY}.png");

                using (PixelAccessor<Rgba32> sourcePixels = image.Lock())
                {
                    // check first and last pixel, these are known:
                    Assert.Equal(Rgba32.Red, sourcePixels[startX, startY]);
                    Assert.Equal(Rgba32.Yellow, sourcePixels[endX, endY]);

                    for (int i = 0; i < size; i++)
                    {
                        // it's diagonal, so for any (a, a) on the gradient line, for all (a-x, b+x) - +/- depending on the diagonal direction - must be the same color)
                    }
                }
            }
        }

        [Theory]
        [InlineData("a", 0, 0, 499, 499, new[] { 0f, .2f, .5f, .9f }, new[] { 0, 0, 1, 1 })]
        [InlineData("b", 0, 499, 499, 0, new[] { 0f, 0.2f, 0.5f, 0.9f }, new[] { 0, 1, 2, 3 })]
        [InlineData("c", 499, 499, 0, 0, new[] { 0f, 0.7f, 0.8f, 0.9f}, new[] { 0, 1, 2, 0 })]
        [InlineData("d", 0, 0, 499, 499, new[] { 0f, .5f, 1f}, new[]{0, 1, 3})]
        public void ArbitraryLinearGradientsProduceImagesVisualCheckOnly(
            string filenameSuffix,
            int startX, int startY,
            int endX, int endY,
            float[] stopPositions,
            int[] stopColorCodes)
        {
            var colors = new Rgba32[]
                             {
                                 Rgba32.Navy,
                                 Rgba32.LightGreen,
                                 Rgba32.Yellow,
                                 Rgba32.Red
                             };

            var colorStops = new LinearGradientBrush<Rgba32>.ColorStop[stopPositions.Length];
            for (int i = 0; i < stopPositions.Length; i++)
            {
                colorStops[i] = new LinearGradientBrush<Rgba32>.ColorStop(
                    stopPositions[i],
                    colors[stopColorCodes[i]]);
            }

            int size = 500;

            string path = TestEnvironment.CreateOutputDirectory("Fill", "LinearGradientBrush");
            using (var image = new Image<Rgba32>(size, size))
            {
                LinearGradientBrush<Rgba32> unicolorLinearGradientBrush = 
                    new LinearGradientBrush<Rgba32>(
                        new SixLabors.Primitives.Point(startX, startY),
                        new SixLabors.Primitives.Point(endX, endY),
                        colorStops);

                image.Mutate(x => x.Fill(unicolorLinearGradientBrush));
                image.Save($"{path}/arbitraryGradient_{filenameSuffix}.png");
            }
        }
    }
}