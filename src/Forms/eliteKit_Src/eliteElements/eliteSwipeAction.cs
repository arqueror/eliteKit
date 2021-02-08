using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class eliteSwipeAction : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private bool swipeButtonDragging = false;
        private bool swipeButtonDefined = false;
        private float swipeButtonX = 0;
        private float swipeButtonWidth = 0;
        private SKRoundRect roundRectButton;

        public event EventHandler SwipeCompleted;
        private void OnSwipeCompleted()
        {
            this.SwipeCompleted?.Invoke(this, EventArgs.Empty);
        }

        // Swipe element background color
        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteSwipeAction), Color.FromRgba(255, 255, 255, 125), propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteSwipeAction)bindableObject).InvalidateSurface();
            }
        });
        public Color ColorBackground
        {
            get => (Color)GetValue(ColorBackgroundProperty);
            set => SetValue(ColorBackgroundProperty, value);
        }

        // Swipe button background color
        public static readonly BindableProperty ColorButtonProperty = BindableProperty.Create(nameof(ColorButton), typeof(Color), typeof(eliteSwipeAction), Color.White, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteSwipeAction)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorButton
        {
            get => (Color)GetValue(ColorButtonProperty);
            set => SetValue(ColorButtonProperty, value);
        }

        // Swipe element overlay (when swiping from left)
        public static readonly BindableProperty ColorOverlayProperty = BindableProperty.Create(nameof(ColorOverlay), typeof(Color), typeof(eliteSwipeAction), Color.FromRgba(0, 0, 0, 50), propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteSwipeAction)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorOverlay
        {
            get => (Color)GetValue(ColorOverlayProperty);
            set => SetValue(ColorOverlayProperty, value);
        }

        // Swipe text color (Swipe to unlock)
        public static readonly BindableProperty ColorSwipeTextProperty = BindableProperty.Create(nameof(ColorSwipeText), typeof(Color), typeof(eliteSwipeAction), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteSwipeAction)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorSwipeText
        {
            get => (Color)GetValue(ColorSwipeTextProperty);
            set => SetValue(ColorSwipeTextProperty, value);
        }

        // Swipe text (Swipe to unlock)
        public static readonly BindableProperty ButtonSwipeTextProperty = BindableProperty.Create(nameof(ButtonSwipeText), typeof(string), typeof(eliteSwipeAction), "Swipe to unlock", propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteSwipeAction)bindableObject).InvalidateSurface();
            }
        });
        public string ButtonSwipeText
        {
            get => (string)GetValue(ButtonSwipeTextProperty);
            set => SetValue(ButtonSwipeTextProperty, value);
        }

        // Swipe button icon color
        public static readonly BindableProperty ColorButtonIconProperty = BindableProperty.Create(nameof(ColorButtonIcon), typeof(Color), typeof(eliteSwipeAction), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteSwipeAction)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorButtonIcon
        {
            get => (Color)GetValue(ColorButtonIconProperty);
            set => SetValue(ColorButtonIconProperty, value);
        }

        // Swipe button icon
        public static readonly BindableProperty ButtonIconProperty = BindableProperty.Create(nameof(ButtonIcon), typeof(string), typeof(eliteSwipeAction), "\uf054", propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteSwipeAction)bindableObject).InvalidateSurface();
            }
        });
        public string ButtonIcon
        {
            get => (string)GetValue(ButtonIconProperty);
            set => SetValue(ButtonIconProperty, value);
        }

        public eliteSwipeAction()
        {
            this.EnableTouchEvents = true;
            this.Touch += eliteSwipeActionTouched;
        }

        private void eliteSwipeActionTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            SKPoint pointCurrent = eventArgs.Location;

            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    {
                        if (pointCurrent.X > this.roundRectButton.Rect.Left
                            && pointCurrent.X < (this.roundRectButton.Rect.Left + this.roundRectButton.Rect.Width))
                            this.swipeButtonDragging = true;
                    } break;

                case SKTouchAction.Moved:
                    {
                        if(this.swipeButtonDragging)
                        {
                            if (pointCurrent.X > 0
                                && pointCurrent.X < (this.canvasWidth - this.swipeButtonWidth))
                                this.swipeButtonX = pointCurrent.X;
                        }
                    } break;

                case SKTouchAction.Released:
                    {
                        if(this.swipeButtonDragging)
                            if (pointCurrent.X >= this.canvasWidth - (this.swipeButtonWidth / 2))
                                this.OnSwipeCompleted();

                        this.swipeButtonDragging = false;

                        Device.StartTimer(TimeSpan.FromMilliseconds(5), () =>
                        {
                            if (this.swipeButtonDragging)
                                return false;

                            if (this.swipeButtonX > 0)
                            {
                                if ((this.swipeButtonX - 20) < 0)
                                    this.swipeButtonX = 0;
                                else
                                    this.swipeButtonX -= 20;
                            }
                            else
                                return false;

                            this.InvalidateSurface();
                            return true;
                        });
                    } break;
            }

            eventArgs.Handled = true;
            this.InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            int borderRadius = this.canvasHeight / 2;
            this.swipeButtonWidth = this.canvasHeight;

            if (!this.swipeButtonDefined)
            {
                this.swipeButtonDefined = true;
                this.swipeButtonX = 0;
            }

            SKPaint paintBackground = new SKPaint()
            {
                Color = this.ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            SKPaint paintOverlay = new SKPaint()
            {
                Color = this.ColorOverlay.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            SKPaint paintButton = new SKPaint()
            {
                Color = this.ColorButton.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            SKRoundRect roundRectBackground = new SKRoundRect(
                new SKRect(
                    0,
                    0,
                    this.canvasWidth,
                    this.canvasHeight
                ),
                borderRadius,
                borderRadius
            );

            SKRoundRect roundRectOverlay = new SKRoundRect(
                new SKRect(
                    0,
                    0,
                    this.swipeButtonX + this.canvasHeight,
                    this.canvasHeight
                ),
                borderRadius,
                borderRadius
            );

            this.roundRectButton = new SKRoundRect(
                new SKRect(
                    this.swipeButtonX,
                    0,
                    this.swipeButtonX + this.canvasHeight,
                    this.canvasHeight
                ),
                borderRadius,
                borderRadius
            );

            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);

            SKPaint paintSwipeTitle = new SKPaint()
            {
                TextSize = 54f,
                IsAntialias = true,
                Color = this.ColorSwipeText.ToSKColor(),
                TextAlign = SKTextAlign.Center
            };

            SKRect swipeTitleRect = new SKRect();
            paintSwipeTitle.MeasureText(this.ButtonSwipeText, ref swipeTitleRect);

            givenCanvas.DrawText(
                this.ButtonSwipeText,
                roundRectBackground.Rect.Left + roundRectBackground.Rect.Width / 2,
                roundRectBackground.Rect.Top + roundRectBackground.Rect.Height / 2 - swipeTitleRect.MidY,
                paintSwipeTitle
            );

            givenCanvas.DrawRoundRect(roundRectOverlay, paintOverlay);
            givenCanvas.DrawRoundRect(roundRectButton, paintButton);

            SKPaint paintButtonIcon = new SKPaint()
            {
                TextSize = 54f,
                IsAntialias = true,
                Color = this.ColorButtonIcon.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf")
            };

            SKRect buttonIconRect = new SKRect();
            paintButtonIcon.MeasureText(this.ButtonIcon, ref buttonIconRect);

            givenCanvas.DrawText(
                this.ButtonIcon,
                roundRectButton.Rect.Left + roundRectButton.Rect.Width / 2,
                roundRectButton.Rect.Top + roundRectButton.Rect.Height / 2 - buttonIconRect.MidY,
                paintButtonIcon
            );
        }

        SKTypeface GetTypeface(string fullFontName)
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
