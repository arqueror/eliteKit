using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteLoaderRadial : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private int sweepAngleLoader = 135;
        private int sweepAngleIndicator = 45;

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteLoaderRadial), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteLoaderRadial)bindableObject).InvalidateSurface();

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
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteLoaderRadial), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteLoaderRadial)bindableObject).InvalidateSurface();

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
                this.InvalidateSurface();
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteLoaderRadial), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteLoaderRadial)bindableObject).InvalidateSurface();

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
                this.InvalidateSurface();
            }
        }

        public eliteLoaderRadial()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteLoaderRadialTouched;

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if (this.sweepAngleLoader >= 360)
                    this.sweepAngleLoader = 0;
                else
                    this.sweepAngleLoader += 2;

                if (this.sweepAngleIndicator >= 360)
                    this.sweepAngleIndicator = 0;
                else
                    this.sweepAngleIndicator += 2;

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
                Color = Color.LightGray.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 30,
                IsAntialias = true
            };
            SKRect loaderRect = new SKRect();
            loaderRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            loaderRect.Location = new SKPoint(15, 15);
            SKPath loaderPath = new SKPath();
            loaderPath.AddArc(loaderRect, this.sweepAngleLoader, 270);
            givenCanvas.DrawPath(loaderPath, loaderPaint);

            SKPaint loaderIndicatorPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                StrokeCap = SKStrokeCap.Round,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 30,
                IsAntialias = true
            };

            if (this.IsGradient)
                loaderIndicatorPaint.Shader = SKShader.CreateLinearGradient(
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

            SKRect loaderIndicatorRect = new SKRect();
            loaderIndicatorRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            loaderIndicatorRect.Location = new SKPoint(15, 15);
            SKPath loaderIndicatorPath = new SKPath();
            loaderIndicatorPath.AddArc(loaderIndicatorRect, this.sweepAngleIndicator, 90);
            givenCanvas.DrawPath(loaderIndicatorPath, loaderIndicatorPaint);
        }

        private void eliteLoaderRadialTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
    }
}
