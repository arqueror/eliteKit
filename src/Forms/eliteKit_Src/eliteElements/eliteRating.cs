using eliteKit.eliteCore;
using eliteKit.eliteEventArgs;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Reflection;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteRating : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;

        private float ratingWidth = 0;

        public static readonly BindableProperty RatingCurrentProperty = BindableProperty.Create(nameof(RatingCurrent), typeof(float), typeof(eliteRating), 1f, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteRating)bindableObject).InvalidateSurface();

                }
            });
        public float RatingCurrent
        {
            get
            {
                return (float)GetValue(RatingCurrentProperty);
            }
            set
            {
                SetValue(RatingCurrentProperty, value);
            }
        }

        public static readonly BindableProperty RatingMaxProperty = BindableProperty.Create(nameof(RatingMax), typeof(float), typeof(eliteRating), 5f, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteRating)bindableObject).InvalidateSurface();

                }
            });
        public float RatingMax
        {
            get
            {
                return (float)GetValue(RatingMaxProperty);
            }
            set
            {
                SetValue(RatingMaxProperty, value);
            }
        }

        public static readonly BindableProperty RatingIconProperty = BindableProperty.Create(nameof(RatingIcon), typeof(string), typeof(eliteRating), "\uf005", propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteRating)bindableObject).InvalidateSurface();

                }
            });
        public string RatingIcon
        {
            get
            {
                return (string)GetValue(RatingIconProperty);
            }
            set
            {
                SetValue(RatingIconProperty, value);
            }
        }

        public static readonly BindableProperty ColorRatingSelectedProperty = BindableProperty.Create(nameof(ColorRatingSelected), typeof(Color), typeof(eliteRating), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteRating)bindableObject).InvalidateSurface();

                    }
                
            });
        public Color ColorRatingSelected
        {
            get
            {
                return (Color)GetValue(ColorRatingSelectedProperty);
            }
            set
            {
                SetValue(ColorRatingSelectedProperty, value);
            }
        }

        public static readonly BindableProperty ColorRatingUnselectedProperty = BindableProperty.Create(nameof(ColorRatingUnselected), typeof(Color), typeof(eliteRating), coreSettings.ColorSecondary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteRating)bindableObject).InvalidateSurface();

                    }
                
            });
        public Color ColorRatingUnselected
        {
            get
            {
                return (Color)GetValue(ColorRatingUnselectedProperty);
            }
            set
            {
                SetValue(ColorRatingUnselectedProperty, value);
            }
        }

        private void OnRatingChanged() => this.RatingChanged?.Invoke(this, new EventArgsRatingChanged()
        {
            ratingValue = this.RatingCurrent
        });
        public event EventHandler RatingChanged;

        public eliteRating()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteRatingTouched;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            this.ratingWidth = this.canvasWidth / this.RatingMax;
            float ratingHeight = this.ratingWidth;

            if (ratingHeight > this.canvasHeight)
                ratingHeight = this.canvasHeight;

            SKPaint paintRatingActive = new SKPaint()
            {
                TextSize = this.ratingWidth / 2f,
                IsAntialias = true,
                Color = this.ColorRatingSelected.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf")
            };

            SKPaint paintRatingInactive = new SKPaint()
            {
                TextSize = this.ratingWidth / 2f,
                IsAntialias = true,
                Color = this.ColorRatingUnselected.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-regular-400.ttf")
            };

            for (float i = 0; i < this.RatingMax; i++)
            {
                SKRect rectRating = new SKRect((i * this.ratingWidth), (this.canvasHeight - this.ratingWidth) / 2f, ((i * this.ratingWidth) + this.ratingWidth), this.ratingWidth + (this.canvasHeight - this.ratingWidth) / 2f);
                SKPaint paintRating = new SKPaint()
                {
                    IsAntialias = true
                };

                SKRect rectRatingSymbol = new SKRect();
                paintRatingActive.MeasureText(this.RatingIcon, ref rectRatingSymbol);
                givenCanvas.DrawText(this.RatingIcon, rectRating.Left + (rectRating.Width / 2f), rectRating.Top + (rectRating.Height / 2f) - rectRatingSymbol.MidY, this.RatingCurrent > i ? paintRatingActive : paintRatingInactive);
            }
        }

        private void eliteRatingTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    {
                        float positionX = eventArgs.Location.X;

                        for (float i = 0; i < this.RatingMax; i++)
                        {
                            if (positionX >= (i * this.ratingWidth)
                                && positionX <= ((i * this.ratingWidth) + this.ratingWidth))
                            {
                                this.RatingCurrent = i + 1;
                                this.OnRatingChanged();
                                this.InvalidateSurface();
                            }
                        }
                    }
                    break;
            }
        }

        public SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("eliteKit.eliteFonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }
    }
}
