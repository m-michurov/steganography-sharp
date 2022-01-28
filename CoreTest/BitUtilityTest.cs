using System;
using FluentAssertions;
using Steganography.Core;
using Steganography.Core.Utility;
using Xunit;

namespace Steganography.CoreTest {
    public sealed class BitUtilityTest {
        [Theory]
        [InlineData(0b_0101, 0, true)]
        [InlineData(0b_0101, 1, false)]
        [InlineData(~0, 31, true)]
        public void GetBit_returns_correct_bit_value(int value, int bitIndex, bool expected) {
            // Act
            var bit = BinaryUtility.GetBit(value, bitIndex);
            
            // Assert
            bit.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(32)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void GetBit_validates_bit_index(int bitIndex) {
            // Arrange
            var action = (Action) (() => BinaryUtility.GetBit(value: 0, bitIndex));
            
            // Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(0, 0, true, 1)]
        [InlineData(0, 31, true, 1 << 31)]
        [InlineData(~0, 0, false, ~0 & ~1)]
        [InlineData(~0, 31, false, ~0 & ~(1 << 31))]
        [InlineData((byte) 0, 0, true, (byte) 1)]
        [InlineData((byte) 0, 7, true, (byte) 0b_1000_0000)]
        [InlineData((byte) 255, 0, false, (byte) 254)]
        [InlineData((byte) 255, 7, false, (byte) 127)]
        public void SetBit_sets_correct_bit_values(int value, int bitIndex, bool bit, int expected) {
            // Act
            var result = BinaryUtility.SetBit(value, bitIndex, bit);
            
            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(32)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SetBit_validates_bit_index(int bitIndex) {
            // Arrange
            var action = (Action) (() => BinaryUtility.SetBit(value: 0, bitIndex, bit: false));
            
            // Assert 
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}