﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.IO;
using ImageMagick;
using Xunit;
using Xunit.Sdk;
using OperatingSystem = ImageMagick.OperatingSystem;

namespace Magick.NET.Tests
{
    public partial class MagickNETTests
    {
        public partial class TheGetFormatInformationMethod
        {
            public class WithByteArray
            {
                [Fact]
                public void ShouldThrowExceptionWhenArrayIsNull()
                {
                    var exception = Assert.Throws<ArgumentNullException>(() => MagickNET.GetFormatInformation((byte[])null));

                    Assert.Equal("data", exception.ParamName);
                }

                [Fact]
                public void ShouldThrowExceptionWhenArrayIsEmpty()
                {
                    var exception = Assert.Throws<ArgumentException>(() => MagickNET.GetFormatInformation(Array.Empty<byte>()));

                    Assert.Equal("data", exception.ParamName);
                }

                [Fact]
                public void ShouldReturnNullWhenFormatCannotBeDetermined()
                {
                    var formatInfo = MagickNET.GetFormatInformation(new byte[] { 42 });

                    Assert.Null(formatInfo);
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForTheJpgFormat()
                {
                    var bytes = File.ReadAllBytes(Files.ImageMagickJPG);
                    var formatInfo = MagickNET.GetFormatInformation(bytes);

                    Assert.NotNull(formatInfo);
                    Assert.Equal(MagickFormat.Jpeg, formatInfo.Format);
                }
            }

            public class WithFileInfo
            {
                [Fact]
                public void ShouldThrowExceptionWhenFileInfoIsNull()
                {
                    var exception = Assert.Throws<ArgumentNullException>(() => MagickNET.GetFormatInformation((FileInfo)null));

                    Assert.Equal("file", exception.ParamName);
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForThePngFormat()
                {
                    var fileInfo = new FileInfo(Files.MagickNETIconPNG);
                    var formatInfo = MagickNET.GetFormatInformation(fileInfo);

                    Assert.NotNull(formatInfo);
                    Assert.True(formatInfo.CanReadMultithreaded);
                    Assert.True(formatInfo.CanWriteMultithreaded);
                    Assert.Equal("Portable Network Graphics", formatInfo.Description);
                    Assert.Equal(MagickFormat.Png, formatInfo.Format);
                    Assert.False(formatInfo.IsMultiFrame);
                    Assert.True(formatInfo.IsReadable);
                    Assert.True(formatInfo.IsWritable);
                    Assert.Equal("image/png", formatInfo.MimeType);
                    Assert.Equal(MagickFormat.Png, formatInfo.ModuleFormat);
                }
            }

            public class WithFilename
            {
                [Fact]
                public void ShouldThrowExceptionWhenFileMameIsNull()
                {
                    var exception = Assert.Throws<ArgumentNullException>(() => MagickNET.GetFormatInformation((string)null));

                    Assert.Equal("fileName", exception.ParamName);
                }

                [Fact]
                public void ShouldThrowExceptionWhenFilenameIsEmpty()
                {
                    var exception = Assert.Throws<ArgumentException>(() => MagickNET.GetFormatInformation(string.Empty));

                    Assert.Equal("fileName", exception.ParamName);
                }

                [Fact]
                public void ShouldReturnNullForUnknownExtension()
                {
                    using (var temporaryFile = new TemporaryFile("foo.bar"))
                    {
                        var formatInfo = MagickNET.GetFormatInformation(temporaryFile.FullName);

                        Assert.Null(formatInfo);
                    }
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForTheJpgFormat()
                {
                    var formatInfo = MagickNET.GetFormatInformation(Files.ImageMagickJPG);

                    Assert.NotNull(formatInfo);
                    Assert.True(formatInfo.CanReadMultithreaded);
                    Assert.True(formatInfo.CanWriteMultithreaded);
                    Assert.Equal("Joint Photographic Experts Group JFIF format", formatInfo.Description);
                    Assert.Equal(MagickFormat.Jpg, formatInfo.Format);
                    Assert.False(formatInfo.IsMultiFrame);
                    Assert.True(formatInfo.IsReadable);
                    Assert.True(formatInfo.IsWritable);
                    Assert.Equal("image/jpeg", formatInfo.MimeType);
                    Assert.Equal(MagickFormat.Jpeg, formatInfo.ModuleFormat);
                }
            }

            public class WithMagickFormat
            {
                [Fact]
                public void ShouldReturnNullForUnknownFormat()
                {
                    var formatInfo = MagickNET.GetFormatInformation((MagickFormat)12345);

                    Assert.Null(formatInfo);
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForTheGradientFormat()
                {
                    var formatInfo = MagickNET.GetFormatInformation(MagickFormat.Gradient);

                    Assert.NotNull(formatInfo);
                    Assert.Equal(MagickFormat.Gradient, formatInfo.Format);
                    Assert.True(formatInfo.CanReadMultithreaded);
                    Assert.True(formatInfo.CanWriteMultithreaded);
                    Assert.Equal("Gradual linear passing from one shade to another", formatInfo.Description);
                    Assert.False(formatInfo.IsMultiFrame);
                    Assert.True(formatInfo.IsReadable);
                    Assert.False(formatInfo.IsWritable);
                    Assert.Null(formatInfo.MimeType);
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForTheJp2Format()
                {
                    var formatInfo = MagickNET.GetFormatInformation(MagickFormat.Jp2);

                    Assert.NotNull(formatInfo);
                    Assert.Equal(MagickFormat.Jp2, formatInfo.Format);
                    Assert.True(formatInfo.CanReadMultithreaded);
                    Assert.True(formatInfo.CanWriteMultithreaded);
                    Assert.Equal("JPEG-2000 File Format Syntax", formatInfo.Description);
                    Assert.False(formatInfo.IsMultiFrame);
                    Assert.True(formatInfo.IsReadable);
                    Assert.True(formatInfo.IsWritable);
                    Assert.Equal("image/jp2", formatInfo.MimeType);
                }

                [Fact]
                public void ShouldReturnTheCorrectInfoForThePangoFormat()
                {
                    var formatInfo = MagickNET.GetFormatInformation(MagickFormat.Pango);

                    Assert.NotNull(formatInfo);
                    Assert.False(formatInfo.CanReadMultithreaded);
                    Assert.False(formatInfo.CanWriteMultithreaded);
                    Assert.Equal("Pango Markup Language", formatInfo.Description);
                    Assert.Equal(MagickFormat.Pango, formatInfo.Format);
                    Assert.False(formatInfo.IsMultiFrame);
                    Assert.True(formatInfo.IsReadable);
                    Assert.False(formatInfo.IsWritable);
                    Assert.Null(formatInfo.MimeType);
                    Assert.Equal(MagickFormat.Pango, formatInfo.ModuleFormat);
                }

                [Fact]
                public void ShouldReturnFormatInfoForAllFormats()
                {
                    var missingFormats = new List<string>();

                    foreach (MagickFormat format in Enum.GetValues(typeof(MagickFormat)))
                    {
                        if (format == MagickFormat.Unknown)
                            continue;

                        var formatInfo = MagickNET.GetFormatInformation(format);
                        if (formatInfo is null)
                        {
                            if (ShouldReport(format))
                                missingFormats.Add(format.ToString());
                        }
                    }

                    if (missingFormats.Count > 0)
                        throw new XunitException("Cannot find MagickFormatInfo for: " + string.Join(", ", missingFormats.ToArray()));
                }

                private static bool ShouldReport(MagickFormat format)
                {
                    if (IsDisabledThroughPolicy(format))
                        return false;

                    if (!OperatingSystem.IsWindows)
                    {
                        if (format == MagickFormat.Clipboard || format == MagickFormat.Emf || format == MagickFormat.Wmf)
                            return false;
                    }

                    if (OperatingSystem.IsMacOS)
                    {
                        if (format == MagickFormat.Jxl)
                            return false;
                    }

                    return true;
                }

                /// <summary>
                /// Disabled with <see cref="TestCollectionOrderer.ModifyPolicy(string)"/>.
                /// </summary>
                private static bool IsDisabledThroughPolicy(MagickFormat format)
                    => format == MagickFormat.Sun || format == MagickFormat.Ras;
            }
        }
    }
}
