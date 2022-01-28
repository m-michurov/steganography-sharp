using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Drawing;
using Steganography.Core.Exceptions;
using Steganography.Core.Utility;

namespace Steganography.Core.Encoding {
    public static class BitmapEncoder {
        private const int HeaderSize = sizeof(int) * 8;

        public static Bitmap Encode(
            Bitmap image,
            IReadOnlyList<byte> data
        ) {
            var result = new Bitmap(image.Width, image.Height, image.PixelFormat);
            var container = image.GetColorData();
            
            CheckContainerSize(container.Length, data.Count);
            
            WriteHeader(container, data.Count);
            WriteData(container, data);
            
            result.SetColorData(container);

            return result;
        }

        public static byte[] Decode(Bitmap container) {
            var colorData = container.GetColorData();
            
            CheckContainerSize(colorData.Length, dataSize: 0);
            
            var dataSize = ReadHeader(colorData);
            
            CheckContainerSize(colorData.Length, dataSize);
            
            return ReadData(colorData, dataSize);
        }

        private static void WriteHeader(
            byte[] container,
            int dataSize
        ) {
            var dataSizeBytes = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32BigEndian(dataSizeBytes, dataSize);

            LeastSignificantBitEncoder.Encode(
                new ArraySegment<byte>(container, offset: 0, HeaderSize),
                dataSizeBytes
            );
        }

        private static void WriteData(
            byte[] container,
            IReadOnlyList<byte> data
        ) => LeastSignificantBitEncoder.Encode(
            new ArraySegment<byte>(container, HeaderSize, container.Length - HeaderSize),
            data
        );

        private static int ReadHeader(byte[] container) {
            var dataSizeBytes = new byte[sizeof(int)];
            LeastSignificantBitEncoder.Decode(
                new ArraySegment<byte>(container, offset: 0, HeaderSize),
                dataSizeBytes
            );
            return BinaryPrimitives.ReadInt32BigEndian(dataSizeBytes);
        }

        private static byte[] ReadData(
            byte[] container,
            int dataSize
        ) {
            var data = new byte[dataSize];
            LeastSignificantBitEncoder.Decode(
                new ArraySegment<byte>(container, HeaderSize, container.Length - HeaderSize),
                data
            );
            return data;
        }

        private static void CheckContainerSize(
            int containerSize,
            int dataSize
        ) {
            var requiredSize = dataSize * 8 + HeaderSize;
            if (containerSize < requiredSize) {
                throw new ContainerTooSmall(containerSize, requiredSize);
            }
        }
    }
}