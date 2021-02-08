using System.Linq;
using eliteKit.eliteElements.BaseElements;
using SkiaSharp;
using SkiaSharp.Extended.Iconify;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using eliteKit.eliteEnums;
#pragma warning disable ALL
namespace eliteKit.eliteElements
{

    public class eliteIcon : BorderBase
    {
        SKBitmap _bitmap;
        SKPaint _iconPaint;
        SKPaint _faPaint;
        SkiaSharp.Extended.Svg.SKSvg _svg;
        SKTextRun[] _textRuns;

        public static readonly BindableProperty IconColorProperty =
            BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(eliteIcon), Color.Black, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {

                        var icon = (eliteIcon)bindableObject;
                        icon.CreateIconPaint();
                        icon.ResetFaPaint();
                        icon.Redraw();
                    
                });

        public Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create(nameof(IconSource), typeof(IconSource), typeof(eliteIcon), null, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                    if (value is IconSource source)
                    {
                        var icon = (eliteIcon)bindableObject;
                        icon.CreateSource();
                        icon.CreateIconPaint();
                        icon.ResetFaPaint();
                        icon.Redraw();
                    }
                });

        public IconSource IconSource
        {
            get => (IconSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }

        public static readonly BindableProperty AspectProperty =
            BindableProperty.Create(nameof(Aspect), typeof(Aspect), typeof(eliteIcon), Aspect.AspectFit, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var icon = (eliteIcon)bindableObject;
                        icon.Redraw();
                    
                });

        public Aspect Aspect
        {
            get => (Aspect)GetValue(AspectProperty);
            set => SetValue(AspectProperty, value);
        }

        public eliteIcon()
        {
            CreateIconPaint();
        }

        void CreateSource()
        {
            switch (IconSource.IconType)
            {
                case IconType.Svg:
                    _svg = new SkiaSharp.Extended.Svg.SKSvg();
                    _svg.Load(IconSource.Stream);
                    break;
                case IconType.Bitmap:
                    _bitmap = SKBitmap.Decode(IconSource.Stream);
                    break;
                case IconType.Fa:
                    SKTextRunLookup.Instance.AddFontAwesome();
                    _textRuns = new SKTextRun[] { new SKTextRun(IconSource.GlyphValue)
                    {
                        Typeface = SKTextRunLookup.Instance.Typefaces.FirstOrDefault(f => f.FamilyName == "FontAwesome"),
                    }};
                    break;
            }
        }

        void CreateIconPaint()
        {
            if (IconSource == null || IconSource.IconType == IconType.None)
            {
                return;
            }

            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/effects/color-filters

            // RGBA
            var r = (float)IconColor.R * 255f;
            var g = (float)IconColor.G * 255f;
            var b = (float)IconColor.B * 255f;
            var a = (float)IconColor.A;

            _iconPaint = new SKPaint
            {
                ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                {
                    0, 0, 0, 0, r,
                    0, 0, 0, 0, g,
                    0, 0, 0, 0, b,
                    0, 0, 0, a, 0,
                }),
                IsAntialias = true
            };
        }

        void ResetFaPaint()
        {
            _faPaint = null;
        }

        void CreateFaPaint(float textSize)
        {
            _faPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = IconColor.ToSKColor(),
                TextSize = textSize,
                TextAlign = SKTextAlign.Center
            };
        }

        protected override void Draw(SKCanvas canvas, SKRect canvasBounds)
        {
            if (IconSource.IconType == IconType.None)
            {
                return;
            }

            // Adjust bounds for drop shadow
            SKRect bounds = new SKRect(
                canvasBounds.Left,
                canvasBounds.Top,
                canvasBounds.Right,
                canvasBounds.Bottom - _shadowDy);

            switch (IconSource.IconType)
            {
                case IconType.Svg:
                    bounds = GetBounds(bounds, _svg.ViewBox.Width, _svg.ViewBox.Height);
                    break;
                case IconType.Bitmap:
                    bounds = GetBounds(bounds, _bitmap.Width, _bitmap.Height);
                    break;
                case IconType.Fa:
                    bounds = GetBounds(bounds, 1, 1);
                    break;
            }

            base.Draw(canvas, bounds);

            DrawIcon(canvas, bounds);
        }

        SKRect GetBounds(SKRect canvasBounds, float imageWidth, float imageHeight)
        {
            var bounds = canvasBounds;

            var iconAspect = imageWidth / imageHeight;
            var canvasAspect = canvasBounds.Width / canvasBounds.Height;

            if (Aspect == Aspect.AspectFit)
            {
                // Fill horizontal
                if (iconAspect > canvasAspect)
                {
                    var scale = imageHeight / imageWidth;
                    var height = scale * canvasBounds.Width;
                    var top = (canvasBounds.Height - height) / 2.0f;

                    bounds = new SKRect(0, top, canvasBounds.Width, top + height);
                }
                // Fill vertical
                else
                {
                    var width = iconAspect * canvasBounds.Height;
                    var left = (canvasBounds.Width - width) / 2.0f;

                    bounds = new SKRect(left, 0, left + width, canvasBounds.Height);
                }
            }
            else if (Aspect == Aspect.AspectFill)
            {
                // Fill vertical
                if (iconAspect > canvasAspect)
                {
                    float width = iconAspect * canvasBounds.Height;
                    float left = (canvasBounds.Width - width) / 2.0f;

                    bounds = new SKRect(left, 0, left + width, canvasBounds.Height);
                }
                // Stretch horizontal
                else
                {
                    var scale = imageHeight / imageWidth;
                    float height = scale * canvasBounds.Width;
                    float top = (canvasBounds.Height - height) / 2.0f;

                    bounds = new SKRect(0, top, canvasBounds.Width, top + height);
                }
            }

            return bounds;
        }

        void DrawIcon(SKCanvas canvas, SKRect bounds)
        {
            var innerBounds = bounds;

            if (Padding != null)
            {
                innerBounds = new SKRect(
                    bounds.Left + (float)Padding.Left,
                    bounds.Top + (float)Padding.Top,
                    bounds.Right - (float)Padding.Right,
                    bounds.Bottom - (float)Padding.Bottom);
            }

            switch (IconSource.IconType)
            {
                case IconType.Svg:
                    {
                        if (_iconPaint == null)
                        {
                            return;
                        }
                        var scale = new SKPoint(innerBounds.Width / _svg.ViewBox.Width,
                        innerBounds.Height / _svg.ViewBox.Height);
                        canvas.Translate(innerBounds.Location);
                        canvas.Scale(scale);
                        canvas.DrawPicture(_svg.Picture, _iconPaint);
                        canvas.Restore();
                    }
                    break;
                case IconType.Bitmap:
                    if (_iconPaint == null)
                    {
                        return;
                    }
                    canvas.DrawBitmap(_bitmap, innerBounds, _iconPaint);
                    break;
                case IconType.Fa:
                    if (_faPaint == null)
                    {
                        CreateFaPaint(innerBounds.Height);
                    }
                    var textBounds = new SKRect();
                    _faPaint.MeasureText("W", ref textBounds);
                    var y = bounds.Height / 2 - textBounds.MidY;
                    _faPaint.TextScaleX = innerBounds.Width / textBounds.Width;
                    canvas.DrawText(_textRuns, innerBounds.MidX, y, _faPaint);
                    break;
            }
        }
    }
}
