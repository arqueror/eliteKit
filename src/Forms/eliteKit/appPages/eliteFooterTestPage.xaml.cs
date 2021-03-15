using SkiaSharp;
using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKitDevelopment.appPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class eliteFooterTestPage : ContentPage
    {
        public eliteFooterTestPage()
        {
            InitializeComponent();
            // 5 fps
            var fps = TimeSpan.FromSeconds(1.0 / 5.0);
            float start = 0;
            Stopwatch watch = new Stopwatch();
            footerImage.Source = ImageSource.FromResource("eliteKitDevelopment.demoImages.backgroundAbstract.png");
            footer.FinishedPresenting += (s, a) =>
            {
                //Create a pulse Animation by playing with the FooterPath prop and a Timer
                watch.Start();
                start = 100;

                //Get Canvas Information
                var width = footer.CanvasData.Info.Width;
                var invert = false;

                Device.StartTimer(fps, () =>
                {

                    if (start < 105 && !invert)
                    {
                        start++;
                    }
                    else
                    {
                        invert = true;
                    }

                    if (start > 100 && invert)
                    {
                        start--;
                    }
                    else
                    {
                        invert = false;
                    }

                    var roundedPath = new SKPath();

                    // get elapsed time
                    var time = (float)watch.Elapsed.TotalMinutes;
                    watch.Restart();

                    roundedPath.MoveTo(0, start);
                    roundedPath.QuadTo(width / 2, start * -1, width, start);
                    roundedPath.LineTo(width, 0);
                    roundedPath.LineTo(0, 0);

                    footer.FooterPath = roundedPath;


                    // continue 
                    return true;

                });


            };

        }
    }
}