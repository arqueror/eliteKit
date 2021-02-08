using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteButton : SKCanvasView
    {
        private SKRoundRect roundRectButton;
        private SKPaint paintButton;

        public static readonly BindableProperty TextAlignProperty = BindableProperty.Create(nameof(TextAlign), typeof(SKTextAlign), typeof(eliteButton), SKTextAlign.Center, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

        });
        public SKTextAlign TextAlign
        {
            get
            {
                return (SKTextAlign)GetValue(TextAlignProperty);
            }
            set
            {
                SetValue(TextAlignProperty, value);
            }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(eliteButton), 50f, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

        });
        public float FontSize
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

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(eliteButton), coreSettings.DefaultFontFamily, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
            {
                ((eliteButton)bindableObject).InvalidateSurface();
            }

        });
        public string FontFamily
        {
            get
            {

                return (string)GetValue(FontFamilyProperty);
            }
            set
            {
                SetValue(FontFamilyProperty, value);
            }
        }

        SKTypeface TextFont
        {
            get
            {
                try
                {
                    var ff = SKTypeface.FromFamilyName(FontFamily);
                    return ff;
                }
                catch (Exception ex) { }
                return SKTypeface.Default;
            }
        }


        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteButton), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteButton), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorTextProperty = BindableProperty.Create(nameof(ColorText), typeof(Color), typeof(eliteButton), Color.White, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

        });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorText
        {
            get
            {
                return (Color)GetValue(ColorTextProperty);
            }
            set
            {
                SetValue(ColorTextProperty, value);
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteButton), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ButtonTitleProperty = BindableProperty.Create(nameof(ButtonTitle), typeof(string), typeof(eliteButton), "eliteButton", BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteButton)bindableObject).InvalidateSurface();
        });
        /// <summary>
        /// 
        /// </summary>
        public string ButtonTitle
        {
            get
            {
                return (string)GetValue(ButtonTitleProperty);
            }
            set
            {
                SetValue(ButtonTitleProperty, value);
            }
        }

        public eliteButton()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteButtonTouched;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();
            int canvasWidth = eventArgs.Info.Width;
            int canvasHeight = eventArgs.Info.Height;

            this.roundRectButton = new SKRoundRect(new SKRect(0, 0, canvasWidth, canvasHeight - 20), ((canvasHeight - 20) / 4), ((canvasHeight - 20) / 4));
            this.paintButton = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                IsAntialias = true,
                ImageFilter = SKImageFilter.CreateDropShadow(
                    0,
                    8,
                    0,
                    this.buttonShadowDrop,
                    Color.FromRgba(0, 0, 0, this.buttonShadowAlpha).ToSKColor(),
                    SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                )
            };

            if (this.IsGradient)
                paintButton.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(this.roundRectButton.Rect.Left, this.roundRectButton.Rect.Top),
                    new SKPoint(this.roundRectButton.Rect.Right, this.roundRectButton.Rect.Bottom),
                    new SKColor[] {
                        this.ColorPrimary.ToSKColor(),
                        this.ColorSecondary.ToSKColor()
                    },
                    new float[] {
                        this.buttonGradientOffset,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

            givenCanvas.DrawRoundRect(this.roundRectButton, this.paintButton);

            SKPaint paintButtonTitle = new SKPaint()
            {
                TextSize = FontSize,
                IsAntialias = true,
                Color = this.ColorText.ToSKColor(),
                TextAlign = TextAlign,
                Typeface = TextFont
            };
            givenCanvas.DrawText(this.ButtonTitle, canvasWidth / 2, ((canvasHeight - 30) / 2 + 25), paintButtonTitle);
        }

        private bool buttonPressed = false;
        private bool buttonPressedAnimationInProgress = false;
        private int buttonPressedInterval = 0;
        private float buttonShadowDrop = 5;
        private double buttonShadowAlpha = 0.4;
        private float buttonGradientOffset = 0;

        private void eliteButtonTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    this.buttonPressedInterval = 0;
                    this.buttonShadowDrop = 5;
                    this.buttonPressed = true;

                    if (!this.buttonPressedAnimationInProgress)
                    {
                        this.buttonPressedAnimationInProgress = true;

                        Device.StartTimer(TimeSpan.FromMilliseconds(10), () =>
                        {
                            this.buttonPressedInterval += 10;

                            if (!this.buttonPressed)
                            {
                                this.OnButtonClick();

                                this.buttonShadowDrop = 5;
                                this.buttonShadowAlpha = 0.4;
                                this.buttonGradientOffset = 0;

                                this.InvalidateSurface();
                                this.buttonPressedAnimationInProgress = false;
                                return false;
                            }

                            if (this.buttonPressedInterval % 100 == 0)
                            {
                                this.buttonShadowDrop++;
                                this.buttonShadowAlpha += 0.1;
                            }

                            if (this.buttonPressedInterval % 20 == 0)
                                this.buttonGradientOffset += 0.1f;

                            this.InvalidateSurface();

                            if (this.buttonPressedInterval == 200)
                            {
                                if (this.buttonPressed)
                                {
                                    if (this.ButtonLongClick != null && this.ButtonClick != null)
                                        this.OnButtonLongClick();
                                }

                                this.buttonPressedAnimationInProgress = false;
                                return false;
                            }

                            return true;
                        });
                    }
                    break;

                case SKTouchAction.Released:
                    this.buttonPressed = false;

                    if (!this.buttonPressedAnimationInProgress)
                    {
                        if (this.ButtonLongClick == null && this.ButtonClick != null)
                            this.OnButtonClick();

                        this.buttonShadowDrop = 5;
                        this.buttonShadowAlpha = 0.4;
                        this.buttonGradientOffset = 0;
                        this.InvalidateSurface();
                    }
                    break;
            }

            eventArgs.Handled = true;
        }

        private void OnButtonClick()
        {
            this.ButtonClick?.Invoke(this, EventArgs.Empty);
            ButtonClickCommand?.Execute(null);
        }
        public event EventHandler ButtonClick;

        private void OnButtonLongClick()
        {
            this.ButtonLongClick?.Invoke(this, EventArgs.Empty);
            ButtonLongClickCommand?.Execute(null);
        }
        public event EventHandler ButtonLongClick;

        public static readonly BindableProperty ButtonClickCommandProperty = BindableProperty.Create(nameof(ButtonClickCommand), typeof(ICommand), typeof(eliteButton));
        /// <summary>
        ///   
        /// </summary>
        public ICommand ButtonClickCommand
        {
            get => (ICommand)GetValue(ButtonClickCommandProperty);
            set => SetValue(ButtonClickCommandProperty, value);
        }

        public static readonly BindableProperty ButtonLongClickCommandProperty = BindableProperty.Create(nameof(ButtonLongClickCommand), typeof(ICommand), typeof(eliteButton));
        /// <summary>
        ///   
        /// </summary>
        public ICommand ButtonLongClickCommand
        {
            get => (ICommand)GetValue(ButtonLongClickCommandProperty);
            set => SetValue(ButtonLongClickCommandProperty, value);
        }
    }
}