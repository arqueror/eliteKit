﻿using eliteKit.Infrastructure.eliteVideo;
using System.Threading;
using System.Threading.Tasks;

namespace eliteKit.Interfaces
{
    internal interface IPlatformFeatures
    {
        #region Properties

        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        string PackageName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Hashes the specified value using SHA-1.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>
        /// A base-64 encoded SHA-1 hash.
        /// </returns>
        string HashSha1(string value);

        /// <summary>
        /// Exits the mobile application.
        /// </summary>
        void Exit();

        #endregion
    }


    /// <summary>
    /// Provides a common set of properties and operations for platform specific video player renderers.
    /// </summary>
    public interface IVideoPlayerRenderer
    {
        #region Methods

        /// <summary>
        /// Plays the video.
        /// </summary>
        void Play();

        /// <summary>
        /// Determines if the video player instance can play.
        /// </summary>
        ///   <c>true</c> if this instance can play; otherwise, <c>false</c>.
        bool CanPlay();

        /// <summary>
        /// Pauses the video.
        /// </summary>
        void Pause();

        /// <summary>
        /// Determines if the video player instance can pause.
        /// </summary>
        ///   <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        bool CanPause();

        /// <summary>
        /// Stops the video.
        /// </summary>
        void Stop();

        /// <summary>
        /// Determines if the video player instance can stop.
        /// </summary>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        bool CanStop();

        /// <summary>
        /// Seeks a specific number of seconds into the playback stream.
        /// </summary>
        /// <param name="seekTime">The seek time.</param>
        void Seek(int seekTime);

        /// <summary>
        /// Determines if the video player instance can seek.
        /// </summary>
        /// <param name="time">The time in seconds.</param>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        bool CanSeek(int time);

        #endregion
    }


    internal interface IVideoSourceHandler
    {
        /// <summary>
        /// Loads the video from the specified source.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The path to the video file.</returns>
        Task<string> LoadVideoAsync(VideoSource source, CancellationToken cancellationToken);
    }
}
