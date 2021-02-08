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
    public partial class eliteVideoPopup : ContentPage
    {
        public eliteVideoPopup()
        {
            InitializeComponent();

            //When player finds an invalid video. It will try to move to next. If that is invalid as well, will play last one that worked
            this.videoPlayer.VideoCollection = new ObservableCollection<eliteVideoItem>()
            {
                new eliteVideoItem(){VideoOrder = 0,VideoProvider = eliteVideoProvider.Vimeo, VideoSource = "76311230"}
            };
        }
    }
}