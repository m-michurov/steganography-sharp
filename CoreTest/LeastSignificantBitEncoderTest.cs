using System;
using FluentAssertions;
using Steganography.Core;
using Steganography.Core.Encoding;
using Xunit;

namespace Steganography.CoreTest {
    public sealed class LeastSignificantBitEncoderTest {
        [Fact]
        private void Encode_Container_must_have_sufficient_size() {
            // Arrange
            const int containerSize = 10;
            const int dataSize = containerSize / 8 + 1;

            var container = new byte[containerSize];
            var data = new byte[dataSize];

            var action = (Action) (() => LeastSignificantBitEncoder.Encode(container, data));

            // Assert
            action.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        private void Decode_Container_must_have_sufficient_size() {
            // Arrange
            const int containerSize = 10;
            const int dataSize = containerSize / 8 + 1;

            var container = new byte[containerSize];
            var data = new byte[dataSize];

            var action = (Action) (() => LeastSignificantBitEncoder.Decode(container, data));

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(8, 1)]
        [InlineData(64, 1)]
        [InlineData(64, 8)]
        private void Encode_Edge_case_container_sizes_are_handled(
            int containerSize,
            int dataSize
        ) {
            // Arrange
            var container = new byte[containerSize];
            var data = new byte[dataSize];

            var action = (Action) (() => LeastSignificantBitEncoder.Encode(container, data));

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(8, 1)]
        [InlineData(64, 1)]
        [InlineData(64, 8)]
        private void Decode_Edge_case_container_sizes_are_handled(
            int containerSize,
            int dataSize
        ) {
            // Arrange
            var container = new byte[containerSize];
            var data = new byte[dataSize];

            var action = (Action) (() => LeastSignificantBitEncoder.Decode(container, data));

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(
            new byte[] {255, 255, 255, 255, 255, 255, 255, 255},
            new byte[] {0},
            new byte[] {254, 254, 254, 254, 254, 254, 254, 254}
        )]
        [InlineData(
            new byte[] {0, 0, 0, 0, 254, 254, 254, 254},
            new byte[] {0b_0011_1010},
            new byte[] {0, 0, 1, 1, 255, 254, 255, 254}
        )]
        [InlineData(
            new byte[] {0, 0, 0, 0, 254, 254, 254, 254, 1, 1, 1, 1},
            new byte[] {0b_0011_1010},
            new byte[] {0, 0, 1, 1, 255, 254, 255, 254, 1, 1, 1, 1}
        )]
        private void Data_is_encoded_correctly(
            byte[] container,
            byte[] data,
            byte[] expected
        ) {
            // Act
            LeastSignificantBitEncoder.Encode(container, data);
            
            // Assert
            container.Should().Equal(expected);
        }

        [Theory]
        [InlineData(
            new byte[] {254, 254, 254, 254, 254, 254, 254, 254},
            new byte[] {0}
        )]
        [InlineData(
            new byte[] {0, 0, 1, 1, 255, 254, 255, 254},
            new byte[] {0b_0011_1010}
        )]
        [InlineData(
            new byte[] {0, 0, 1, 1, 255, 254, 255, 254, 1, 1, 1, 1},
            new byte[] {0b_0011_1010}
        )]
        private void Data_is_decoded_correctly(
            byte[] container,
            byte[] expected
        ) {
            // Arrange
            var data = new byte[expected.Length];
            
            // Act
            LeastSignificantBitEncoder.Decode(container, data);
            
            // Assert
            data.Should().Equal(expected);
        }
    }
}