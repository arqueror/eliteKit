using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using eliteKit.Models;
using eliteKit.eliteEnums;

namespace eliteKitVersionCheck
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
       
            this.videoPlayer.VideoCollection = new ObservableCollection<eliteVideoItem>()
            {
                new eliteVideoItem(){VideoOrder = 0,VideoProvider = eliteVideoProvider.Vimeo, VideoSource = "76311230"},
                new eliteVideoItem(){VideoOrder = 0,VideoProvider = eliteVideoProvider.Vimeo, VideoSource = "214352663"}
            };

            this.audioPlayer.AudioCollection = new ObservableCollection<eliteKit.eliteElements.eliteAudioItem>()
            {
                new eliteKit.eliteElements.eliteAudioItem()
                {
                    AudioSource = "ForceCut.mp3",
                    AudioTitle = "Any title fagget"
                },
                new eliteKit.eliteElements.eliteAudioItem()
                {
                    AudioSource = "Whataride.mp3",
                    AudioTitle = "Other title fagget"
                }
            };
        }

    }
}
