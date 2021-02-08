using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteProgressRadialCut : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;
        private float progressCurrentTextValue = 0;
        private float progressCurrentSweepAngle = 0;
        private float progressCurrentDesiredSweepAngle = 135;
        private float progressCurrentAnimationStepSweepAngle = 5;

        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteProgressRadialCut), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
                 {
                     if (value != null)
                     {
                         ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteProgressRadialCut), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteProgressRadialCut), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteProgressRadialCut), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

                    }
                
            });

        public static readonly BindableProperty DisplayPercentageProperty = BindableProperty.Create(nameof(DisplayAsPercentage), typeof(bool), typeof(eliteProgressRadialCut), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

                }
            
        });
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayAsPercentage
        {
            get
            {
                return (bool)GetValue(DisplayPercentageProperty);
            }

            set
            {
                SetValue(DisplayPercentageProperty, value);
            }
        }

        public static readonly BindableProperty DisplayValueProperty = BindableProperty.Create(nameof(DisplayValue), typeof(bool), typeof(eliteProgressRadialCut), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

                }
            
        });
        /// <summary>
        /// 
        /// </summary>
        public bool DisplayValue
        {
            get
            {
                return (bool)GetValue(DisplayValueProperty);
            }

            set
            {
                SetValue(DisplayValueProperty, value);
            }
        }

        public static readonly BindableProperty FormatProviderProperty = BindableProperty.Create(nameof(FormatProvider), typeof(IFormatProvider), typeof(eliteProgressRadialCut), null, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

                }
            

        });
        /// <summary>
        /// 
        /// </summary>
        public IFormatProvider FormatProvider
        {
            get
            {
                return (IFormatProvider)GetValue(FormatProviderProperty);
            }

            set
            {
                SetValue(FormatProviderProperty, value);
            }
        }

        public static readonly BindableProperty CustomFormatProperty = BindableProperty.Create(nameof(CustomFormat), typeof(string), typeof(eliteProgressRadialCut), null, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
            {
                ((eliteProgressRadialCut)bindableObject).InvalidateSurface();

            }

        });
        /// <summary>
        /// 
        /// </summary>
        public string CustomFormat
        {
            get
            {
                return (string)GetValue(CustomFormatProperty);
            }

            set
            {
                SetValue(CustomFormatProperty, value);
            }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(TextSize), typeof(float), typeof(eliteProgressRadialCut), 70f, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteProgressRadialCut)bindableObject).InvalidateSurface();
            
        });
        public float TextSize
        {
            get
            {
                return (float)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

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

        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(eliteProgressRadialCut), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressRadialCut)bindableObject).updateValue((int)value);

                }
            });
        /// <summary>
        /// 
        /// </summary>
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

        void updateValue(int newValue)
        {
            decimal oneDegreeInPercent = 100m / 270;
            decimal onePercentInDegree = 270 / 100m;

            float requiredDegrees = (float)onePercentInDegree * this.CurrentValue;
            this.progressCurrentDesiredSweepAngle = requiredDegrees;

            Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
            {
                if ((this.progressCurrentSweepAngle + this.progressCurrentAnimationStepSweepAngle) <= this.progressCurrentDesiredSweepAngle)
                    this.progressCurrentSweepAngle += this.progressCurrentAnimationStepSweepAngle;
                else
                {
                    float progressDifferenceSweepAngle = this.progressCurrentDesiredSweepAngle - this.progressCurrentSweepAngle;
                    this.progressCurrentSweepAngle += progressDifferenceSweepAngle;
                }

                decimal degreesInPercent = (int)this.progressCurrentSweepAngle * oneDegreeInPercent;
                this.progressCurrentTextValue = (float)Math.Round(degreesInPercent);

                this.InvalidateSurface();

                if (this.progressCurrentSweepAngle == this.progressCurrentDesiredSweepAngle)
                    return false;
                else
                    return true;
            });

            this.InvalidateSurface();
        }

        public eliteProgressRadialCut()
        {
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            // Background progress circle
            SKPaint progressPaint = new SKPaint()
            {
                Color = this.ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 30,
                IsAntialias = true,
                StrokeCap = SKStrokeCap.Round
            };
            SKRect progressRect = new SKRect();
            progressRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            progressRect.Location = new SKPoint(15, 15);
            SKPath progressPath = new SKPath();
            progressPath.AddArc(progressRect, 135, 270);
            givenCanvas.DrawPath(progressPath, progressPaint);

            // Current progress circle
            SKPaint progressCurrentPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 30,
                IsAntialias = true
            };

            if (this.IsGradient)
                progressCurrentPaint.Shader = SKShader.CreateLinearGradient(
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

            SKRect progressCurrentRect = new SKRect();
            progressCurrentRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            progressCurrentRect.Location = new SKPoint(15, 15);
            SKPath progressCurrentPath = new SKPath();
            progressCurrentPath.AddArc(progressCurrentRect, 135, this.progressCurrentSweepAngle);
            givenCanvas.DrawPath(progressCurrentPath, progressCurrentPaint);

            // Current progress text
            SKPaint progrssCurrentTextPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                TextSize = TextSize,
                IsAntialias = true
            };
            var percentageIndicator = DisplayAsPercentage ? "%" : "";
            if (DisplayValue)
            {
                if(FormatProvider == null || CustomFormat == null)
                    givenCanvas.DrawText(string.Format("{0}" + $"{percentageIndicator}", this.progressCurrentTextValue), (canvasWidth + 20) / 2, (canvasHeight + 20 + 35) / 2, progrssCurrentTextPaint);

                if(FormatProvider != null && CustomFormat != null)
                    givenCanvas.DrawText(string.Format(FormatProvider ,CustomFormat, this.progressCurrentTextValue), (canvasWidth + 20) / 2, (canvasHeight + 20 + 35) / 2, progrssCurrentTextPaint);
            }
        }
    }
}
