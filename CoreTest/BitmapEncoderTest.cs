using System;
using System.Drawing;
using System.Drawing.Imaging;
using FluentAssertions;
using Steganography.Core.Encoding;
using Xunit;

namespace Steganography.CoreTest {
    public sealed class BitmapEncoderTest {
        [Theory]
        [InlineData(1, 1, PixelFormat.Format32bppArgb, 0)]
        [InlineData(1, 1, PixelFormat.Format16bppGrayScale, 0)]
        [InlineData(2, 3, PixelFormat.Format32bppArgb, 0)]
        [InlineData(4, 3, PixelFormat.Format16bppGrayScale, 0)]
        [InlineData(8, 8, PixelFormat.Format32bppArgb, 1000)]
        [InlineData(8, 8, PixelFormat.Format16bppGrayScale, 1000)]
        [InlineData(16, 16, PixelFormat.Format32bppArgb, 1000)]
        [InlineData(16, 16, PixelFormat.Format16bppGrayScale, 1000)]
        public void Encode_Invalid_containers_cannot_be_used(
            int width,
            int height,
            PixelFormat pixelFormat,
            int dataSize
        ) {
            // Arrange
            var action = (Action) (() => {
                using var image = new Bitmap(width, height, pixelFormat);
                _ = BitmapEncoder.Encode(image, new byte[dataSize]);
            });

            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Theory]
        [InlineData(1, 1, PixelFormat.Format32bppArgb)]
        [InlineData(1, 1, PixelFormat.Format16bppGrayScale)]
        [InlineData(2, 3, PixelFormat.Format32bppArgb)]
        [InlineData(4, 3, PixelFormat.Format16bppGrayScale)]
        public void Decode_Invalid_containers_cannot_be_used(
            int width,
            int height,
            PixelFormat pixelFormat
        ) {
            // Arrange
            var action = (Action) (() => {
                using var image = new Bitmap(width, height, pixelFormat);
                _ = BitmapEncoder.Decode(image);
            });

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(32, 32, new byte[] { })]
        [InlineData(32, 32, new byte[] {154})]
        [InlineData(32, 32, new byte[] {154, 71, 59})]
        [InlineData(2, 4, new byte[] { })]
        [InlineData(17, 32, new byte[] {33, 154, 71})]
        public void Data_can_be_encoded_and_decoded(
            int width,
            int height,
            byte[] data
        ) {
            // Arrange
            using var container = GenerateRandomImage(width, height);

            // Act
            var encoded = BitmapEncoder.Encode(container, data);
            var decoded = BitmapEncoder.Decode(encoded);

            // Assert
            encoded.Size.Should().Be(container.Size);
            decoded.Should().Equal(data);
        }

        private static Bitmap GenerateRandomImage(
            int width,
            int height
        ) {
            var image = new Bitmap(width, height);
            var r = new Random();
            var randomColor = (Func<Color>) (() => Color.FromArgb(
                (byte) r.Next(),
                (byte) r.Next(),
                (byte) r.Next(),
                (byte) r.Next()
            ));

            for (var y = 0; y < image.Height; y += 1) {
                for (var x = 0; x < image.Width; x += 1) {
                    image.SetPixel(x, y, randomColor());
                }
            }

            return image;
        }
    }
}