using eliteKit.eliteCore;
using eliteKit.eliteEventArgs;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteSlider : SKCanvasView
    {
        private SKPoint sliderThumbPoint;
        private SKPoint sliderThumbPointUpdated;

        private bool currentValueSet = true;
        private bool sliderThumbPointHasUpdated = false;
        private bool sliderThumbPointCanMove = false;

        private int sliderBarHeight;
        private int sliderBarWidth;
        private int sliderThumbSize;
        private int sliderThumbSizeUpdated;

        private int canvasWidth;

        private Color colorThumb = coreSettings.ColorSecondary;
        private Color colorSliderBar = Color.LightGray;
        private Color colorSliderBarActive = coreSettings.ColorPrimary;

        public static readonly BindableProperty ColorThumbProperty = BindableProperty.Create(nameof(ColorThumb), typeof(Color), typeof(eliteSlider), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSlider)bindableObject).InvalidateSurface();

                }
            });
        public Color ColorThumb
        {
            get
            {
                return (Color)GetValue(ColorThumbProperty);
            }
            set
            {
                SetValue(ColorThumbProperty, value);
            }
        }

        public static readonly BindableProperty ColorSliderBarProperty = BindableProperty.Create(nameof(ColorSliderBar), typeof(Color), typeof(eliteSlider), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSlider)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        ///
        /// </summary>
        public Color ColorSliderBar
        {
            get
            {
                return (Color)GetValue(ColorSliderBarProperty);
            }
            set
            {
                SetValue(ColorSliderBarProperty, value);
            }
        }

        public static readonly BindableProperty ColorSliderBarActiveProperty = BindableProperty.Create(nameof(ColorSliderBarActive), typeof(Color), typeof(eliteSlider), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSlider)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        ///
        /// </summary>
        public Color ColorSliderBarActive
        {
            get
            {
                return (Color)GetValue(ColorSliderBarActiveProperty);
            }
            set
            {
                SetValue(ColorSliderBarActiveProperty, value);
            }
        }

        public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(int), typeof(eliteSlider), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSlider)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(eliteSlider), 1000, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSlider)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(nameof(CurrentValue), typeof(int), typeof(eliteSlider), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    if (((eliteSlider)bindableObject).CurrentValue > 0)
                        ((eliteSlider)bindableObject).currentValueSet = false;
                    ((eliteSlider)bindableObject).InvalidateSurface();

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

        public eliteSlider()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteSliderTouched;

            if (this.CurrentValue > 0)
                this.currentValueSet = false;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            int canvasHeight = eventArgs.Info.Height;

            this.sliderBarHeight = canvasHeight / 4;
            this.sliderThumbSize = this.sliderBarHeight;
            this.sliderBarWidth = this.canvasWidth - this.sliderThumbSize;

            if (!this.currentValueSet)
            {
                decimal oneValueInPercent = 100m / this.MaxValue;
                decimal currentValueInPercent = this.CurrentValue * oneValueInPercent;
                decimal onePercentInPixel = this.sliderBarWidth / 100m;
                decimal currentValueInPixel = currentValueInPercent * onePercentInPixel;

                this.sliderThumbPointHasUpdated = true;
                this.sliderThumbPointUpdated = new SKPoint((float)currentValueInPixel, canvasHeight / 2);
                this.sliderThumbPoint = this.sliderThumbPointUpdated;
                this.sliderThumbSizeUpdated = this.sliderThumbSize;

                this.currentValueSet = true;
            }

            // Slider bar
            SKPaint sliderBarPaint = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorSliderBar.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = this.sliderBarHeight,
                StrokeCap = SKStrokeCap.Round
            };

            givenCanvas.DrawLine(this.sliderBarHeight, canvasHeight / 2, this.sliderBarWidth, canvasHeight / 2, sliderBarPaint);

            // Slider bar active
            SKPaint sliderBarActivePaint = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorSliderBarActive.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = this.sliderBarHeight,
                StrokeCap = SKStrokeCap.Round
            };

            if (this.sliderThumbPointHasUpdated)
                givenCanvas.DrawLine(this.sliderBarHeight, canvasHeight / 2, this.sliderThumbPointUpdated.X + this.sliderThumbSize / 2, canvasHeight / 2, sliderBarActivePaint);

            // Slider thumb
            SKPaint sliderThumbPaint = new SKPaint()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = this.ColorThumb.ToSKColor()
            };

            if (!this.sliderThumbPointHasUpdated)
                this.sliderThumbPoint = new SKPoint(this.sliderThumbSize, canvasHeight / 2);
            else
                this.sliderThumbPoint = this.sliderThumbPointUpdated;

            givenCanvas.DrawCircle(this.sliderThumbPoint, this.sliderThumbPointHasUpdated ? this.sliderThumbSizeUpdated : this.sliderThumbSize, sliderThumbPaint);
        }

        private void eliteSliderTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Pressed:
                    {
                        float givenDistance = SKPoint.Distance(this.sliderThumbPoint, eventArgs.Location);

                        if (givenDistance <= this.sliderThumbSize)
                        {
                            this.sliderThumbPointCanMove = true;
                            this.sliderThumbSizeUpdated = this.sliderThumbSize + 8;
                            this.InvalidateSurface();
                        }
                    }
                    break;

                case SKTouchAction.Released:
                    {
                        if (this.sliderThumbPointCanMove)
                        {
                            this.sliderThumbSizeUpdated = this.sliderThumbSize;
                            this.InvalidateSurface();
                        }

                        this.sliderThumbPointCanMove = false;
                    }
                    break;

                case SKTouchAction.Moved:
                    {
                        if (this.sliderThumbPointCanMove)
                        {
                            if (eventArgs.Location.X <= this.sliderThumbSize
                                || eventArgs.Location.X >= this.canvasWidth - this.sliderThumbSize)
                                return;

                            int thumbOffset = (int)(eventArgs.Location.X - this.sliderThumbSize - 10);
                            int barWidth = this.sliderBarWidth - this.sliderThumbSize - 10;

                            decimal onePixelInPercent = 100m / barWidth;
                            decimal pixelInPercent = onePixelInPercent * thumbOffset;
                            decimal sliderPercent = Math.Ceiling(pixelInPercent);

                            this.OnValueChanged((int)sliderPercent);

                            this.sliderThumbPointUpdated = new SKPoint(eventArgs.Location.X, this.sliderThumbPoint.Y);
                            this.sliderThumbPointHasUpdated = true;
                            this.InvalidateSurface();
                        }
                    }
                    break;
            }

            eventArgs.Handled = true;
        }

        public event EventHandler<EventArgsSliderValueChanged> ValueChanged;
        private void OnValueChanged(int currentValue)
        {
            decimal onePercentInValue = this.MaxValue / 100m;
            decimal percentInValue = onePercentInValue * currentValue;

            if (percentInValue >= this.MaxValue)
                percentInValue = this.MaxValue;
            else if (percentInValue <= this.MinValue)
                percentInValue = this.MinValue;

            this.CurrentValue = (int)percentInValue;

            this.ValueChanged?.Invoke(this, new EventArgsSliderValueChanged()
            {
                CurrentValue = this.CurrentValue
            });
            ValueChangedCommand?.Execute(CurrentValue);
        }

        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(nameof(ValueChangedCommand), typeof(ICommand), typeof(eliteSlider));
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
