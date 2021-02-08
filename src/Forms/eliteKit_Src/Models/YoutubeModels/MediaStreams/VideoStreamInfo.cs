﻿namespace eliteKit.Models.YoutubeModels.MediaStreams
{
    /// <summary>
    /// Metadata associated with a certain <see cref="MediaStream"/> that contains only video.
    /// </summary>
    internal class VideoStreamInfo : MediaStreamInfo
    {
        /// <summary>
        /// Bitrate (bits/s) of the associated stream.
        /// </summary>
        public long Bitrate { get; }

        /// <summary>
        /// Video encoding of the associated stream.
        /// </summary>
        public VideoEncoding VideoEncoding { get; }

        /// <summary>
        /// Video quality label of the associated stream.
        /// </summary>
        public string VideoQualityLabel { get; }

        /// <summary>
        /// Video quality of the associated stream.
        /// </summary>
        public VideoQuality VideoQuality { get; }

        /// <summary>
        /// Video resolution of the associated stream.
        /// </summary>
        public VideoResolution Resolution { get; }

        /// <summary>
        /// Video framerate (FPS) of the associated stream.
        /// </summary>
        public int Framerate { get; }

        /// <summary>
        /// Initializes an instance of <see cref="VideoStreamInfo"/>.
        /// </summary>
        public VideoStreamInfo(int itag, string url, Container container, long size, long bitrate,
            VideoEncoding videoEncoding, string videoQualityLabel, VideoQuality videoQuality,
            VideoResolution resolution, int framerate)
            : base(itag, url, container, size)
        {
            Bitrate = bitrate;
            VideoEncoding = videoEncoding;
            VideoQualityLabel = videoQualityLabel;
            VideoQuality = videoQuality;
            Resolution = resolution;
            Framerate = framerate;
        }

        /// <inheritdoc />
        public override string ToString() => $"{Itag} ({Container}) [video]";
    }
}