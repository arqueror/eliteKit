using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using eliteKit.AndroidCore.eliteAudio;
using eliteKit.eliteDependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(dependencyAudio))]
namespace eliteKit.AndroidCore.eliteAudio
{
    public class dependencyAudio : IDependencyAudio
    {
        private MediaPlayer _mediaPlayer;

        public Action OnFinishedPlaying { get; set; }
        public Action OnPrepared { get; set; }

        public dependencyAudio()
        {
        }

        public int getCurrentPosition()
        {
            return this._mediaPlayer.CurrentPosition;
        }

        public int getDuration()
        {
            return this._mediaPlayer.Duration;
        }

        public void PrepareAudio(string pathToSoundName)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Completion -= MediaPlayer_Completion;
                _mediaPlayer.Stop();
            }

            var fullPath = pathToSoundName;

            Android.Content.Res.AssetFileDescriptor afd = null;

            try
            {
                afd = Forms.Context.Assets.OpenFd(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error openfd: " + ex);
            }

            if (afd != null)
            {
                if (_mediaPlayer == null)
                {
                    _mediaPlayer = new MediaPlayer();
                    _mediaPlayer.Prepared += (sender, args) =>
                    {
                        _mediaPlayer.Completion += MediaPlayer_Completion;
                        this.OnPrepared?.Invoke();
                    };
                }

                _mediaPlayer.Reset();
                _mediaPlayer.SetVolume(1.0f, 1.0f);

                _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                _mediaPlayer.PrepareAsync();
            }
        }

        void MediaPlayer_Completion(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();
            _mediaPlayer.SeekTo(0);
        }

        public void Pause()
        {
            _mediaPlayer?.Pause();
        }

        public void Play()
        {
            _mediaPlayer?.Start();
        }
    }
}