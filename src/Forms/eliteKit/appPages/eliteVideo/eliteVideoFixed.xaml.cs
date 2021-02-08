using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using eliteKit.eliteEnums;
using eliteKit.Models;

namespace eliteKitDevelopment.appPages.eliteVideo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class eliteVideoFixed : ContentPage
    {
        public eliteVideoFixed()
        {
            InitializeComponent();
            this.videoPlayer.VideoCollection = new ObservableCollection<eliteVideoItem>()
            {
                new eliteVideoItem(){VideoOrder = 0,VideoProvider = eliteVideoProvider.Vimeo, VideoSource = "76311230"},
                new eliteVideoItem(){VideoOrder = 0,VideoProvider = eliteVideoProvider.Vimeo, VideoSource = "214352663"}
            };
        }
    }
}