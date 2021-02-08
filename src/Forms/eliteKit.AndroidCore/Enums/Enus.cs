using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace eliteKit.AndroidCore.Enums
{
    /// <summary>
    /// Represents the states of an Android MediaPlayer.
    /// See: http://developer.android.com/images/mediaplayer_state_diagram.gif
    /// </summary>
    internal enum MediaPlayerStatus
    {
        Error,
        Idle,
        Preparing,
        Prepared,
        Playing,
        Paused,
        PlaybackCompleted
    }
}