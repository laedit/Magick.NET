﻿// Copyright Dirk Lemstra https://github.com/dlemstra/Magick.NET.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using ImageMagick.Defines;

namespace ImageMagick.Formats
{
    /// <summary>
    /// Class for defines that are used when a <see cref="MagickFormat.Jpeg"/> image is written.
    /// </summary>
    public sealed class JpegWriteDefines : WriteDefinesCreator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JpegWriteDefines"/> class.
        /// </summary>
        public JpegWriteDefines()
          : base(MagickFormat.Jpeg)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether arithmetic coding is enabled or disabled (jpeg:arithmetic-coding).
        /// </summary>
        public bool? ArithmeticCoding { get; set; }

        /// <summary>
        /// Gets or sets the dtc method that will be used (jpeg:dct-method).
        /// </summary>
        public JpegDctMethod? DctMethod { get; set; }

        /// <summary>
        /// Gets or sets the compression quality that does not exceed the specified extent in kilobytes (jpeg:extent).
        /// </summary>
        public int? Extent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether optimize coding is enabled or disabled (jpeg:optimize-coding).
        /// </summary>
        public bool? OptimizeCoding { get; set; }

        /// <summary>
        /// Gets or sets the file name that contains custom quantization tables (jpeg:q-table).
        /// </summary>
        public string? QuantizationTables { get; set; }

        /// <summary>
        /// Gets or sets jpeg sampling factor (jpeg:sampling-factor).
        /// </summary>
        public JpegSamplingFactor? SamplingFactor { get; set; }

        /// <summary>
        /// Gets the defines that should be set as a define on an image.
        /// </summary>
        public override IEnumerable<IDefine> Defines
        {
            get
            {
                if (ArithmeticCoding.HasValue)
                    yield return CreateDefine("arithmetic-coding", ArithmeticCoding.Value);

                if (DctMethod.HasValue)
                    yield return CreateDefine("dct-method", DctMethod.Value);

                if (Extent.HasValue)
                    yield return CreateDefine("extent", Extent.Value + "KB");

                if (OptimizeCoding.HasValue)
                    yield return CreateDefine("optimize-coding", OptimizeCoding.Value);

                if (QuantizationTables is not null && QuantizationTables.Length > 0)
                    yield return CreateDefine("q-table", QuantizationTables);

                if (SamplingFactor.HasValue)
                    yield return CreateDefine("sampling-factor", CreateSamplingFactors(SamplingFactor.Value));
            }
        }

        private static string CreateSamplingFactors(JpegSamplingFactor samplingFactor)
            => samplingFactor switch
            {
                JpegSamplingFactor.Ratio410 => "4x2,1x1,1x1",
                JpegSamplingFactor.Ratio411 => "4x1,1x1,1x1",
                JpegSamplingFactor.Ratio420 => "2x2,1x1,1x1",
                JpegSamplingFactor.Ratio422 => "2x1,1x1,1x1",
                JpegSamplingFactor.Ratio440 => "1x2,1x1,1x1",
                JpegSamplingFactor.Ratio444 => "1x1,1x1,1x1",
                _ => throw new InvalidOperationException(),
            };
    }
}
