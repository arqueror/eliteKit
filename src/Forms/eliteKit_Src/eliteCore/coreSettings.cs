using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace eliteKit.eliteCore
{
    class coreSettings
    {
        public static Color ColorPrimary { get; set; } = Color.FromHex("#548EC1");
        public static Color ColorSecondary { get; set; } = Color.FromHex("#254867");
        public static Color ColorHighlight { get; set; } = Color.FromHex("#3674A3");
        public static string DefaultFontFamily { get; set; } = SKTypeface.Default.FamilyName;
        public static HttpClient HttpClientSingleton = new HttpClient();

        /// <summary>
        /// Represents the scaling factor used to present specific elements like Fonts or primitives with a correct proportion based on device density.
        /// </summary>
        public static float ScalingFactor
        {
            get
            {
                var screenWidthDpi = DeviceDisplay.MainDisplayInfo;
                var scalingFactor = (((screenWidthDpi.Height / 2) / screenWidthDpi.Width)) * screenWidthDpi.Density;

                return (scalingFactor > 0 ? (float)scalingFactor : 1);
            }
        }
    }
}