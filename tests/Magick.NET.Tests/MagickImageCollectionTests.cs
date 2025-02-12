﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using System.IO;
using System.Linq;
using ImageMagick;
using Xunit;

#if Q8
using QuantumType = System.Byte;
#elif Q16
using QuantumType = System.UInt16;
#elif Q16HDRI
using QuantumType = System.Single;
#else
#error Not implemented!
#endif

namespace Magick.NET.Tests
{
    public partial class MagickImageCollectionTests
    {
        [Fact]
        public void Test_Deconstruct()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Deconstruct();
                });

                collection.Add(new MagickImage(MagickColors.Red, 20, 20));

                using (var frames = new MagickImageCollection())
                {
                    frames.Add(new MagickImage(MagickColors.Red, 10, 20));
                    frames.Add(new MagickImage(MagickColors.Purple, 10, 20));

                    collection.Add(frames.AppendHorizontally());
                }

                Assert.Equal(20, collection[1].Width);
                Assert.Equal(20, collection[1].Height);
                Assert.Equal(new MagickGeometry(0, 0, 10, 20), collection[1].Page);
                ColorAssert.Equal(MagickColors.Red, collection[1], 3, 3);

                collection.Deconstruct();

                Assert.Equal(10, collection[1].Width);
                Assert.Equal(20, collection[1].Height);
                Assert.Equal(new MagickGeometry(10, 0, 10, 20), collection[1].Page);
                ColorAssert.Equal(MagickColors.Purple, collection[1], 3, 3);
            }
        }

        [Fact]
        public void Test_Evaluate()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Evaluate(EvaluateOperator.Exponential);
                });

                collection.Add(new MagickImage(MagickColors.Yellow, 40, 10));

                using (var frames = new MagickImageCollection())
                {
                    frames.Add(new MagickImage(MagickColors.Green, 10, 10));
                    frames.Add(new MagickImage(MagickColors.White, 10, 10));
                    frames.Add(new MagickImage(MagickColors.Black, 10, 10));
                    frames.Add(new MagickImage(MagickColors.Yellow, 10, 10));

                    collection.Add(frames.AppendHorizontally());
                }

                using (var image = collection.Evaluate(EvaluateOperator.Min))
                {
                    ColorAssert.Equal(MagickColors.Green, image, 0, 0);
                    ColorAssert.Equal(MagickColors.Yellow, image, 10, 0);
                    ColorAssert.Equal(MagickColors.Black, image, 20, 0);
                    ColorAssert.Equal(MagickColors.Yellow, image, 30, 0);
                }
            }
        }

        [Fact]
        public void Test_Flatten()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Flatten();
                });

                collection.Add(new MagickImage(MagickColors.Brown, 10, 10));
                var center = new MagickImage(MagickColors.Fuchsia, 4, 4);
                center.Page = new MagickGeometry(3, 3, 4, 4);
                collection.Add(center);

                using (var image = collection.Flatten())
                {
                    ColorAssert.Equal(MagickColors.Brown, image, 0, 0);
                    ColorAssert.Equal(MagickColors.Fuchsia, image, 5, 5);
                }
            }
        }

        [Fact]
        public void Test_Index()
        {
            using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    collection[i].Resize(35, 23);
                    Assert.Equal(35, collection[i].Width);

                    collection[i] = collection[i];
                    Assert.Equal(35, collection[i].Width);
                }
            }
        }

        [Fact]
        public void Test_Merge()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Merge();
                });

                collection.Read(Files.RoseSparkleGIF);

                using (var first = collection.Merge())
                {
                    Assert.Equal(collection[0].Width, first.Width);
                    Assert.Equal(collection[0].Height, first.Height);
                }
            }
        }

        [Fact]
        public void Test_Montage()
        {
            using (var collection = new MagickImageCollection())
            {
                var settings = new MontageSettings();
                settings.Geometry = new MagickGeometry(string.Format("{0}x{1}", 200, 200));
                settings.TileGeometry = new MagickGeometry(string.Format("{0}x", 2));

                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Montage(settings);
                });

                for (int i = 0; i < 9; i++)
                    collection.Add(Files.Builtin.Logo);

                using (var montageResult = collection.Montage(settings))
                {
                    Assert.NotNull(montageResult);
                    Assert.Equal(400, montageResult.Width);
                    Assert.Equal(1000, montageResult.Height);
                }
            }
        }

        [Fact]
        public void Test_Morph()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Morph(10);
                });

                collection.Add(Files.Builtin.Logo);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Morph(10);
                });

                collection.AddRange(Files.Builtin.Wizard);

                collection.Morph(4);
                Assert.Equal(6, collection.Count);
            }
        }

        [Fact]
        public void Test_Mosaic()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.Mosaic();
                });

                collection.Add(Files.SnakewarePNG);
                collection.Add(Files.ImageMagickJPG);

                using (var mosaic = collection.Mosaic())
                {
                    Assert.Equal(286, mosaic.Width);
                    Assert.Equal(118, mosaic.Height);
                }
            }
        }

        [Fact]
        public void Test_Smush()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.SmushHorizontal(5);
                });

                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.SmushVertical(6);
                });

                collection.AddRange(Files.RoseSparkleGIF);

                using (var image = collection.SmushHorizontal(20))
                {
                    Assert.Equal((70 * 3) + (20 * 2), image.Width);
                    Assert.Equal(46, image.Height);
                }

                using (var image = collection.SmushVertical(40))
                {
                    Assert.Equal(70, image.Width);
                    Assert.Equal((46 * 3) + (40 * 2), image.Height);
                }
            }
        }

        [Fact]
        public void Test_ReadSettings()
        {
            var settings = new MagickReadSettings();
            settings.FontFamily = "Courier New";
            settings.FillColor = MagickColors.Gold;
            settings.FontPointsize = 80;
            settings.Format = MagickFormat.Text;
            settings.TextGravity = Gravity.Center;

            using (var images = new MagickImageCollection(Files.ImageMagickTXT, settings))
            {
                Assert.Equal(2, images.Count);
                ColorAssert.Equal(MagickColors.Gold, images[0], 348, 648);
            }

            using (var images = new MagickImageCollection())
            {
                images.Ping(Files.ImageMagickTXT, settings);

                Assert.Equal(2, images.Count);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    ColorAssert.Equal(MagickColors.Gold, images[0], 348, 648);
                });
            }
        }

        [Fact]
        public void Test_Remove()
        {
            using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
            {
                var first = collection[0];
                collection.Remove(first);

                Assert.Equal(2, collection.Count);
                Assert.Equal(-1, collection.IndexOf(first));

                first = collection[0];
                collection.RemoveAt(0);

                Assert.Single(collection);
                Assert.Equal(-1, collection.IndexOf(first));
            }
        }

        [Fact]
        public void Test_RePage()
        {
            using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
            {
                collection[0].Page = new MagickGeometry("0x0+10+20");

                Assert.Equal(10, collection[0].Page.X);
                Assert.Equal(20, collection[0].Page.Y);

                collection[0].Settings.Page = new MagickGeometry("0x0+10+20");

                Assert.Equal(10, collection[0].Settings.Page.X);
                Assert.Equal(20, collection[0].Settings.Page.Y);

                collection.RePage();

                Assert.Equal(0, collection[0].Page.X);
                Assert.Equal(0, collection[0].Page.Y);

                Assert.Equal(10, collection[0].Settings.Page.X);
                Assert.Equal(20, collection[0].Settings.Page.Y);
            }
        }

        [Fact]
        public void Test_Reverse()
        {
            using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
            {
                var first = collection.First();
                collection.Reverse();

                var last = collection.Last();
                Assert.True(last == first);
            }
        }

        [Fact]
        public void Test_ToBase64()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Equal(string.Empty, collection.ToBase64());

                collection.Read(Files.Builtin.Logo);
                Assert.Equal(1228800, collection.ToBase64(MagickFormat.Rgb).Length);
            }
        }

        [Fact]
        public void Test_TrimBounds()
        {
            using (var collection = new MagickImageCollection())
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    collection.TrimBounds();
                });

                collection.Add(Files.Builtin.Logo);
                collection.Add(Files.Builtin.Wizard);
                collection.TrimBounds();

                Assert.Equal(640, collection[0].Page.Width);
                Assert.Equal(640, collection[0].Page.Height);
                Assert.Equal(0, collection[0].Page.X);
                Assert.Equal(0, collection[0].Page.Y);

                Assert.Equal(640, collection[1].Page.Width);
                Assert.Equal(640, collection[1].Page.Height);
                Assert.Equal(0, collection[0].Page.X);
                Assert.Equal(0, collection[0].Page.Y);
            }
        }

        [Fact]
        public void Test_Write()
        {
            var fileSize = new FileInfo(Files.RoseSparkleGIF).Length;
            Assert.Equal(9891, fileSize);

            using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    collection.Write(memStream);

                    Assert.Equal(fileSize, memStream.Length);
                }
            }

            var tempFile = new FileInfo(Path.GetTempFileName() + ".gif");
            try
            {
                using (var collection = new MagickImageCollection(Files.RoseSparkleGIF))
                {
                    collection.Write(tempFile);

                    Assert.Equal(fileSize, tempFile.Length);
                }
            }
            finally
            {
                Cleanup.DeleteFile(tempFile);
            }
        }
    }
}
