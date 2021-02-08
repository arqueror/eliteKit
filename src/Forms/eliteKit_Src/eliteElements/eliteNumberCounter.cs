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
    public class eliteNumberCounter : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(eliteNumberCounter), 48f, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteNumberCounter), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteNumberCounter)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteNumberCounter), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteNumberCounter)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteNumberCounter), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteNumberCounter)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty IconMinusProperty = BindableProperty.Create(nameof(IconMinus), typeof(string), typeof(eliteNumberCounter), "\uf056", BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public string IconMinus
        {
            get
            {
                return (string)GetValue(IconMinusProperty);
            }
            set
            {
                SetValue(IconMinusProperty, value);
            }
        }

        public static readonly BindableProperty IconPlusProperty = BindableProperty.Create(nameof(IconPlus), typeof(string), typeof(eliteNumberCounter), "\uf055", BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public string IconPlus
        {
            get
            {
                return (string)GetValue(IconPlusProperty);
            }
            set
            {
                SetValue(IconPlusProperty, value);
            }
        }

        public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(int), typeof(eliteNumberCounter), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public int MinValue
        {
            get
            {
                return (int)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(eliteNumberCounter), 1000, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public int MaxValue
        {
            get
            {
                return (int)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(eliteNumberCounter), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteNumberCounter)bindableObject).InvalidateSurface();

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

        public eliteNumberCounter()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteNumberCounterTouched;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;
            int borderRadius = this.canvasHeight / 2;
            int buttonRadius = this.canvasHeight / 2;

            Assembly givenAssembly = GetType().GetTypeInfo().Assembly;

            SKRoundRect roundRectBorder = new SKRoundRect(new SKRect(5, 5, this.canvasWidth - 5, this.canvasHeight - 5), borderRadius, borderRadius);
            SKPaint paintBorder = new SKPaint()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 8,
                Color = this.ColorPrimary.ToSKColor()
            };

            if (this.IsGradient)
                paintBorder.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(roundRectBorder.Rect.Left, roundRectBorder.Rect.Top),
                    new SKPoint(roundRectBorder.Rect.Right, roundRectBorder.Rect.Bottom),
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

            givenCanvas.DrawRoundRect(roundRectBorder, paintBorder);

            SKPaint paintMinus = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor()
            };

            if (this.IsGradient)
                paintMinus.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(0, 0),
                    new SKPoint(this.canvasHeight, this.canvasHeight),
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

            SKPath pathMinus = new SKPath();
            pathMinus.MoveTo(15, 15);
            pathMinus.AddCircle(this.canvasHeight / 2, this.canvasHeight / 2, this.canvasHeight / 2 - 15);
            givenCanvas.DrawPath(pathMinus, paintMinus);

            SKPaint paintPlus = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor()
            };

            if (this.IsGradient)
                paintPlus.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(this.canvasWidth - this.canvasHeight, 0),
                    new SKPoint(this.canvasWidth, this.canvasHeight),
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

            SKPath pathPlus = new SKPath();
            pathPlus.MoveTo(this.canvasWidth - 15, 15);
            pathPlus.AddCircle(this.canvasWidth - (this.canvasHeight / 2), this.canvasHeight / 2, this.canvasHeight / 2 - 15);
            givenCanvas.DrawPath(pathPlus, paintPlus);

            SKPaint paintValueTitle = new SKPaint()
            {
                TextSize = 50f,
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                FakeBoldText = true
            };
            givenCanvas.DrawText(this.CurrentValue.ToString(), this.canvasWidth / 2, (this.canvasHeight / 2 + 20), paintValueTitle);

            SKPaint paintIcon = new SKPaint()
            {
                TextSize = 48f,
                IsAntialias = true,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf") //TODO Add custom font support
            };

            SKRect rectIconMinus = new SKRect();
            paintIcon.MeasureText(this.IconMinus, ref rectIconMinus);

            SKRect rectIconPlus = new SKRect();
            paintIcon.MeasureText(this.IconPlus, ref rectIconPlus);

            givenCanvas.DrawText(
                this.IconMinus,
                this.canvasHeight / 2,
                this.canvasHeight / 2 - rectIconMinus.MidY,
                paintIcon
            );

            givenCanvas.DrawText(
                this.IconPlus,
                this.canvasWidth - (this.canvasHeight / 2),
                this.canvasHeight / 2 - rectIconPlus.MidY,
                paintIcon
            );
        }

        private void eliteNumberCounterTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        float positionX = eventArgs.Location.X;
                        float positionY = eventArgs.Location.Y;

                        if (positionX >= 15
                            && positionX <= this.canvasHeight - 15
                            && positionY >= 15
                            && positionY <= this.canvasHeight - 15)
                        {
                            if (this.CurrentValue > this.MinValue)
                            {
                                this.CurrentValue--;
                                this.OnValueDecreased();
                                this.OnValueChanged();
                                this.InvalidateSurface();
                            }
                        }

                        if (positionX >= this.canvasWidth - (this.canvasHeight - 15)
                           && positionX <= this.canvasWidth - 15
                           && positionY >= 15
                           && positionY <= this.canvasHeight - 15)
                        {
                            if (this.CurrentValue < this.MaxValue)
                            {
                                this.CurrentValue++;
                                this.OnValueIncreased();
                                this.OnValueChanged();
                                this.InvalidateSurface();
                            }
                        }
                    }
                    break;
            }

            eventArgs.Handled = true;
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

        private void OnValueChanged()
        {
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
            ValueChangedCommand?.Execute(CurrentValue);
        }
        public event EventHandler ValueChanged;

        private void OnValueIncreased() => this.ValueIncreased?.Invoke(this, EventArgs.Empty);
        public event EventHandler ValueIncreased;

        private void OnValueDecreased() => this.ValueDecreased?.Invoke(this, EventArgs.Empty);
        public event EventHandler ValueDecreased;

        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(nameof(ValueChangedCommand), typeof(ICommand), typeof(eliteNumberCounter));
        /// <summary>
        ///   
        /// </summary>
        public ICommand ValueChangedCommand
        {
            get => (ICommand)GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }
    }
}