﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eliteKit.eliteEventArgs
{
    /// <summary>
    /// Contains error information about the video player.
    /// </summary>
    internal class EventArgsVideoPlayerError : EventArgsVideoPlayer
    {
        #region Properties

        /// <summary>
        /// The reason the error occurred.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// The native exception object.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public object ErrorObject { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerErrorEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errorObject">The exception.</param>
        public EventArgsVideoPlayerError(string message, object errorObject = null)
        {
            Message = message;
            ErrorObject = errorObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rate">The rate of playback.</param>
        /// <param name="message">The message.</param>
        /// <param name="errorObject">The error object.</param>
        public EventArgsVideoPlayerError(TimeSpan currentTime, TimeSpan duration, float rate, string message, object errorObject)
            : base(currentTime, duration, rate)
        {
            Message = message;
            ErrorObject = errorObject;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        /// <param name="videoPlayerEventArgs">The <see cref="VideoPlayerEventArgs" /> instance containing the event data.</param>
        /// <param name="message">The message.</param>
        /// <param name="errorObject">The error object.</param>
        public EventArgsVideoPlayerError(EventArgsVideoPlayer videoPlayerEventArgs, string message, object errorObject)
            : this(videoPlayerEventArgs.CurrentTime, videoPlayerEventArgs.Duration, videoPlayerEventArgs.Rate, message, errorObject)
        {
        }

        #endregion
    }
}
