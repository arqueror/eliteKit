using System;
using System.Collections.Generic;
using System.Text;

namespace eliteKit.eliteDependencies
{
    public interface IDependencyAudio
    {
        void PrepareAudio(string pathToAudioFile);
        void Play();
        void Pause();
        int getCurrentPosition();
        int getDuration();
        Action OnFinishedPlaying { get; set; }
        Action OnPrepared { get; set; }
    }
}
