namespace eliteKit.Models
{
    internal class AudioStreamInfo : MediaStreamInfo
    {
        /// <summary>
        /// Bitrate (bits/s) of the associated stream.
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        /// Audio encoding of the associated stream.
        /// </summary>
        public AudioEncoding AudioEncoding { get; }

        /// <summary>
        /// Initializes an instance of <see cref="AudioStreamInfo"/>.
        /// </summary>
        public AudioStreamInfo(int itag, string url, Container container, long size, long bitrate, AudioEncoding audioEncoding)
            : base(itag, url, container, size)
        {
            Bitrate = bitrate;
            AudioEncoding = audioEncoding;
        }

        /// <inheritdoc />
        public override string ToString() => $"{Itag} ({Container}) [audio]";
    }
}
