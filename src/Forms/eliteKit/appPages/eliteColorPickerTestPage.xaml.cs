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

        void Slider_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            colorPicker.HueValue = (float)e.NewValue;

            UpdateLabel();
        }

        void Slider_ValueChanged_1(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            colorPicker.SaturationValue = (float)e.NewValue;
            UpdateLabel();
        }

        void Slider_ValueChanged_2(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            colorPicker.LightnessValue = (float)e.NewValue;
            UpdateLabel();
        }

        void UpdateLabel()
        {
            curentValue.Text = $"HSL(H:{(huePicker.Value.ToString("0.##"))},S: {saturationPicker.Value.ToString("0.##")}, L:{luminosityPicker.Value.ToString("0.##")}, A:{alphaPicker.Value.ToString("0.##")})";
        }

        void alphaPicker_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            colorPicker.AlphaValue = (byte)e.NewValue;
            UpdateLabel();
        }
    }
}