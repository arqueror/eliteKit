using eliteKit.eliteCore;
using eliteKit.Extensions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public enum BorderStyle
    {
        None,
        Oval,
        Rectangular,
        RoundedRectangular
    }

    public abstract class BorderBase : ElementBase
    {
        SKPaint _borderPaint;
        SKPaint _borderBackgroundPaint;
        readonly float _shadowSigmaY = 5;
        readonly double _shadowAlpha = 0.4;
        readonly float _gradientOffset = 0;
        protected readonly float _shadowDy = 8;

        // Border Color
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(BorderBase), Color.Black, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {

                        var borderBase = (BorderBase)bindableObject;
                        borderBase.CreateBorderPaint();
                        borderBase.Redraw();
                    
                });

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        // Background Color Primary
        public static readonly BindableProperty BackgroundColorPrimaryProperty =
            BindableProperty.Create(nameof(BackgroundColorPrimary), typeof(Color), typeof(eliteButton), coreSettings.ColorPrimary,
                BindingMode.OneWay, propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var borderBase = (BorderBase)bindableObject;
                        borderBase.ResetBorderBackgroundPaint();
                        borderBase.Redraw();
                    
                });

        public Color BackgroundColorPrimary
        {
            get => (Color)GetValue(BackgroundColorPrimaryProperty);
            set => SetValue(BackgroundColorPrimaryProperty, value);
        }

        // Background Color Secondary
        public static readonly BindableProperty BackgroundColorSecondaryProperty =
            BindableProperty.Create(nameof(BackgroundColorSecondary), typeof(Color), typeof(eliteButton), coreSettings.ColorSecondary,
                BindingMode.OneWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

                var borderBase = (BorderBase)bindableObject;
                borderBase.ResetBorderBackgroundPaint();
                borderBase.Redraw();
            
        });

        public Color BackgroundColorSecondary
        {
            get => (Color)GetValue(BackgroundColorSecondaryProperty);
            set => SetValue(BackgroundColorSecondaryProperty, value);
        }

        // Is Gradient
        public static readonly BindableProperty IsGradientProperty =
            BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteButton), false,
                BindingMode.OneWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                var borderBase = (BorderBase)bindableObject;
                borderBase.ResetBorderBackgroundPaint();
                borderBase.Redraw();
            
        });

        public bool IsGradient
        {
            get => (bool)GetValue(IsGradientProperty);
            set => SetValue(IsGradientProperty, value);
        }

        // Border Width
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(BorderBase), 0f, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var borderBase = (BorderBase)bindableObject;
                        borderBase.CreateBorderPaint();
                        borderBase.Redraw();
                    
                });

        public float BorderWidth
        {
            get => (float)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        // Border Style
        public static readonly BindableProperty BorderStyleProperty =
            BindableProperty.Create(nameof(BorderStyle), typeof(BorderStyle), typeof(BorderBase), BorderStyle.None, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var borderBase = (BorderBase)bindableObject;
                        borderBase.Redraw();
                    
                });

        public BorderStyle BorderStyle
        {
            get => (BorderStyle)GetValue(BorderStyleProperty);
            set => SetValue(BorderStyleProperty, value);
        }

        // Border Corner Radius
        public static readonly BindableProperty BorderCornerRadiusProperty =
            BindableProperty.Create(nameof(BorderCornerRadius), typeof(float), typeof(BorderBase), 0f, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var borderBase = (BorderBase)bindableObject;
                        borderBase.Redraw();
                    
                });

        public float BorderCornerRadius
        {
            get => (float)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        // Padding
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(BorderBase), null, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var borderBase = (BorderBase)bindableObject;
                        borderBase.Redraw();
                    
                });

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public BorderBase()
        {
            CreateBorderPaint();
        }

        void CreateBorderPaint()
        {
            _borderPaint = new SKPaint
            {
                Color = BorderColor.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = BorderWidth,
                IsAntialias = true
            };
        }

        void ResetBorderBackgroundPaint()
        {
            _borderBackgroundPaint = null;
        }

        void CreateBorderBackgroundPaint(SKRect bounds)
        {
            _borderBackgroundPaint = new SKPaint()
            {
                Color = BackgroundColorPrimary.ToSKColor(),
                IsAntialias = true,
                ImageFilter = SKImageFilter.CreateDropShadow(
                    0,
                    _shadowDy,
                    0,
                    _shadowSigmaY,
                    Color.FromRgba(0, 0, 0, _shadowAlpha).ToSKColor(),
                    SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                )
            };

            if (IsGradient)
                _borderBackgroundPaint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(bounds.Left, bounds.Top),
                    new SKPoint(bounds.Right, bounds.Bottom),
                    new SKColor[] {
                        BackgroundColorPrimary.ToSKColor(),
                        BackgroundColorSecondary.ToSKColor()
                    },
                    new float[] {
                        _gradientOffset,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

        }

        protected override void Draw(SKCanvas canvas, SKRect bounds)
        {
            if(_borderPaint == null)
            {
                return;
            }            

            if (BorderStyle != BorderStyle.None && BorderWidth > 0)
            {
                var widthRadius = BorderWidth / 2.0f;

                // Adjust for border width
                var adjustedBounds = new SKRect(bounds.Left + widthRadius,
                    bounds.Top + widthRadius,
                    bounds.Right - widthRadius,
                    bounds.Bottom - widthRadius);

                if (_borderBackgroundPaint == null)
                {
                    CreateBorderBackgroundPaint(adjustedBounds);
                }

                if (_borderBackgroundPaint == null)
                {
                    return;
                }

                if (BackgroundColorPrimary != Color.Transparent)
                {
                    switch(BorderStyle)
                    {
                        case BorderStyle.Oval:
                            canvas.DrawOval(adjustedBounds, _borderBackgroundPaint);
                            break;
                        case BorderStyle.Rectangular:
                            canvas.DrawRect(adjustedBounds, _borderBackgroundPaint);
                            break;
                        case BorderStyle.RoundedRectangular:
                            canvas.DrawRoundRect(adjustedBounds,
                                new SKSize(BorderCornerRadius, BorderCornerRadius),
                                _borderBackgroundPaint);
                            break;
                    }
                }
                
                switch (BorderStyle)
                {
                    case BorderStyle.Oval:
                        canvas.DrawOval(adjustedBounds, _borderPaint);
                        break;
                    case BorderStyle.Rectangular:
                        canvas.DrawRect(adjustedBounds, _borderPaint);
                        break;
                    case BorderStyle.RoundedRectangular:
                        canvas.DrawRoundRect(adjustedBounds,
                            new SKSize(BorderCornerRadius, BorderCornerRadius),
                            _borderPaint);
                        break;
                }                
            }
        }
    }
}
