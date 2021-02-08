using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteProgressRadial : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;
        private float progressCurrentTextValue = 0;
        private float progressCurrentSweepAngle = 0;
        private float progressCurrentDesiredSweepAngle = 270;
        private float progressCurrentAnimationStepSweepAngle = 5;

        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteProgressRadial), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
               {
                   if (value != null)
                   {
                       ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteProgressRadial), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteProgressRadial), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteProgressRadial), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty DisplayPercentageProperty = BindableProperty.Create(nameof(DisplayAsPercentage), typeof(bool), typeof(eliteProgressRadial), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty DisplayValueProperty = BindableProperty.Create(nameof(DisplayValue), typeof(bool), typeof(eliteProgressRadial), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadial)bindableObject).InvalidateSurface();

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


        public static readonly BindableProperty FormatProviderProperty = BindableProperty.Create(nameof(FormatProvider), typeof(IFormatProvider), typeof(eliteProgressRadial), null, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty CustomFormatProperty = BindableProperty.Create(nameof(CustomFormat), typeof(string), typeof(eliteProgressRadial), null, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
            {
                ((eliteProgressRadial)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(TextSize), typeof(float), typeof(eliteProgressRadial), 70f, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteProgressRadial)bindableObject).InvalidateSurface();
            
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

        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(eliteProgressRadial), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressRadial)bindableObject).updateValue((int)value);

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

        //private string imageBackground = "";
        //public string ImageBackground
        //{
        //    get
        //    {
        //        return this.imageBackground;
        //    }
        //    set
        //    {
        //        this.imageBackground = value;
        //        this.InvalidateSurface();
        //    }
        //}

        void updateValue(int newValue)
        {

            decimal oneDegreeInPercent = 100m / 360;
            decimal onePercentInDegree = 360 / 100m;

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
        }

        public eliteProgressRadial()
        {
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            SKBitmap backgroundImageBitmap = new SKBitmap();

            //// Background image
            //if (this.imageBackground != "")
            //{
            //    string resourceID = this.imageBackground;
            //    Assembly assembly = GetType().GetTypeInfo().Assembly;

            //    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            //    {
            //        backgroundImageBitmap = SKBitmap.Decode(stream);
            //    }
            //}

            // Background progress circle
            SKPaint progressPaint = new SKPaint()
            {
                Color = this.ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeJoin = SKStrokeJoin.Round,
                IsStroke = true,
                StrokeWidth = 30,
                IsAntialias = true
            };
            SKRect progressRect = new SKRect();
            progressRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            progressRect.Location = new SKPoint(15, 15);
            SKPath progressPath = new SKPath();
            progressPath.AddArc(progressRect, -90, 360);

            // Current progress circle
            SKPaint progressCurrentPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeJoin = SKStrokeJoin.Round,
                IsStroke = true,
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
            progressCurrentPath.AddArc(progressCurrentRect, -90, this.progressCurrentSweepAngle);

            // Current progress text
            SKPaint progrssCurrentTextPaint = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                TextSize = TextSize,
                IsAntialias = true
            };

            givenCanvas.ClipPath(progressPath, SKClipOperation.Intersect, true);

            //if(this.imageBackground != "")
            //{
            //    givenCanvas.DrawBitmap(
            //        backgroundImageBitmap,
            //        new SKRect(
            //            0,
            //            0,
            //            this.canvasWidth,
            //            this.canvasHeight
            //        )
            //    );
            //}

            givenCanvas.DrawPath(progressPath, progressPaint);
            givenCanvas.DrawPath(progressCurrentPath, progressCurrentPaint);
            var percentageIndicator = DisplayAsPercentage ? "%" : "";
            if (DisplayValue)
            {
                if(FormatProvider == null || CustomFormat == null)
                    givenCanvas.DrawText(string.Format("{0}" + $"{percentageIndicator}", this.progressCurrentTextValue), (canvasWidth + 20) / 2, (canvasHeight + 20 + 35) / 2, progrssCurrentTextPaint);
                
                if (FormatProvider != null && CustomFormat != null)
                    givenCanvas.DrawText(string.Format(FormatProvider,CustomFormat, this.progressCurrentTextValue), (canvasWidth + 20) / 2, (canvasHeight + 20 + 35) / 2, progrssCurrentTextPaint);

            }
        }
    }
}