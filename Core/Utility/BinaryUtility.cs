using System;

namespace Steganography.Core.Utility {
    public static class BinaryUtility {
        public static bool GetBit(
            int value,
            int bitIndex
        ) {
            if (bitIndex is < 0 or >= sizeof(int) * 8) {
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            }
            
            return 0 != (value & (1 << bitIndex));
        }

        public static int SetBit(
            int value,
            int bitIndex,
            bool bit
        ) {
            if (bitIndex is < 0 or >= sizeof(int) * 8) {
                throw new ArgumentOutOfRangeException(nameof(bitIndex));
            }

            return bit
                ? value | (1 << bitIndex)
                : value & ~(1 << bitIndex);
        }
    }
}