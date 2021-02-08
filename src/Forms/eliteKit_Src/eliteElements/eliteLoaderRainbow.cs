using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Collections.Generic;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class eliteLoaderRainbow : SKCanvasView
    {
        private string _progressAnim = "pa";
        private string _rotAnim = "ra";
        private SKColor _currentColor;
        private double _progress = 0.0;
        private double _rotation = 0.0;
        private int _currentColorIndex = 0;
        private float _progressArcDiameterProportion = 0.65f;

        #region Binding Properties
        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteLoaderRainbow), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteLoaderRainbow)bindableObject).InvalidateSurface();
            
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

        public static readonly BindableProperty ProgressDurationProperty = BindableProperty.Create(nameof(ProgressDuration),
           typeof(int),
           typeof(eliteLoaderRainbow),
           2000, propertyChanged: (bindableObject, oldValue, value) =>
           {

           });

        public int ProgressDuration
        {
            get => (int)GetValue(ProgressDurationProperty);
            set => SetValue(ProgressDurationProperty, value);
        }

        public static readonly BindableProperty RotationDurationProperty = BindableProperty.Create(nameof(RotationDuration),
            typeof(int),
            typeof(eliteLoaderRainbow),
            2000, propertyChanged: (bindableObject, oldValue, value) =>
            {

            });

        public int RotationDuration
        {
            get => (int)GetValue(ProgressDurationProperty);
            set => SetValue(ProgressDurationProperty, value);
        }

        public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),
            typeof(Color),
            typeof(eliteLoaderRainbow),
            Color.White, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != BackgroundColorProperty.DefaultValue)
                    {
                        ((eliteLoaderRainbow)bindableObject).BackgroundColor = (Color)BackgroundColorProperty.DefaultValue;
                        return;
                    }
                
            });

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }


        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow),
            typeof(bool),
            typeof(eliteLoaderRainbow),
            true, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != HasShadowProperty.DefaultValue)
                    {
                        ((eliteLoaderRainbow)bindableObject).HasShadow = (bool)HasShadowProperty.DefaultValue;
                        return;
                    }
                
            });

        public bool HasShadow
        {
            get => (bool)GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static readonly BindableProperty ProgressColorsProperty = BindableProperty.Create(nameof(ProgressColors),
            typeof(List<Color>),
            typeof(eliteLoaderRainbow),
            new List<Color>
            {
                new Color(66/255.0f,133/255.0f,244/255.0f),
                new Color(219/255.0f,68/255.0f,55/255.0f),
                new Color(244/255.0f,160/255.0f,0/255.0f),
                new Color(15/255.0f,157/255.0f,88/255.0f)

            });

        public List<Color> ProgressColors
        {
            get => (List<Color>)GetValue(ProgressColorsProperty);
            set => SetValue(ProgressColorsProperty, value);
        }
        #endregion

        SKPaint backgroundPaint = new SKPaint()
        {
            Color = SKColors.White,
            IsStroke = false,
            IsAntialias = true,
        };

        SKPaint progressPaint = new SKPaint()
        {
            Color = SKColors.White,
            IsStroke = true,
            StrokeWidth = 10,
            IsAntialias = true
        };



        protected override void OnParentSet()
        {
            base.OnParentSet();

            _currentColor = ProgressColors[_currentColorIndex].ToSKColor();
            progressPaint.Color = _currentColor;

            RunProgressAnimation();
            RunRotationAnimation();
        }

        private void RunRotationAnimation()
        {
            var rotationAnimation = new Animation(interpolatedValue =>
            {
                _rotation = interpolatedValue;
                InvalidateSurface();
            });

            rotationAnimation.Commit(this, _rotAnim,
                length: (uint)RotationDuration,
                easing: Easing.Linear,
                repeat: () => true);
        }

        private void RunProgressAnimation()
        {
            var progressAnimation = new Animation(interpolated =>
            {
                _progress = interpolated;
                InvalidateSurface();
            });

            progressAnimation.Commit(this, _progressAnim,
                length: (uint)ProgressDuration,
                easing: Easing.CubicInOut,
                finished: (d, b) =>
                {
                    _currentColorIndex++;
                    _currentColorIndex = _currentColorIndex % ProgressColors.Count;
                    _currentColor = ProgressColors[_currentColorIndex].ToSKColor();
                    InvalidateSurface();
                },
                repeat: () => true);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            var info = args.Info;
            var size = info.Size;
            var surface = args.Surface;
            var canvas = args.Surface.Canvas;

            var canvasCenter = new SKPoint(size.Width / 2.0f, size.Height / 2.0f);

            // Clear the canvas and move the canvas center point to the viewport center
            canvas.Clear();
            canvas.Translate(canvasCenter);

            // Draw background as a disc
            backgroundPaint.Color = BackgroundColor.ToSKColor();
            if (HasShadow)
            {
                backgroundPaint.ImageFilter = SKImageFilter.CreateDropShadow(
                    0,
                    8,
                    0,
                    5,
                    Color.FromRgba(0, 0, 0, 0.4).ToSKColor(),
                    SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                );

                canvas.Scale(0.73f, 0.73f);
            }
            var radius = size.Width * 0.5f; // the term backgroundDiscRadiusScale is there just to ensure the shadow will be visible
            canvas.DrawCircle(0, 0, radius, backgroundPaint);

            // Rotate the canvas
            canvas.RotateDegrees((float)(_rotation * 360.0f));

            // Draw the progress arc
            var progressArcBoundingRect = new SKRect(
                -size.Width * _progressArcDiameterProportion * 0.5f,
                -size.Height * _progressArcDiameterProportion * 0.5f,
                size.Width * _progressArcDiameterProportion * 0.5f,
                size.Height * _progressArcDiameterProportion * 0.5f);
            progressPaint.StrokeWidth = size.Width * 0.08f;

            if (IsGradient)
            {
                var tmpIndex = _currentColorIndex + 1;
                if (tmpIndex >= ProgressColors.Count)
                {
                    tmpIndex = 0;
                }
                progressPaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(progressArcBoundingRect.Left, progressArcBoundingRect.Top),
                    new SKPoint(progressArcBoundingRect.Right, progressArcBoundingRect.Bottom),
                    new SKColor[]
                    {
                        ProgressColors[_currentColorIndex].ToSKColor(),
                        ProgressColors[tmpIndex].ToSKColor(),
                    },
                    new float[]
                    {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );
            }
            else
            {
                progressPaint.Color = _currentColor;
            }

            using (var arcPath = new SKPath())
            {
                float startAngle = 0.0f;
                float sweepAngle = 0.0f;
                if (_progress <= 0.5f)
                {
                    sweepAngle = (float)(2 * _progress * 360.0f);
                    startAngle = -90.0f;
                }

                if (_progress > 0.5d)
                {
                    startAngle = ((float)_progress - 0.5f) // shift the range to (0, 0.5]
                        * 2 // map the range to (0, 1.0]
                        * 360.0f // map the range to (0, 360]
                        - 90.0f; // shift the range to (-90.0, 270]
                    sweepAngle = 270.0f - startAngle;
                }

                arcPath.AddArc(progressArcBoundingRect, startAngle, sweepAngle);
                canvas.DrawPath(arcPath, progressPaint);
            }
        }
    }
}
