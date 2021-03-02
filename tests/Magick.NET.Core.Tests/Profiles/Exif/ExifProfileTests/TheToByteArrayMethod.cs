﻿// Copyright 2013-2021 Dirk Lemstra <https://github.com/dlemstra/Magick.NET/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
//
//   https://imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using ImageMagick;
using Xunit;

namespace Magick.NET.Core.Tests
{
    public partial class ExifProfileTests
    {
        public class TheToByteArrayMethod
        {
            [Fact]
            public void ShouldReturnEmptyArrayWhenEmpty()
            {
                var profile = new ExifProfile();

                var bytes = profile.ToByteArray();
                Assert.Empty(bytes);
            }

            [Fact]
            public void ShouldReturnEmptyArrayWhenAllValuesAreInvalid()
            {
                var bytes = new byte[] { 69, 120, 105, 102, 0, 0, 73, 73, 42, 0, 8, 0, 0, 0, 1, 0, 42, 1, 4, 0, 1, 0, 0, 0, 42, 0, 0, 0, 26, 0, 0, 0, 0, 0 };

                var profile = new ExifProfile(bytes);

                var unkownTag = new ExifTag<uint>((ExifTagValue)298);
                var value = profile.GetValue<uint>(unkownTag);
                Assert.Equal(42U, value.GetValue());
                Assert.Equal("42", value.ToString());

                bytes = profile.ToByteArray();
                Assert.Empty(bytes);
            }

            [Fact]
            public void ShouldExcludeEmptyStrings()
            {
                var profile = new ExifProfile();
                profile.SetValue(ExifTag.ImageDescription, string.Empty);

                var data = profile.ToByteArray();

                var result = ExifReader.Read(data);

                Assert.Empty(result.Values);
            }
        }
    }
}
