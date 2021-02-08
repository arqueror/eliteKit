using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteProgressBar : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;
        private float oneStepInWidth = 0;

        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteProgressBar), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressBar)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorBackground
        {
            get
            {
                return (Color)GetValue(ColorBackgroundProperty);
            }
            set
            {
                SetValue(ColorBackgroundProperty, value);
            }
        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteProgressBar), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressBar)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteProgressBar), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressBar)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteProgressBar), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressBar)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(eliteProgressBar), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressBar)bindableObject).InvalidateSurface();

                }
            });
            
        public int CurrentValue
        {
            get
            {
                return (int)GetValue(CurrentValueProperty);
            }

            set
            {
                SetValue(CurrentValueProperty, value);
            }
        }

        public eliteProgressBar()
        {
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            this.oneStepInWidth = this.canvasWidth / 100f;

            SKRoundRect progressRoundRectBackground = new SKRoundRect(new SKRect(0, 0, canvasWidth, canvasHeight), (canvasHeight / 2), (canvasHeight / 2));
            SKPaint progressPaintBackground = new SKPaint()
            {
                Color = this.ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
            givenCanvas.DrawRoundRect(progressRoundRectBackground, progressPaintBackground);

            SKRoundRect progressRoundRectCurrent = new SKRoundRect(new SKRect(0, 0, this.oneStepInWidth * this.CurrentValue, canvasHeight), (canvasHeight / 2), (canvasHeight / 2));
            SKPaint progressPaintCurrent = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor()
            };

            if (this.IsGradient)
                progressPaintCurrent.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(progressRoundRectCurrent.Rect.Left, progressRoundRectCurrent.Rect.Top),
                    new SKPoint(progressRoundRectCurrent.Rect.Right, progressRoundRectCurrent.Rect.Bottom),
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

            givenCanvas.DrawRoundRect(progressRoundRectCurrent, progressPaintCurrent);
        }
    }
}
