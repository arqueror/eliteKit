using System;

namespace eliteKit.Models
{
    internal readonly partial struct VideoResolution : IEquatable<VideoResolution>
    {
        /// <summary>
        /// Viewport width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Viewport height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes an instance of <see cref="VideoResolution"/>.
        /// </summary>
        public VideoResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is VideoResolution other && Equals(other);

        /// <inheritdoc />
        public bool Equals(VideoResolution other) => Width == other.Width && Height == other.Height;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }

        /// <inheritdoc />
        public override string ToString() => $"{Width}x{Height}";
    }

    internal partial struct VideoResolution
    {
        /// <summary />
        public static bool operator ==(VideoResolution r1, VideoResolution r2) => r1.Equals(r2);

        /// <summary />
        public static bool operator !=(VideoResolution r1, VideoResolution r2) => !(r1 == r2);
    }
}
