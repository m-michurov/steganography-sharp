using System;
using System.Collections.Generic;
using Steganography.Core.Exceptions;
using Steganography.Core.Utility;

namespace Steganography.Core.Encoding {
    public static class LeastSignificantBitEncoder {
        private const int BitsInByte = 8;
        private const int LeastSignificantBit = 0;

        private static void CheckContainerSize(
            int containerSize,
            int dataSize
        ) {
            if (containerSize >= dataSize * BitsInByte) {
                return;
            }

            throw new ContainerTooSmall(containerSize, dataSize);
        }

        public static void Encode(
            IList<byte> container,
            IReadOnlyList<byte> data
        ) {
            if (0 == data.Count) {
                return;
            }

            CheckContainerSize(container.Count, data.Count);

            var dataIndex = 0;
            var bitIndex = 0;

            for (
                var containerIndex = 0;
                containerIndex < container.Count && dataIndex < data.Count;
                containerIndex += 1
            ) {
                container[containerIndex] = (byte) BinaryUtility.SetBit(
                    container[containerIndex], 
                    LeastSignificantBit,
                    BinaryUtility.GetBit(data[dataIndex], BitsInByte - 1 - bitIndex)
                );

                bitIndex += 1;
                dataIndex += bitIndex / BitsInByte;
                bitIndex %= BitsInByte;
            }
        }

        public static void Decode(
            IReadOnlyList<byte> container,
            IList<byte> data
        ) {
            if (0 == data.Count) {
                return;
            }

            CheckContainerSize(container.Count, data.Count);

            var dataIndex = 0;
            var bitIndex = 0;

            for (
                var containerIndex = 0;
                containerIndex < container.Count && dataIndex < data.Count;
                containerIndex += 1
            ) {
                data[dataIndex] = (byte) BinaryUtility.SetBit(
                    data[dataIndex],
                    BitsInByte - 1 - bitIndex,
                    BinaryUtility.GetBit(container[containerIndex], LeastSignificantBit)
                );

                bitIndex += 1;
                dataIndex += bitIndex / BitsInByte;
                bitIndex %= BitsInByte;
            }
        }
    }
}