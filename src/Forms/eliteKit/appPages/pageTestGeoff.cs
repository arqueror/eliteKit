using System;
using System.Reflection;
using eliteKit.eliteElements;
using eliteKit.eliteElements.BaseElements;
using eliteKit.eliteEnums;
using Xamarin.Forms;

namespace eliteKitDevelopment.appPages
{
    class pageTestGeoff : ContentPage
    {
        int _seed = 0;
        eliteSignature _signaturePanel;
        Image _image;

        public pageTestGeoff()
        {
            BackgroundColor = Color.White;

            _signaturePanel = new eliteSignature
            {
                PenColor = Color.Red,
                PenThickness = 10.0f,
                HeightRequest = 200,
                BorderWidth = 5f,
                BorderStyle = BorderStyle.RoundedRectangular,
                BorderCornerRadius = 20f,
                BackgroundColorPrimary = Color.White,
                PenMode = SignaturePenMode.ActiveTrace
            };

            _image = new Image
            {
                HeightRequest = 200
            };
            
            Content = new StackLayout()
            {
                Padding = new Thickness(40),
                Children =
                {
                   _signaturePanel,
                   _image,
                   new eliteButton
                   {
                       HeightRequest = 60,
                       ButtonTitle = "Clear",
                       ButtonClickCommand = new Command(() =>
                       {
                           _signaturePanel.Clear();
                       })
                   },
                   new eliteButton
                   {
                       HeightRequest = 60,
                       ButtonTitle = "Save",
                       ButtonClickCommand = new Command(() =>
                       {
                           SaveStream();
                       })
                   }
                }
            };
        }

        async void SaveStream()
        {
            var stream = await _signaturePanel.GetImageStream();
            _image.Source = ImageSource.FromStream(() => stream);
        }

        void addIcons()
        {
            var assembly = GetType().GetTypeInfo().Assembly;

            string svgResourceId = "eliteKitDevelopment.demoImages.imagesIcons.globe.svg";
            eliteIcon svgIcon;

            using (var stream = assembly.GetManifestResourceStream(svgResourceId))
            {
                var source = new IconSource
                {
                    Stream = stream,
                    FileName = svgResourceId
                };

                svgIcon = new eliteIcon
                {
                    IconSource = source,
                    HeightRequest = 150,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BorderWidth = 10f,
                    Padding = new Thickness(70.0),
                    BorderStyle = BorderStyle.Oval,
                    IsGradient = true
                };
            }
            
            var pngResourceId = "eliteKitDevelopment.demoImages.imagesIcons.iconSettings.png";
            eliteIcon pngIcon;

            using (var stream = assembly.GetManifestResourceStream(pngResourceId))
            {
                var source = new IconSource
                {
                    Stream = stream,
                    FileName = pngResourceId
                };

                pngIcon = new eliteIcon
                {
                    IconSource = source,
                    HeightRequest = 150,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    BorderWidth = 10f,
                    Padding = new Thickness(70.0),
                    BorderStyle = BorderStyle.Rectangular,
                    IsGradient = true
                };
            }

            var faIcon = new eliteIcon
            {
                IconSource = new IconSource { GlyphValue = "\uf08a" },
                HeightRequest = 150,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BorderWidth = 10f,
                Padding = new Thickness(70.0),
                BorderStyle = BorderStyle.RoundedRectangular,
                BorderCornerRadius = 50f,
                IsGradient = true
            };

            Content = new StackLayout()
            {
                Padding = new Thickness(40),
                Children =
                {
                   svgIcon,
                   pngIcon,
                   faIcon,
                   new eliteButton
                   {
                       HeightRequest = 60,
                       ButtonTitle = "RND Color",
                       ButtonClickCommand = new Command(() =>
                       {
                           var c1 = GetRandomColor();
                           var c2 = GetRandomColor();
                           var c3 = GetRandomColor();

                           svgIcon.IconColor = c1;
                           svgIcon.BorderColor = c1;

                           pngIcon.IconColor = c2;
                           pngIcon.BorderColor = c2;

                           faIcon.IconColor = c3;
                           faIcon.BorderColor = c3;
                       })
                   }
                }
            };
        }

        Color GetRandomColor()
        {
            var rnd = new Random(_seed++);

            var red = rnd.Next(byte.MinValue, byte.MaxValue) / 255f;
            var green = rnd.Next(byte.MinValue, byte.MaxValue) / 255f;
            var blue = rnd.Next(byte.MinValue, byte.MaxValue) / 255f;

            return new Color(red, green, blue);
        }
    }
}
