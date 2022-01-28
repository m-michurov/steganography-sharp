using System;

namespace Steganography.Core.Exceptions {
    public sealed class ContainerTooSmall : ArgumentException {
        public int ContainerSize { get; }
        public int RequiredSize { get; }

        public ContainerTooSmall(int containerSize, int requiredSize)
            : base($"Container is too small. Container size: {containerSize}b, required size: {requiredSize}b.") 
            => (ContainerSize, RequiredSize) = (containerSize, requiredSize);
    }
}