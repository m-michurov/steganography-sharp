using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Steganography.Core.Utility {
    public static class BitmapUtility {
        public static byte[] GetColorData(this Bitmap image) {
            var imageData = image.LockBits(
                new Rectangle(x: 0, y: 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                image.PixelFormat
            );
            var rawColorData = imageData.Scan0;
            var colorDataSize = Math.Abs(imageData.Stride) * image.Height;
            var colorData = new byte[colorDataSize];

            Marshal.Copy(rawColorData, colorData, startIndex: 0, colorDataSize);

            image.UnlockBits(imageData);

            return colorData;
        }

        public static void SetColorData(this Bitmap image, byte[] colorData) {
            var imageData = image.LockBits(
                new Rectangle(x: 0, y: 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                image.PixelFormat
            );
            var rawColorData = imageData.Scan0;

            Marshal.Copy(colorData, startIndex: 0, rawColorData, colorData.Length);

            image.UnlockBits(imageData);
        }
    }
}