using eliteKit.eliteCore;
using eliteKit.eliteEnums;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteLoaderBar : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;
        private int barAnimationSpeed = 25;

        private float currentPosition = 0;
        private float currentWidth = 0;
        private bool floatingDirection = false;

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteLoaderBar), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderBar)bindableObject).InvalidateSurface();
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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteLoaderBar), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderBar)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteLoaderBar), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteLoaderBar)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty AnimationSpeedProperty = BindableProperty.Create(nameof(TransitionSpeed), typeof(eliteSpeed), typeof(eliteLoaderBar), eliteSpeed.Normal, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        var val = (eliteSpeed)value;
                        switch (val)
                        {
                            case eliteSpeed.Slow:
                                ((eliteLoaderBar)bindableObject).barAnimationSpeed = 15;
                                break;
                            case eliteSpeed.Normal:
                                ((eliteLoaderBar)bindableObject).barAnimationSpeed = 25;
                                break;
                            case eliteSpeed.Fast:
                                ((eliteLoaderBar)bindableObject).barAnimationSpeed = 35;
                                break;
                        }

                    }
                
            });
        /// <summary>
        /// 
        /// </summary>
        public eliteSpeed TransitionSpeed
        {
            get { return (eliteSpeed)GetValue(AnimationSpeedProperty); }
            set
            {
                SetValue(AnimationSpeedProperty, value);
            }
        }

        public eliteLoaderBar()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteLoaderBarTouched;

            Device.StartTimer(TimeSpan.FromMilliseconds(15), () =>
            {
                if (this.floatingDirection)
                {
                    if (this.currentPosition < (this.canvasWidth - this.currentWidth))
                    {
                        if (this.currentPosition + barAnimationSpeed > this.canvasWidth - this.currentWidth)
                            this.currentPosition = this.canvasWidth - this.currentWidth;
                        else
                            this.currentPosition += barAnimationSpeed;
                    }
                    else
                        this.floatingDirection = false;
                }
                else
                {
                    if (this.currentPosition > 0)
                    {
                        if (this.currentPosition - barAnimationSpeed < 0)
                            this.currentPosition = 0;
                        else
                            this.currentPosition -= barAnimationSpeed;
                    }
                    else
                        this.floatingDirection = true;
                }

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

            this.currentWidth = this.canvasWidth / 4;

            SKRoundRect loaderRoundRectBackground = new SKRoundRect(new SKRect(0, 0, canvasWidth, canvasHeight), (canvasHeight / 2), (canvasHeight / 2));
            SKPaint loaderPaintBackground = new SKPaint()
            {
                Color = Color.LightGray.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
            givenCanvas.DrawRoundRect(loaderRoundRectBackground, loaderPaintBackground);

            SKRoundRect loaderRoundRectCurrent = new SKRoundRect(new SKRect(this.currentPosition, 0, this.currentPosition + this.currentWidth, canvasHeight), (canvasHeight / 2), (canvasHeight / 2));
            SKPaint loaderPaintCurrent = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = this.ColorPrimary.ToSKColor(),
                IsAntialias = true
            };

            if (this.IsGradient)
                loaderPaintCurrent.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(loaderRoundRectCurrent.Rect.Left, loaderRoundRectCurrent.Rect.Top),
                    new SKPoint(loaderRoundRectCurrent.Rect.Right, loaderRoundRectCurrent.Rect.Bottom),
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

            givenCanvas.DrawRoundRect(loaderRoundRectCurrent, loaderPaintCurrent);
        }

        private void eliteLoaderBarTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            eventArgs.Handled = true;
        }
    }
}
