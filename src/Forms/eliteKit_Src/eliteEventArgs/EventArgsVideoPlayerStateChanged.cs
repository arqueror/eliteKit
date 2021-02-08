using System;
using System.Collections.Generic;
using System.Text;

using eliteKit.eliteEnums;

namespace eliteKit.eliteEventArgs
{
    /// <summary>
    /// Contains information about video player state transitions.
    /// </summary>
    internal class EventArgsVideoPlayerStateChanged : EventArgsVideoPlayer
    {
        #region Properties

        /// <summary>
        /// The current state of the video player.
        /// </summary>
        /// <value>
        /// The state of the video player.
        /// </value>
        public elitePlayerState CurrentState { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        public EventArgsVideoPlayerStateChanged(elitePlayerState currentState)
        {
            CurrentState = currentState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rate">The rate of playback.</param>
        /// <param name="currentState">State of the current.</param>
        public EventArgsVideoPlayerStateChanged(TimeSpan currentTime, TimeSpan duration, float rate, elitePlayerState currentState)
            : base(currentTime, duration, rate)
        {
            CurrentState = currentState;
        }

        public EventArgsVideoPlayerStateChanged(EventArgsVideoPlayer videoPlayerEventArgs, elitePlayerState currentState)
            : this(videoPlayerEventArgs.CurrentTime, videoPlayerEventArgs.Duration, videoPlayerEventArgs.Rate, currentState)
        {
        }

        #endregion
    }
}
