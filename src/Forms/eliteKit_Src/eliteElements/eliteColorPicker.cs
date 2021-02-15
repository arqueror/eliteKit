using System;
using System.Windows.Input;
using eliteKit.eliteCore;
using eliteKit.eliteEventArgs;
using eliteKit.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class eliteColorPicker : SKCanvasView
    {
        #region Private Members
        private SKPoint _touchLocation;
        private SKColor _selectedColor = Color.Transparent.ToSKColor();
        private SKPoint _center;
        private bool _colorChanged;
        private SKColor[] _colors = new SKColor[8];
        private bool _isFirstTime = true;
        public event EventHandler<EventArgsColorChanged> ColorChanged;

        #endregion

        #region Bindable Properties
        public static readonly BindableProperty MarkerColorProperty =
          BindableProperty.Create(nameof(MarkerColor),
          typeof(Color),
          typeof(eliteColorPicker),
          Color.White,
          propertyChanged: (bindable, oldValue, newValue) =>
          {

                  if (newValue != MarkerColorProperty.DefaultValue)
                  {
                      ((eliteColorPicker)bindable).MarkerColor = (Color)MarkerColorProperty.DefaultValue;
                      return;
                  }
              
          });
        public Color MarkerColor
        {
            get
            {
                return (Color)GetValue(MarkerColorProperty);
            }
            set
            {
                SetValue(MarkerColorProperty, value);
            }
        }

        public static readonly BindableProperty HueValueProperty =
        BindableProperty.Create(nameof(HueValue),
            typeof(float),
            typeof(eliteColorPicker),
            360f,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                (bindable as eliteColorPicker).SetColorsRange();
            });
        public float HueValue
        {
            get
            {
                return (float)GetValue(HueValueProperty);
            }
            set
            {
                SetValue(HueValueProperty, value);
            }
        }

        public static readonly BindableProperty SaturationValueProperty =
           BindableProperty.Create(nameof(SaturationValue),
               typeof(float),
               typeof(eliteColorPicker),
               100f,
               propertyChanged: (bindable, oldValue, newValue) => { (bindable as eliteColorPicker).SetColorsRange(); });
        public float SaturationValue
        {
            get
            {
                return (float)GetValue(SaturationValueProperty);
            }
            set
            {
                SetValue(SaturationValueProperty, value);
            }
        }

        public static readonly BindableProperty LightnessValueProperty =
           BindableProperty.Create(nameof(LightnessValue),
               typeof(float),
               typeof(eliteColorPicker),
               50f,
               propertyChanged: (bindable, oldValue, newValue) => { (bindable as eliteColorPicker).SetColorsRange(); });
        public float LightnessValue
        {
            get
            {
                return (float)GetValue(LightnessValueProperty);
            }
            set
            {
                SetValue(LightnessValueProperty, value);
            }
        }

        public static readonly BindableProperty AlphaValueProperty =
        BindableProperty.Create(nameof(AlphaValue),
            typeof(byte),
            typeof(eliteColorPicker),
            (byte)255,
            propertyChanged: (bindable, oldValue, newValue) => { (bindable as eliteColorPicker).SetColorsRange(); });
        public byte AlphaValue
        {
            get
            {
                return (byte)GetValue(AlphaValueProperty);
            }
            set
            {
                SetValue(AlphaValueProperty, value);
            }
        }


        public static readonly BindableProperty MarkerRadiusProperty =
            BindableProperty.Create(nameof(MarkerRadius),
                typeof(int),
                typeof(eliteColorPicker),
                15,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    (bindable as eliteColorPicker).InvalidateSurface();
                });
        private int MarkerRadius
        {
            get
            {
                return (int)GetValue(MarkerRadiusProperty);
            }
            set
            {
                SetValue(MarkerRadiusProperty, value);
            }
        }

        public static readonly BindableProperty RadiusProperty =
          BindableProperty.Create(nameof(Radius),
              typeof(int),
              typeof(eliteColorPicker),
              200,
              propertyChanged: (bindable, oldValue, newValue) =>
              {
                  (bindable as eliteColorPicker)._circlePalette.StrokeWidth = (int)newValue / 2;
                  (bindable as eliteColorPicker).InvalidateSurface();
              });
        private int Radius
        {
            get
            {
                return (int)GetValue(RadiusProperty);
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        public static readonly BindableProperty ColorChangedCommandProperty =
            BindableProperty.Create(nameof(ColorChangedCommand),
                typeof(ICommand),
                typeof(eliteColorPicker));
        public ICommand ColorChangedCommand
        {
            get => (ICommand)GetValue(ColorChangedCommandProperty);
            set => SetValue(ColorChangedCommandProperty, value);
        }
        #endregion

        public Color SelectedColor
        {
            get => _selectedColor.ToFormsColor();
        }

        private SKPaint _touchCircleOutline = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 5,
            IsAntialias = true
        };

        private readonly SKPaint _circlePalette = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true
        };

        private readonly SKPaint _rectanglePalette = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        public eliteColorPicker()
        {
            _circlePalette.StrokeWidth = Radius / 2;
            SetColorsRange();
            _selectedColor = _colors[0];
            EnableTouchEvents = true;
            Touch += OnTouchEffectAction;
        }

        private void SetColorsRange()
        {
            if (AlphaValue > 255)
                AlphaValue = 255;
            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = SKColor.FromHsl(i * HueValue / _colors.Length, SaturationValue, LightnessValue, AlphaValue);
            }
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {

            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            Radius = (int)((WidthRequest ) + (Math.Pow(HeightRequest, 2)) / (8 * WidthRequest)) ;
            MarkerRadius = Radius /15;

            _center = new SKPoint(info.Rect.MidX, info.Rect.MidY);
            _circlePalette.Shader = SKShader.CreateSweepGradient(_center, _colors, null);

            canvas.DrawCircle(_center, Radius - _circlePalette.StrokeWidth, _circlePalette);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = _selectedColor,
                StrokeWidth = 15,
                IsAntialias = true
            };
            canvas.DrawCircle(_center, (Radius - _circlePalette.StrokeWidth) + (_circlePalette.StrokeWidth / 2 + 15), paint);

            if (_touchLocation == SKPoint.Empty)
            {
                _touchLocation = _center;
                _colorChanged = true;
            }

            if (_colorChanged)
            {
                using (var bmp = new SKBitmap(info))
                {
                    IntPtr dstpixels = bmp.GetPixels();

                    var succeed = surface.ReadPixels(info, dstpixels, info.RowBytes, (int)_touchLocation.X, (int)_touchLocation.Y);
                    if (succeed)
                    {
                        _selectedColor = bmp.GetPixel(0, 0);
                        ColorChanged?.Invoke(this, new EventArgsColorChanged(_selectedColor.ToFormsColor()));
                        ColorChangedCommand?.Execute(_selectedColor.ToFormsColor());

                    }
                }
            }

            //If user started moving marker Draw little circle in new position
            if (!_isFirstTime)
            {
                _touchCircleOutline.Color = MarkerColor.ToSKColor();
                canvas.DrawCircle(_touchLocation, MarkerRadius, _touchCircleOutline);
            }
        }

        void OnTouchEffectAction(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs args)
        {
            var skPoint = args.Location;

            //User is not tapping in empty center of ColorPicker?
            if (!skPoint.IsInsideCircle(_center, Radius - _circlePalette.StrokeWidth - (_circlePalette.StrokeWidth / 2)))
            {
                //Is user tapping within valid colored area??
                if (skPoint.IsInsideCircle(_center, (Radius - _circlePalette.StrokeWidth) + (_circlePalette.StrokeWidth / 2)))
                {
                    _touchLocation = skPoint;
                    _isFirstTime = false;
                    if (args.ActionType == SKTouchAction.Entered ||
                         args.ActionType == SKTouchAction.Pressed ||
                         args.ActionType == SKTouchAction.Moved ||
                         args.ActionType == SKTouchAction.Released)
                    {
                        _colorChanged = true;
                        InvalidateSurface();
                        args.Handled = true;
                    }
                }
                else
                {
                    _colorChanged = false;
                }
            }

        }
    }
}
