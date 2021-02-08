using System.Reflection;
using eliteKit.eliteElements;
using eliteKit.Extensions;
using Xamarin.Forms;

namespace eliteKitDevelopment.appPages
{
    class pageTest : ContentPage
    {
        public pageTest()
        {
            this.BackgroundColor = Color.LightBlue;

            elitePulseIcon pulseButton = new elitePulseIcon()
            {
                IsPulsing = true,
                IsGradient = true,
                HasShadow = true,
                ColorPrimary = Color.Blue,
                ColorSecondary = Color.Yellow,
                PulseColor = Color.FromHex("#8e44ad"),
                Speed = 20,
                Radius = 150,
                PulseRadius = 2,
                Source = this.GetAssemblyResourceStream("eliteKitDevelopment.demoImages.imagesIcons.iconMenu.png")

            };

            this.Content = new Grid()
            {

                Children =
                {
                   pulseButton
                }
            };
        }
    }
}
