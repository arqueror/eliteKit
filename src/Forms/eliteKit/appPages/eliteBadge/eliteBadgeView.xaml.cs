using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKitDevelopment.appPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class eliteBadgeView : ContentPage
    {
        public eliteBadgeView()
        {
            InitializeComponent();
            badgeView.Source = ImageSource.FromResource("eliteKitDevelopment.demoImages.backgroundAbstract.png");
         
        }
    }
}