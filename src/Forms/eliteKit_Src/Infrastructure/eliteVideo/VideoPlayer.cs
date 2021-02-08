using eliteKit.Diagnostics;
using eliteKit.eliteEnums;
using eliteKit.eliteEventArgs;
using eliteKit.Interfaces;
using System;
using System.Windows.Input;
using eliteKit.Extensions;
using Xamarin.Forms;

namespace eliteKit.Infrastructure.eliteVideo
{
    /// <summary>
    /// A Xamarin Forms control that renders the native platform video player and reacts to touch events.
    /// </summary>
    internal class VideoPlayer : View
    {
        #region Fields & Properties

        /// <summary>
        /// The native video player renderer.
        /// </summary>
        internal IVideoPlayerRenderer NativeRenderer;

        #region State

        /// <summary>
        /// The is state property key.
        /// </summary>
        internal static readonly BindablePropertyKey StatePropertyKey =
            BindableProperty.CreateReadOnly(nameof(State), typeof(elitePlayerState), typeof(VideoPlayer), elitePlayerState.Idle);

        /// <summary>
        /// Identifies the State bindable property.
        /// </summary>
        /// 
        /// <remarks>
        /// This bindable property is readonly.
        /// </remarks>
        public static readonly BindableProperty StateProperty = StatePropertyKey.BindableProperty;

        /// <summary>
        /// The current state of the video player.
        /// </summary>
        /// <value>
        /// A <see cref="T:PlayerState"/> indicating the current state of the video player. The default is PlayerState.Idle.
        /// </value>
        public elitePlayerState State => (elitePlayerState)GetValue(StateProperty);

        #endregion

        #region CurrentTime

        /// <summary>
        /// The CurrentTime property key.
        /// </summary>
        internal static readonly BindablePropertyKey CurrentTimePropertyKey =
            BindableProperty.CreateReadOnly(nameof(CurrentTime), typeof(TimeSpan), typeof(VideoPlayer), TimeSpan.Zero);

        /// <summary>
        /// The VideoDuration property key.
        /// </summary>
        internal static readonly BindablePropertyKey VideoDurationPropertyKey =
            BindableProperty.CreateReadOnly(nameof(VideoDuration), typeof(TimeSpan), typeof(VideoPlayer),
                TimeSpan.Zero);


        /// <summary>
        /// Identifies the CurrentTime bindable property.
        /// </summary>
        /// 
        /// <remarks>
        /// This bindable property is readonly.
        /// </remarks>
        public static readonly BindableProperty CurrentTimeProperty = CurrentTimePropertyKey.BindableProperty;

        /// <summary>
        /// Identifies the VideoDuration bindable property.
        /// </summary>
        /// 
        /// <remarks>
        /// This bindable property is readonly.
        /// </remarks>
        public static readonly BindableProperty VideoDurationProperty = VideoDurationPropertyKey.BindableProperty;

        /// <summary>
        /// The current time of the video player.
        /// </summary>
        /// <value>
        /// A <see cref="T:TimeSpan"/> representing the current position in the playback timeline.
        /// </value>
        public TimeSpan CurrentTime => (TimeSpan)GetValue(CurrentTimeProperty);

        /// <summary>
        /// The current video duration.
        /// </summary>
        /// <value>
        /// A <see cref="T:TimeSpan"/> representing the current position in the playback timeline.
        /// </value>
        public TimeSpan VideoDuration => (TimeSpan)GetValue(VideoDurationProperty);

        #endregion

        #region IsLoading



        /// <summary>
        /// Identifies the IsLoading bindable property.
        /// </summary>
        /// 
        /// <remarks>
        /// This bindable property is readonly.
        /// </remarks>
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create("IsLoading", typeof(bool), typeof(VideoPlayer), false);

        /// <summary>
        /// The current loading status of the video. Useful for determining when buffering cccurs.
        /// </summary>
        /// 
        /// <value>
        /// A <see cref="T:System.Boolean"/> indicating if the video is loading. Default is false.
        /// </value>
        /// 
        /// <remarks>
        /// 
        /// <para>
        /// The following example illustrates running a  <see cref="T:Xamarin.Forms.ActivityIndicator"/> to indicate that the video is loading:
        /// </para>
        /// 
        /// <example>
        /// 
        /// <code lang="C#">
        /// <![CDATA[
        /// var videoPlayer = new VideoPlayer {
        ///   Source = new VideoLoader {
        ///     Uri = new Uri ("http://video-js.zencoder.com/oceans-clip.mp4")
        ///   },
        ///   HeightRequest = 300
        /// };
        /// 
        /// var indicator = new ActivityIndicator {Color = new Color (.5),};
        /// indicator.SetBinding (ActivityIndicator.IsRunningProperty, "IsLoading");
        /// indicator.BindingContext = videoPlayer;]]>
        /// </code>
        /// 
        /// </example>
        /// 
        /// </remarks>
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        #endregion

        #region AutoPlay

        /// <summary>
        /// The automatic play property
        /// </summary>
        public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(VideoPlayer), false);

        /// <summary>
        /// Specifies that the video will start playing as soon as it is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> plays the video automatically when it starts up; otherwise, <c>false</c>. Default is <c>false</c>.
        /// </value>
        public bool AutoPlay
        {
            get => (bool)GetValue(AutoPlayProperty);
            set => SetValue(AutoPlayProperty, value);
        }

        #endregion

        #region DisplayControls

        /// <summary>
        /// The display controls property.
        /// </summary>
        public static readonly BindableProperty DisplayControlsProperty = BindableProperty.Create(nameof(DisplayControls), typeof(bool), typeof(VideoPlayer), true);

        /// <summary>
        /// Specifies that video controls should be displayed (such as a play and pause button).
        /// </summary>
        /// <value>
        ///   <c>true</c> if video controls should be displayed; otherwise, <c>false</c>. Default is <c>true</c>.
        /// </value>
        public bool DisplayControls
        {
            get => (bool)GetValue(DisplayControlsProperty);
            set => SetValue(DisplayControlsProperty, value);
        }

        #endregion

        #region FillMode

        /// <summary>
        /// The FillMode bindable property.
        /// </summary>
        public static readonly BindableProperty FillModeProperty = BindableProperty.Create(nameof(FillMode), typeof(eliteFillMode), typeof(VideoPlayer), eliteFillMode.ResizeAspect);

        /// <summary>
        /// Defines how the video content is displayed within the player layer's bounds.
        /// </summary>
        /// <value>
        /// The video gravity. Default is ResizeAspect.
        /// </value>
        public eliteFillMode FillMode
        {
            get => (eliteFillMode)GetValue(FillModeProperty);
            set => SetValue(FillModeProperty, value);
        }

        #endregion

        //#region FullScreen

        ///// <summary>
        ///// The Fullscreen bindable property.
        ///// </summary>
        //public static readonly BindableProperty FullScreenProperty = BindableProperty.Create(nameof(FullScreen), typeof(bool), typeof(VideoPlayer), false);

        ///// <summary>
        ///// Determines whether the full screen should be used.
        ///// </summary>
        ///// <value>
        ///// The full screen.
        ///// </value>
        //public bool FullScreen
        //{
        //    get { return (bool)GetValue(FullScreenProperty); }
        //    set { SetValue(FullScreenProperty, value); }
        //}

        //#endregion

        #region TimeElapsedInterval

        /// <summary>
        /// The TimeElapsedIntervalProperty bindable property.
        /// </summary>
        public static readonly BindableProperty TimeElapsedIntervalProperty = BindableProperty.Create(nameof(TimeElapsedInterval), typeof(int), typeof(VideoPlayer), 0);

        /// <summary>
        /// The time interval in seconds for the TimeElapsed event firing to occur.
        /// Default is 0 seconds which means TimeElapsed will not fire.
        /// If you use TimeElapsed, it is recommended to set this value to fire less frequently for better peroformance.
        /// </summary>
        /// <value>
        /// The TimeElapsed event firing interval.
        /// </value>
        public int TimeElapsedInterval
        {
            get => (int)GetValue(TimeElapsedIntervalProperty);
            set
            {
                if (value >= 0)
                {
                    SetValue(TimeElapsedIntervalProperty, value);
                }
                else
                {
                    var message = $"TimeElapsedInterval of '{value}' must be greater than or equal to 0 seconds.";
                    Failed.RaiseEvent(new EventArgsVideoPlayerError(message));
                }
            }
        }

        #endregion

        #region Repeat

        /// <summary>
        /// The RepeatProperty bindable property.
        /// </summary>
        public static readonly BindableProperty RepeatProperty = BindableProperty.Create(nameof(Repeat), typeof(bool), typeof(VideoPlayer), false);

        /// <summary>
        /// Repeat video when playback is complete. Default is false.
        /// </summary>
        /// <value>
        /// The TimeElapsed event firing interval.
        /// </value>
        public bool Repeat
        {
            get => (bool)GetValue(RepeatProperty);
            set => SetValue(RepeatProperty, value);
        }

        #endregion

        #region Volume

        /// <summary>
        /// The Volume bindable property.
        /// </summary>
        public static readonly BindableProperty VolumeProperty = BindableProperty.Create(nameof(Volume), typeof(int), typeof(VideoPlayer), int.MinValue);

        /// <summary>
        /// The volume level of the current media stream.
        /// If this property is not set, it defaults to the current media volume setting of the mobile device.
		/// Valid values are 0 (muted) - 100 (max volume)
        /// </summary>
        /// <value>
        /// The volume level percentage.
        /// </value>
        public int Volume
        {
            get => (int)GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }

        #endregion

        #region Source

        /// <summary>
        /// The source bindable property.
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(VideoSource), typeof(VideoPlayer), null);

        /// <summary>
        /// A local file path or remote URL to a video file.
        /// </summary>
        /// <value>
        /// The path where this video file is located.
        /// </value>
        [TypeConverter(typeof(VideoSourceConverter))]
        public VideoSource Source
        {
            get => (VideoSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Notification the playback rate has changed to a non-zero rate.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayer> Playing;

        /// <summary>
        /// Raises the <see cref="E:Playing" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerEventArgs"/> instance containing the event data.</param>
        internal void OnPlaying(EventArgsVideoPlayer e)
        {
            Playing.RaiseEvent(this, e);
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.Playing));
        }

        /// <summary>
        /// Notification the playback rate has changed to zero.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayer> Paused;

        /// <summary>
        /// Raises the <see cref="E:Pause" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerEventArgs"/> instance containing the event data.</param>
        internal void OnPaused(EventArgsVideoPlayer e)
        {
            Paused.RaiseEvent(this, e);
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.Paused));
        }

        /// <summary>
        /// Notification the specified number of seconds configured in the `VideoPlayer.TimeElapsedInterval` have passed.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayer> TimeElapsed;

        /// <summary>
        /// Raises the <see cref="E:TimeElapsed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerEventArgs"/> instance containing the event data.</param>
        internal void OnTimeElapsed(EventArgsVideoPlayer e) { TimeElapsed.RaiseEvent(this, e); }

        /// <summary>
        /// Notification fires when the video player has reached the end of the playback stream.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayer> Completed;

        /// <summary>
        /// Raises the <see cref="E:Completed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerEventArgs"/> instance containing the event data.</param>
        internal void OnCompleted(EventArgsVideoPlayer e)
        {
            Completed.RaiseEvent(this, e);
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.Completed));
        }

        internal void OnBuffering(EventArgsVideoPlayer e)
        {
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.Buffering));
        }

        internal void OnFinishedBuffering(EventArgsVideoPlayer e)
        {
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.FinishedBuffering));
        }

        /// <summary>
        /// Notification fires when the video player experiences an error during playback or during initialization of the media file.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayerError> Failed;

        /// <summary>
        /// Raises the <see cref="E:Failed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerEventArgs"/> instance containing the event data.</param>
        internal void OnFailed(EventArgsVideoPlayerError e)
        {
            if (e.ErrorObject is Exception o)
                Log.Error(o, e.Message);
            else
                Log.Error(e.Message);

            Failed.RaiseEvent(this, e);
            OnPlayerStateChanged(new EventArgsVideoPlayerStateChanged(e, elitePlayerState.Error));
        }

        /// <summary>
        /// Event notification fires when the video player's internal state machine changes state.
        /// </summary>
        public event EventHandler<EventArgsVideoPlayerStateChanged> PlayerStateChanged;

        /// <summary>
        /// Raises the <see cref="E:PlayerStateChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="VideoPlayerStateChangedEventArgs"/> instance containing the event data.</param>
        internal void OnPlayerStateChanged(EventArgsVideoPlayerStateChanged e)
        {
            if (e?.CurrentState != null)
            {
                SetValue(StatePropertyKey, e.CurrentState);
                PlayerStateChanged.RaiseEvent(this, e);
            }
        }

        #endregion

        #region Commands

        /// <summary>
        /// The play command property.
        /// </summary>
        public static readonly BindableProperty PlayCommandProperty = BindableProperty.Create(nameof(PlayCommand), typeof(ICommand), typeof(VideoPlayer), null);

        /// <summary>
        /// Begins video playback.
        /// </summary>
        /// <value>
        /// The play command.
        /// </value>
        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            private set { SetValue(PlayCommandProperty, value); }
        }

        /// <summary>
        /// The pause command property.
        /// </summary>
        public static readonly BindableProperty PauseCommandProperty = BindableProperty.Create(nameof(PauseCommand), typeof(ICommand), typeof(VideoPlayer), null);

        /// <summary>
        /// Pauses video playback.
        /// </summary>
        /// <value>
        /// The pause command.
        /// </value>
        public ICommand PauseCommand
        {
            get { return (ICommand)GetValue(PauseCommandProperty); }
            private set { SetValue(PauseCommandProperty, value); }
        }

        /// <summary>
        /// The seek command property.
        /// </summary>
        public static readonly BindableProperty SeekCommandProperty = BindableProperty.Create(nameof(SeekCommand), typeof(ICommand), typeof(VideoPlayer), null);

        /// <summary>
        /// This command seeks a specific number of seconds forward or backward in the playback stream.
        /// For example, a command parameter of -3 seeks 3 seconds back.
        /// </summary>
        /// <value>
        /// The seek command.
        /// </value>
        public ICommand SeekCommand
        {
            get { return (ICommand)GetValue(SeekCommandProperty); }
            private set { SetValue(SeekCommandProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayer"/> class.
        /// </summary>
        public VideoPlayer()
        {
            Log.Info($"Creating video player instance [{Id}]");
            HeightRequest = 300;
            HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;

            PlayCommand = new Command(
                () => NativeRenderer?.Play(),
                () => NativeRenderer != null && NativeRenderer.CanPlay());

            PauseCommand = new Command(
                () => NativeRenderer?.Pause(),
                () => NativeRenderer != null && NativeRenderer.CanPause());

            SeekCommand = new Command<string>(
                (time) => NativeRenderer?.Seek(int.Parse(time)),
                (time) => NativeRenderer != null && NativeRenderer.CanSeek(int.Parse(time)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayer" /> class.
        /// </summary>
        /// <param name="source">The video source.</param>
        /// <param name="autoPlay">Specifies that the video will start playing as soon as it is ready.</param>
        public VideoPlayer(string source, bool autoPlay = false)
        {
            AutoPlay = autoPlay;
            Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayer"/> class.
        /// </summary>
        /// <param name="source">The video source.</param>
        /// <param name="autoPlay">Specifies that the video will start playing as soon as it is ready.</param>
        public VideoPlayer(VideoSource source, bool autoPlay = false)
        {
            AutoPlay = autoPlay;
            Source = source;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Begins video playback. 
        /// Only functions if the video player has entered a valid state (i.e. prepared).
        /// It may be best to hook into the <c>PlayerStateChanged</c> event to listen for the prepared or paused state before calling this method directly.
        /// </summary>
        public void Play()
        {
            PlayCommand.Execute(null);
        }

        /// <summary>
        /// Pauses video playback.
        /// Only functions if the video player has a non-zero playback rate.
        /// It may be best to hook into the <c>PlayerStateChanged</c> event to listen for the playing or prepared state before calling this method directly.
        /// </summary>
        public void Pause()
        {
            PauseCommand.Execute(null);
        }

        /// <summary>
        /// Seeks a specific number of seconds forward or backward in the playback stream.
        /// For example, a time of -3 seeks 3 seconds back.
        /// It may be best to hook into the <c>PlayerStateChanged</c> event to listen for the prepared state before calling this method directly.
        /// </summary>
        /// <param name="time">The time in seconds to seek forward or backward.</param>
        public void Seek(int time)
        {
            SeekCommand.Execute(time.ToString());
        }

        #endregion
    }
}
