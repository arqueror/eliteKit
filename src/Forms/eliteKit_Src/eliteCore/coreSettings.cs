using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
    }
}