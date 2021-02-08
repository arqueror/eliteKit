using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteProgressSteps : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;
        private int stepSweepAngle = 0;

        public static readonly BindableProperty StepCurrentProperty = BindableProperty.Create(nameof(StepCurrent), typeof(int), typeof(eliteProgressSteps), 2, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressSteps)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        /// 
        /// </summary>
        public int StepCurrent
        {
            get
            {
                return (int)GetValue(StepCurrentProperty);
            }
            set
            {
                SetValue(StepCurrentProperty, value);
            }
        }

        public static readonly BindableProperty StepMaxProperty = BindableProperty.Create(nameof(StepMax), typeof(int), typeof(eliteProgressSteps), 4, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteProgressSteps)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        /// 
        /// </summary>
        public int StepMax
        {
            get
            {
                return (int)GetValue(StepMaxProperty);
            }
            set
            {
                SetValue(StepMaxProperty, value);
            }
        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteProgressSteps), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressSteps)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteProgressSteps), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressSteps)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorInactiveStepsProperty = BindableProperty.Create(nameof(ColorInactiveSteps), typeof(Color), typeof(eliteProgressSteps), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {

                if (value != null)
                {
                    ((eliteProgressSteps)bindableObject).InvalidateSurface();

                }

            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorInactiveSteps
        {
            get
            {
                return (Color)GetValue(ColorInactiveStepsProperty);
            }
            set
            {
                SetValue(ColorInactiveStepsProperty, value);
            }
        }

        public static readonly BindableProperty ColorActiveStepsProperty = BindableProperty.Create(nameof(ColorActiveSteps), typeof(Color), typeof(eliteProgressSteps), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressSteps)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorActiveSteps
        {
            get
            {
                return (Color)GetValue(ColorActiveStepsProperty);
            }
            set
            {
                SetValue(ColorActiveStepsProperty, value);
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteProgressSteps), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteProgressSteps)bindableObject).InvalidateSurface();

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

        public eliteProgressSteps()
        {
            this.stepSweepAngle = 360 / this.StepMax;
        }

        public void updateStep(int stepCurrent)
        {
            this.StepCurrent = stepCurrent;
            this.InvalidateSurface();
        }

        public void increaseStep()
        {
            this.StepCurrent++;
            this.InvalidateSurface();
        }

        public void decreaseStep()
        {
            this.StepCurrent--;
            this.InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            // Draw background steps
            SKPaint paintBackgroundSteps = new SKPaint()
            {
                Color = this.ColorInactiveSteps.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 30,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };

            SKRect progressRect = new SKRect();
            progressRect.Size = new SKSize(this.canvasWidth - 35, this.canvasHeight - 35);
            progressRect.Location = new SKPoint(15, 15);

            int startAngle = -90;
            int requiredOffset = 15;
            int requiredAngle = (360 - (this.StepMax * requiredOffset)) / this.StepMax;

            for (int i = 0; i < this.StepMax; i++)
            {
                SKPath progressPath = new SKPath();
                progressPath.AddArc(progressRect, startAngle, requiredAngle);
                givenCanvas.DrawPath(progressPath, paintBackgroundSteps);

                startAngle += (requiredAngle + requiredOffset);
            }

            // Draw active steps
            SKPaint paintActiveSteps = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeWidth = 30,
                IsAntialias = true
            };

            if (this.IsGradient)
                paintActiveSteps.Shader = SKShader.CreateLinearGradient(
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

            for (int i = 0; i < this.StepCurrent; i++)
            {
                SKPath progressPath = new SKPath();
                progressPath.AddArc(progressRect, startAngle, requiredAngle);
                givenCanvas.DrawPath(progressPath, paintActiveSteps);

                startAngle += (requiredAngle + requiredOffset);
            }

            // Current progress text
            SKPaint progressCurrentTextPaint = new SKPaint()
            {
                Color = this.ColorActiveSteps.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                TextSize = 70f,
                IsAntialias = true
            };

            string currentText = string.Format("{0}/{1}", this.StepCurrent, this.StepMax);

            SKRect rectCurrentText = new SKRect();
            progressCurrentTextPaint.MeasureText(currentText, ref rectCurrentText);

            givenCanvas.DrawText(currentText, canvasWidth / 2, canvasHeight / 2 - rectCurrentText.MidY, progressCurrentTextPaint);
        }
    }
}