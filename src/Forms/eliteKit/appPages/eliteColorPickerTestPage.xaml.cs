using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace eliteKitDevelopment.appPages
{
    public partial class eliteColorPickerTestPage : ContentPage
    {
        public eliteColorPickerTestPage()
        {
            InitializeComponent();
            colorPicker.ColorChanged += (s, a) => {
                selectedColor.BackgroundColor = a.Color;
                selectedColor.Text = a.Color.ToHex();
            };
            huePicker.Value = colorPicker.HueValue;
            saturationPicker.Value = colorPicker.SaturationValue;
            luminosityPicker.Value = colorPicker.LightnessValue;
            alphaPicker.Value = colorPicker.AlphaValue;
            UpdateLabel();
        }

        void UpdateLabel()
        {
            curentValue.Text = $"HSL(H:{(huePicker.Value.ToString("0.##"))},S: {saturationPicker.Value.ToString("0.##")}, L:{luminosityPicker.Value.ToString("0.##")}, A:{alphaPicker.Value.ToString("0.##")})";
        }

    }
}
