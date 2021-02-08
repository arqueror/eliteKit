using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteCheckbox : SKCanvasView
    {
        private int canvasWidth = 0;
        private int canvasHeight = 0;
        private Color colorCurrent;
        private int shadowPadding = 10;

        public static readonly BindableProperty ColorCheckedProperty = BindableProperty.Create(nameof(ColorChecked), typeof(Color), typeof(eliteCheckbox), coreSettings.ColorSecondary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorChecked
        {
            get
            {
                return (Color)GetValue(ColorCheckedProperty);
            }
            set
            {
                SetValue(ColorCheckedProperty, value);
            }
        }

        public static readonly BindableProperty ColorUncheckedProperty = BindableProperty.Create(nameof(ColorUnchecked), typeof(Color), typeof(eliteCheckbox), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorUnchecked
        {
            get
            {
                return (Color)GetValue(ColorUncheckedProperty);
            }
            set
            {
                SetValue(ColorUncheckedProperty, value);
            }
        }

        public static readonly BindableProperty ColorHighlightedProperty = BindableProperty.Create(nameof(ColorHighlighted), typeof(Color), typeof(eliteCheckbox), coreSettings.ColorHighlight, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorHighlighted
        {
            get
            {
                return (Color)GetValue(ColorHighlightedProperty);
            }
            set
            {
                SetValue(ColorHighlightedProperty, value);
            }
        }

        public static readonly BindableProperty ColorIconProperty = BindableProperty.Create(nameof(ColorIcon), typeof(Color), typeof(eliteCheckbox), Color.White, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
            });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorIcon
        {
            get
            {
                return (Color)GetValue(ColorIconProperty);
            }
            set
            {
                SetValue(ColorIconProperty, value);
            }
        }

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(eliteCheckbox), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
            });
        /// <summary>
        /// 
        /// </summary>
        public bool HasShadow
        {
            get
            {
                return (bool)GetValue(HasShadowProperty);
            }
            set
            {
                SetValue(HasShadowProperty, value);
            }
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(eliteCheckbox), false, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    var val = (bool)value;
                    ((eliteCheckbox)bindableObject).colorCurrent = val ? ((eliteCheckbox)bindableObject).ColorChecked : ((eliteCheckbox)bindableObject).ColorUnchecked;
                    ((eliteCheckbox)bindableObject).InvalidateSurface();
                }
            });
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public eliteCheckbox()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteCheckboxTouched;
            this.colorCurrent = this.ColorUnchecked;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            if (!this.HasShadow)
                this.shadowPadding = 0;

            // Draw the Checkbox container
            SKRoundRect roundRectCheckbox = new SKRoundRect(new SKRect(this.shadowPadding, 0, this.canvasWidth - this.shadowPadding, this.canvasHeight - (this.shadowPadding * 2)), ((this.canvasHeight - (this.shadowPadding * 2)) / 6), ((this.canvasHeight - (this.shadowPadding * 2)) / 6));
            SKPaint paintCheckbox = new SKPaint()
            {
                Color = this.colorCurrent.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            if (this.HasShadow)
                paintCheckbox.ImageFilter = SKImageFilter.CreateDropShadow(
                    0,
                    8,
                    0,
                    5,
                    Color.FromRgba(0, 0, 0, 0.4).ToSKColor(),
                    SKDropShadowImageFilterShadowMode.DrawShadowAndForeground
                );

            givenCanvas.DrawRoundRect(roundRectCheckbox, paintCheckbox);

            if (this.IsChecked)
            {
                SKPaint paintIcon = new SKPaint()
                {
                    TextSize = 48f,
                    IsAntialias = true,
                    Color = this.ColorIcon.ToSKColor(),
                    TextAlign = SKTextAlign.Center,
                    Typeface = this.GetTypeface("fa-solid-900.ttf")
                };
                SKRect rectIcon = new SKRect();
                paintIcon.MeasureText("\uf00d", ref rectIcon);

                givenCanvas.DrawText("\uf00d", this.canvasWidth / 2, this.canvasHeight / 2 - rectIcon.MidY - this.shadowPadding, paintIcon);
            }
        }

        private void eliteCheckboxTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    {
                        this.colorCurrent = this.ColorHighlighted;
                        this.InvalidateSurface();
                    }
                    break;

                case SKTouchAction.Released:
                    {
                        this.IsChecked = !this.IsChecked;
                        this.colorCurrent = this.IsChecked ? this.ColorChecked : this.ColorUnchecked;

                        if (this.IsChecked)
                            this.OnChecked();
                        else
                            this.OnUnchecked();

                        this.OnToggled();

                        this.InvalidateSurface();
                    }
                    break;
            }

            eventArgs.Handled = true;
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

        public static readonly BindableProperty CheckedCommandProperty = BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(eliteCheckbox));
        /// <summary>
        ///   
        /// </summary>
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            set => SetValue(CheckedCommandProperty, value);
        }

        private void OnChecked()
        {
            this.Checked?.Invoke(this, EventArgs.Empty);
            CheckedCommand?.Execute(true);
        }
        public event EventHandler Checked;

        private void OnUnchecked()
        {
            this.Unchecked?.Invoke(this, EventArgs.Empty);
            CheckedCommand?.Execute(false);
        }
        public event EventHandler Unchecked;

        private void OnToggled() => this.Toggled?.Invoke(this, EventArgs.Empty);
        public event EventHandler Toggled;
    }
}
