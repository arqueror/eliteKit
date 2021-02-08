using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AVFoundation;
using AVKit;
using eliteKit.eliteDependencies;
using eliteKit.iOSCore.eliteAudio;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(dependencyAudio))]
namespace eliteKit.iOSCore.eliteAudio
{
    public class dependencyAudio : IDependencyAudio
    {
        private AVAudioPlayer _audioPlayer = null;

        public Action OnFinishedPlaying { get; set; }
        public Action OnPrepared { get; set; }

        public dependencyAudio()
        {

        }

        public int getDuration()
        {
            if (_audioPlayer == null) return 0;
            return (int)this._audioPlayer.Duration;
        }

        public int getCurrentPosition()
        {
            if (_audioPlayer == null) return 0;
            return (int)this._audioPlayer.CurrentTime;
        }

        public void PrepareAudio(string pathToAudioFile)
        {
            try
            {
                if (_audioPlayer != null)
                {
                    _audioPlayer.FinishedPlaying -= Player_FinishedPlaying;
                    _audioPlayer.Stop();
                }

                string localUrl = pathToAudioFile;
                _audioPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(localUrl));
                _audioPlayer.FinishedPlaying += Player_FinishedPlaying;
                this.OnPrepared?.Invoke();
            }
            catch { }
        }

        public void Play()
        {
            _audioPlayer?.Play();
        }

        public void Pause()
        {
            _audioPlayer?.Pause();
        }

        private void Player_FinishedPlaying(object sender, AVStatusEventArgs e)
        {
            OnFinishedPlaying?.Invoke();
            if(_audioPlayer!=null)
                _audioPlayer.CurrentTime = 0;
        }
    }
}