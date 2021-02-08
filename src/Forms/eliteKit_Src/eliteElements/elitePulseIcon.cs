using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class elitePulseIcon : SKCanvasView
    {
        public event EventHandler ButtonClick;
        SKBitmap resourceBitmap;
        double cycleTime = 30000;
        Stopwatch stopwatch = new Stopwatch();
        public bool IsRun;
        float[] t = new float[3];
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke
        };
        private SKPaint backPaint;

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(elitePulseIcon), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((elitePulseIcon)bindableObject).InvalidateSurface();

                }
            
        });
        public bool HasShadow
        {
            get
            {
                return (bool)GetValue(HasShadowProperty);
            }
            set
            {
                SetValue(HasShadowProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(elitePulseIcon), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((elitePulseIcon)bindableObject).InvalidateSurface();
            
        });
        public bool IsGradient
        {
            get
            {
                return (bool)GetValue(IsGradientProperty);
            }
            set
            {
                SetValue(IsGradientProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(elitePulseIcon), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((elitePulseIcon)bindableObject).InvalidateSurface();
            
        });
        public Color ColorPrimary
        {
            get
            {
                return (Color)GetValue(ColorPrimaryProperty);
            }
            set
            {
                SetValue(ColorPrimaryProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(elitePulseIcon), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((elitePulseIcon)bindableObject).InvalidateSurface();
            
        });
        public Color ColorSecondary
        {
            get
            {
                return (Color)GetValue(ColorSecondaryProperty);
            }
            set
            {
                SetValue(ColorSecondaryProperty, value);
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty PulseRadiusProperty =
          BindableProperty.Create(nameof(PulseRadius), typeof(int), typeof(elitePulseIcon), 4, propertyChanged: (bindableObject, oldValue, value) =>
          {
              var icon = (elitePulseIcon)bindableObject;
              icon.InvalidateSurface();
          });

        public static readonly BindableProperty RadiusProperty =
         BindableProperty.Create(nameof(Radius), typeof(int), typeof(elitePulseIcon), 100, propertyChanged: (bindableObject, oldValue, value) =>
         {
             var icon = (elitePulseIcon)bindableObject;


         });

        public static readonly BindableProperty PulseColorProperty =
        BindableProperty.Create(nameof(PulseColor), typeof(Color), typeof(elitePulseIcon), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != PulseColorProperty.DefaultValue)
                {
                    ((elitePulseIcon)bindableObject).PulseColor = (Color)PulseColorProperty.DefaultValue;
                    return;
                }
            
        });

        public static readonly BindableProperty IsPulsingProperty =
        BindableProperty.Create(nameof(IsPulsing), typeof(bool), typeof(elitePulseIcon), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                var icon = (elitePulseIcon)bindableObject;
                if (icon.IsPulsing)
                {
                    icon.start();
                    icon.InvalidateSurface();
                }
                else
                {
                    icon.IsRun = false;
                    icon.InvalidateSurface();
                }
            
        });

        public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(Stream), typeof(elitePulseIcon), null, propertyChanged: (bindableObject, oldValue, value) =>
        {
            var icon = (elitePulseIcon)bindableObject;
            var newImageStream = value as Stream;
            if (newImageStream != null)
            {
                if (newImageStream != null)
                   icon.resourceBitmap = SKBitmap.Decode(newImageStream);
            }

            //icon.resourceBitmap = icon.resourceBitmap?.Resize(new SKImageInfo((int)icon.Radius , (int)icon.Radius ), SKFilterQuality.High);
        });

        public static readonly BindableProperty SpeedProperty =
        BindableProperty.Create(nameof(Speed), typeof(int), typeof(elitePulseIcon), 10, propertyChanged: (bindableObject, oldValue, value) =>
        {
            var icon = (elitePulseIcon)bindableObject;
            icon.Cycletime /= icon.Speed;
            icon.SetNewCycleSpeed();
            icon.InvalidateSurface();
        });

        public static readonly BindableProperty ButtonClickCommandProperty = BindableProperty.Create(nameof(ButtonClickCommand), typeof(ICommand), typeof(elitePulseIcon));
        public ICommand ButtonClickCommand
        {
            get => (ICommand)GetValue(ButtonClickCommandProperty);
            set => SetValue(ButtonClickCommandProperty, value);
        }


        public Color PulseColor
        {
            get { return (Color)GetValue(PulseColorProperty); }
            set { SetValue(PulseColorProperty, value); }
        }
        public bool IsPulsing
        {
            get { return (bool)GetValue(IsPulsingProperty); }
            set { SetValue(IsPulsingProperty, value); }
        }
        public Stream Source
        {
            get { return (Stream)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public int Speed
        {
            get { return (int)GetValue(SpeedProperty); }
            set
            {
                SetValue(SpeedProperty, value);
            }
        }

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public int PulseRadius
        {
            get { return (int)GetValue(PulseRadiusProperty); }
            set { SetValue(PulseRadiusProperty, value); }
        }

        public double Cycletime
        {
            get => cycleTime;
            private set
            {
                cycleTime = value;
            }
        }

        public elitePulseIcon()
        {

            cycleTime /= Speed;
            EnableTouchEvents = true;
            Touch += iconTouched;

            if (IsPulsing)
            {
                start();
                InvalidateSurface();
            }

        }
        void SetNewCycleSpeed()
        {
            cycleTime *= Speed;
        }
        void start()
        {
            IsRun = true;
            stopwatch.Start();
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
            {
                t[0] = (float)(stopwatch.Elapsed.TotalMilliseconds % cycleTime / cycleTime);
                if (stopwatch.Elapsed.TotalMilliseconds > cycleTime / 3)
                    t[1] = (float)((stopwatch.Elapsed.TotalMilliseconds - cycleTime / 3) % cycleTime / cycleTime);
                if (stopwatch.Elapsed.TotalMilliseconds > cycleTime * 2 / 3)
                    t[2] = (float)((stopwatch.Elapsed.TotalMilliseconds - cycleTime * 2 / 3) % cycleTime / cycleTime);
                this.InvalidateSurface();

                if (!IsRun)
                {
                    stopwatch.Stop();
                    stopwatch.Reset();
                }
                return IsRun;
            });
        }
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            base.OnPaintSurface(args);
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            backPaint = new SKPaint()
            {
                Color = ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            if (HasShadow)
            {
                this.backPaint.ImageFilter = SKImageFilter.CreateDropShadow(
                    0,
                    8,
                    0,
                    5,
                    Color.FromRgba(0, 0, 0, 0.4).ToSKColor(),
                    SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                );
            }
            if (this.IsGradient)
                backPaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint((info.Width - info.Height), 0),
                    new SKPoint(info.Width, info.Height),
                    new[]
                    {
                        ColorPrimary.ToSKColor(),
                        ColorSecondary.ToSKColor()
                    },
                    new float[]
                    {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

            canvas.Clear();
            byte R = (byte)(PulseColor.R * 255);
            byte G = (byte)(PulseColor.G * 255);
            byte B = (byte)(PulseColor.B * 255);

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            float radius = 0;
            if (IsRun)
            {
                if (PulseRadius <= 0)
                    PulseRadius = 1;

                for (int i = 0; i < t.Length; i++)
                {
                    radius = info.Width / PulseRadius * (t[i]);
                    paint.Color = new SKColor(R, G, B, (byte)(255 * (1 - t[i])));
                    paint.Style = SKPaintStyle.Fill;
                    canvas.DrawCircle(center.X, center.Y, radius, paint);
                }

            }
            canvas.DrawCircle(center.X, center.Y, (int)Radius - (Radius / 10), backPaint);
            var resizedBitmap = resourceBitmap?.Resize(new SKImageInfo((int)Radius, (int)Radius), SKFilterQuality.High);
            if (resizedBitmap != null)
            {
                canvas.DrawBitmap(resizedBitmap, center.X - resizedBitmap.Width / 2, center.Y - resizedBitmap.Height / 2);
            }
        }

        private void iconTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:

                    ButtonClick?.Invoke(this, null);
                    ButtonClickCommand?.Execute(null);
                    break;
            }

            eventArgs.Handled = true;
        }
    }
}
