using SkiaSharp;
using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eliteKitDevelopment.appPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class eliteHeaderTestPage : ContentPage
    {
        public eliteHeaderTestPage()
        {
            InitializeComponent();
            // 5 fps
            var fps = TimeSpan.FromSeconds(1.0 / 5.0);
            float start = 0;
            Stopwatch watch = new Stopwatch();
            headerImage.Source = ImageSource.FromResource("eliteKitDevelopment.demoImages.backgroundAbstract.png");
            header.FinishedPresenting += (s, a) =>
            {
                //Create a pulse Animation by playing with the FooterPath prop and a Timer
                watch.Start();
                start = 100;

                //Get Canvas Information
                var height = header.CanvasData.Info.Height;
                var width = header.CanvasData.Info.Width;
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

                    roundedPath.MoveTo(0, height - start);
                    roundedPath.QuadTo(width / 2, height + 100, width, height - start);
                    roundedPath.LineTo(width, height);
                    roundedPath.LineTo(0, height);
                    roundedPath.LineTo(0, height - 100);

                    header.HeaderPath = roundedPath;


                    // continue 
                    return true;

                });


            };
        }
    }
}