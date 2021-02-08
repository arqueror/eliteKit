using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteLoaderRadialCut : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private int sweepAngleLoader = 0;

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteLoaderRadialCut), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderRadialCut)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorPrimary
        {
            get
            {
                return (Color)GetValue(ColorPrimaryProperty);
            }

            set
            {
                SetValue(ColorPrimaryProperty, value);
            }
        }

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteLoaderRadialCut),coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderRadialCut)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorSecondary
        {
            get
            {
                return (Color)GetValue(ColorSecondaryProperty);
            }

            set
            {
                SetValue(ColorSecondaryProperty, value);
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteLoaderRadialCut), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderRadialCut)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        /// 
        /// </summary>
        public bool IsGradient
        {
            get
            {
                return (bool)GetValue(IsGradientProperty);
            }

            set
            {
                SetValue(IsGradientProperty, value);
            }
        }

        public eliteLoaderRadialCut()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteLoaderRadialCutTouched;

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if (this.sweepAngleLoader >= 360)
                    this.sweepAngleLoader = 0;
                else
                    this.sweepAngleLoader += 2;

                this.InvalidateSurface();
                return true;
            });
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            SKPaint loaderPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 30,
                IsAntialias = true
            };

            if (this.IsGradient)
                loaderPaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, this.canvasWidth),
                    new SKPoint(this.canvasHeight, 0),
                    new SKColor[] {
                        this.ColorPrimary.ToSKColor(),
                        this.ColorSecondary.ToSKColor()
                    },
                    new float[] {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

            SKRect loaderRect = new SKRect();
            loaderRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            loaderRect.Location = new SKPoint(15, 15);
            SKPath loaderPath = new SKPath();
            loaderPath.AddArc(loaderRect, this.sweepAngleLoader, 270);
            givenCanvas.DrawPath(loaderPath, loaderPaint);
        }

        private void eliteLoaderRadialCutTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
    }
}
