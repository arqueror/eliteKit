using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Views;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using eliteKit.AndroidCore.eliteVideo;
using eliteKit.AndroidCore.Enums;
using eliteKit.AndroidCore.Renderers;

using eliteKit.Diagnostics;
using eliteKit.eliteEnums;
using eliteKit.eliteEventArgs;
using eliteKit.Infrastructure.eliteVideo;
using eliteKit.Interfaces;

using Math = System.Math;
using Timer = System.Timers.Timer;
using VideoView = eliteKit.AndroidCore.eliteVideo.VideoView;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]
[assembly: UsesPermission(Name = Manifest.Permission.Internet)]
[assembly: UsesPermission(Name = Manifest.Permission.WakeLock)]
namespace eliteKit.AndroidCore.Renderers
{
    /// <summary>
    /// A custom renderer for the native platform video player.
    /// </summary>
    internal class VideoPlayerRenderer : ViewRenderer<VideoPlayer, VideoView>, IVideoPlayerRenderer
    {
        #region Fields

        /// <summary>
        /// Tracks the playback time so we can update CurrentTime in the Xamarin Forms video player.
        /// </summary>
        private Timer _currentPositionTimer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerRenderer"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VideoPlayerRenderer(Context context) : base(context)
        {

        }

        #endregion

        #region IVideoPlayerRenderer

        /// <summary>
        /// Plays the video.
        /// </summary>
        public virtual void Play()
        {
            if (CanPlay())
                Control?.Start();
        }

        /// <summary>
        /// Determines if the video player instance can play.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can play; otherwise, <c>false</c>.
        public virtual bool CanPlay()
        {
            var control = Control;
            return control != null && (control.Status == MediaPlayerStatus.Prepared
                || control.Status == MediaPlayerStatus.Paused);
        }

        /// <summary>
        /// Pauses the video.
        /// </summary>
        public virtual void Pause()
        {
            if (CanPause())
                Control?.Pause();
        }

        /// <summary>
        /// Determines if the video player instance can pause.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        public virtual bool CanPause()
        {
            var control = Control;
            return control != null && control.CanPause();
        }

        /// <summary>
        /// Stops the video.
        /// </summary>
        public virtual void Stop()
        {
            if (CanStop())
                Control?.StopPlayback();
        }

        /// <summary>
        /// Determines if the video player instance can stop.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        public virtual bool CanStop()
        {
            var control = Control;
            return control != null
                && (control.IsPlaying
                   || control.Status == MediaPlayerStatus.Playing
                   || control.Status == MediaPlayerStatus.Prepared
                   || control.Status == MediaPlayerStatus.PlaybackCompleted);
        }

        /// <summary>
        /// Seeks a specific number of seconds into the playback stream.
        /// </summary>
        /// <param name="time">The time in seconds.</param>
        public virtual void Seek(int time)
        {
            if (CanSeek(time))
            {
                var control = Control;

                if (control != null)
                {
                    var currentTime = control.CurrentPosition;
                    var targetTime = currentTime + TimeSpan.FromSeconds(time).TotalMilliseconds;
                    Log.Info($"SEEK: CurrentTime={currentTime}; NewTime={targetTime}");
                    control.SeekTo((int)targetTime);
                    Element.SetValue(VideoPlayer.CurrentTimePropertyKey,
                        TimeSpan.FromMilliseconds(control.CurrentPosition));
                
                }
            }
        }

        /// <summary>
        /// Determines if the video player instance can seek.
        /// </summary>
        /// <param name="time">The time in seconds.</param>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        public virtual bool CanSeek(int time)
        {
            var control = Control;
            return control != null && ((time > 0 && Control.CanSeekForward() && (Control.CurrentPosition + time) <= Control.Duration)
                   || (time < 0 && Control.CanSeekBackward() && (Control.CurrentPosition + time) >= 0));
        }

        #endregion

        #region ViewRenderer Overrides

        /// <summary>
        /// Raises the <see cref="E:ElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ElementChangedEventArgs{VideoPlayer}"/> instance containing the event data.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new VideoView(Context)
                {
                    LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent)
                });
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                Element.NativeRenderer = null;
            }

            if (e.NewElement != null)
            {
                // Subscribe
                UpdateVisibility();
                RegisterEvents();
                await UpdateSource(e.OldElement);
                Element.NativeRenderer = this;
            }
        }

        /// <summary>
        /// Called when [element property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null || Control == null) return;

            if (e.PropertyName == VideoPlayer.DisplayControlsProperty.PropertyName)
            {
                UpdateDisplayControls();
            }
            else if (e.PropertyName == VideoPlayer.FillModeProperty.PropertyName)
            {
                UpdateFillMode();
            }
            else if (e.PropertyName == VideoPlayer.TimeElapsedIntervalProperty.PropertyName)
            {
                UpdateTimeElapsedInterval();
            }
            else if (e.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                await UpdateSource();
            }
            else if (e.PropertyName == VideoPlayer.VolumeProperty.PropertyName)
            {
                UpdateVolume();
            }
            else if (e.PropertyName == VideoPlayer.RepeatProperty.PropertyName)
            {
                UpdateRepeat();
            }
            else if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
            {
                UpdateVisibility();
            }
            else if (e.PropertyName == "IsLoading")
            {
                if(Element.IsLoading)
                    Element.OnBuffering(CreateVideoPlayerEventArgs());
                else
                    Element.OnFinishedBuffering(CreateVideoPlayerEventArgs());
            }
        }

        #endregion

        #region  Events

        /// <summary>
        /// Registers this renderer with native events.
        /// </summary>
        private void RegisterEvents()
        {
            // CurrentTime Setup
            _currentPositionTimer = new Timer(1000);
            _currentPositionTimer.Elapsed += (source, eventArgs) =>
            {
                if (_currentPositionTimer.Enabled && Control.Player != null)
                    Element.SetValue(VideoPlayer.CurrentTimePropertyKey, TimeSpan.FromMilliseconds(Control.CurrentPosition));
            };

            // Prepared
            Control.Prepared += OnPrepared;

            // Time Elapsed
            Control.TimeElapsed += (sender, args) => Element.OnTimeElapsed(CreateVideoPlayerEventArgs());

            // Completion
            Control.Completion += (sender, args) =>
            {
                _currentPositionTimer.Stop();
                Element.OnCompleted(CreateVideoPlayerEventArgs());
            };

            // Error
            Control.Error += (sender, args) =>
            {
                _currentPositionTimer.Stop();
                Element.OnFailed(CreateVideoPlayerErrorEventArgs(args.What));
            };

            // Play
            Control.Started += (sender, args) =>
            {
                _currentPositionTimer.Start();
                Element.OnPlaying(CreateVideoPlayerEventArgs());
            };

            // Paused
            Control.Paused += (sender, args) =>
            {
                _currentPositionTimer.Stop();
                Element.OnPaused(CreateVideoPlayerEventArgs());

            };

            // Stopped
            Control.Stopped += (sender, args) =>
            {
                _currentPositionTimer.Stop();
                Element.OnPaused(CreateVideoPlayerEventArgs());
            };
        }

        /// <summary>
        /// Called when the video player has been prepared for playback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnPrepared(object sender, EventArgs eventArgs)
        {
            Element.SetValue(VideoPlayer.IsLoadingProperty, false);
            Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(elitePlayerState.Prepared));
            Element.SetValue(VideoPlayer.VideoDurationPropertyKey, TimeSpan.FromMilliseconds(Control.Duration));

            // Player Setup
            UpdateDisplayControls();
            UpdateVolume();
            UpdateFillMode();
            UpdateTimeElapsedInterval();
            UpdateRepeat();

            if (Element.AutoPlay)
                Play();

            Control.Player.Info += (s, a) =>
            {
                switch (a.What)
                {
                    case Android.Media.MediaInfo.BufferingStart:
                        //AppLog.showLogE(TAG,"Buffering...");
                        Element.IsLoading = true;
                        break;
                    case Android.Media.MediaInfo.BufferingEnd:
                        //AppLog.showLogE(TAG,"Buffering End");
                        Element.IsLoading = false;
                        break;
                }
            };

        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the display controls property on the native player.
        /// </summary>
        private void UpdateDisplayControls()
        {
            Control.MediaController.Visibility = Element.DisplayControls ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <summary>
        /// Updates the volume level.
        /// </summary>
        private void UpdateVolume()
        {
            var volume = Element.Volume;

            if (volume != int.MinValue)
            {
                var nativeVolume = (float)Math.Min(100, Math.Max(0, volume)) / 100;
                Control.Player?.SetVolume(nativeVolume, nativeVolume);
            }
        }

        /// <summary>
        /// Updates the video source property on the native player.
        /// </summary>
		/// <param name="oldElement">The old element.</param>
		private async Task UpdateSource(VideoPlayer oldElement = null)
        {
            try
            {
                var newSource = Element?.Source;

                if (oldElement != null)
                {
                    var oldSource = oldElement.Source;

                    if (!oldSource.Equals(newSource))
                        return;
                }

                Element.SetValue(VideoPlayer.IsLoadingProperty, true);
                var videoSourceHandler = VideoSourceHandler.Create(newSource);
                var path = await videoSourceHandler.LoadVideoAsync(newSource, new CancellationToken());
                Log.Info($"Video Source: {path}");

                if (!string.IsNullOrEmpty(path))
                {
                    Element.SetValue(VideoPlayer.CurrentTimePropertyKey, TimeSpan.Zero);
                    Control.SetVideoPath(path);
                    Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(elitePlayerState.Initialized));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Element.SetValue(VideoPlayer.IsLoadingProperty, false);
            }
        }

        /// <summary>
        /// Updates the time elapsed interval of the video player.
        /// </summary>
        private void UpdateTimeElapsedInterval()
        {
            Control.TimeElapsedInterval = Element.TimeElapsedInterval;
        }

        /// <summary>
        /// Updates the fill mode property on the native player.
        /// </summary>
        private void UpdateFillMode()
        {
            Control.FillMode = Element.FillMode;
        }

        /// <summary>
        /// Updates the repeat property on the native player.
        /// </summary>
        private void UpdateRepeat()
        {
            Control.Player.Looping = Element.Repeat;
        }

        /// <summary>
        /// Updates the visibility of the video player.
        /// </summary>
        private void UpdateVisibility()
        {
            Control.Visibility = Element.IsVisible ? ViewStates.Visible : ViewStates.Gone;
        }

        /// <summary>
        /// Creates the video player event arguments.
        /// </summary>
        /// <returns>VideoPlayerEventArgs with information populated.</returns>
        private EventArgsVideoPlayer CreateVideoPlayerEventArgs()
        {
            var mediaPlayer = Control;

            if (mediaPlayer != null)
            {
                //var audioManager = Context.GetSystemService("audio") as AudioManager;
                //audioManager.GetStreamVolume(Stream.Music)

                return new EventArgsVideoPlayer(
                    TimeSpan.FromMilliseconds(Control.CurrentPosition),
                    TimeSpan.FromMilliseconds(Control.Duration),
                    1
                );
            }

            return null;
        }

        /// <summary>
        /// Creates the video player state changed event arguments.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns></returns>
        private EventArgsVideoPlayerStateChanged CreateVideoPlayerStateChangedEventArgs(elitePlayerState state)
        {
            var videoPlayerEventArgs = CreateVideoPlayerEventArgs();

            return videoPlayerEventArgs == null
                ? new EventArgsVideoPlayerStateChanged(state)
                : new EventArgsVideoPlayerStateChanged(videoPlayerEventArgs, state);
        }

        /// <summary>
        /// Creates the video player error event arguments.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="extra">The extra.</param>
        /// <returns>
        /// VideoPlayerErrorEventArgs with information populated.
        /// </returns>
        private EventArgsVideoPlayerError CreateVideoPlayerErrorEventArgs(MediaError errorCode)
        {
            var videoPlayerEventArgs = CreateVideoPlayerEventArgs();

            return videoPlayerEventArgs == null
                ? new EventArgsVideoPlayerError(errorCode.ToString())
                : new EventArgsVideoPlayerError(videoPlayerEventArgs, errorCode.ToString(), null);
        }

        #endregion
    }
}