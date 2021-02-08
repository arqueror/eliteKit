using System;
using System.Collections.Generic;
using System.Text;

namespace eliteKit.eliteEnums
{
    public enum elitePlayerState
    {
        /// <summary>
        /// The idle state is the default state of a newly created video player.
        /// </summary>
        Idle,

        /// <summary>
        /// The video player enters this state when a video source has been specified for playback.
        /// </summary>
        Initialized,

        /// <summary>
        /// The video player is ready to begin playback.
        /// </summary>
        Prepared,

        /// <summary>
        /// Video playback is currently active.
        /// </summary>
        Playing,

        /// <summary>
        /// Video playback is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Video playback is complete.
        /// </summary>
        Completed,

        /// <summary>
        /// The video player has experienced an error.
        /// </summary>
        Error,
        /// <summary>
        /// The video player has started buffering.
        /// </summary>
        Buffering,
        /// <summary>
        /// The video player has finished buffering.
        /// </summary>
        FinishedBuffering,
        /// <summary>
        /// Player not able to play current Video.
        /// </summary>
        SourceFailedToLoad
    }
}