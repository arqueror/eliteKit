using System;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using eliteKit.eliteCore;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    [ContentProperty("HeaderView")]
    public class eliteHeader : ContentView
    {
        private AbsoluteLayout absoluteLayout;
        private eliteHeaderShape headerShape;

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteHeader), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteHeader)bindableObject).ColorPrimary = (Color)value;

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteHeader), coreSettings.ColorSecondary, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteHeader)bindableObject).ColorSecondary = (Color)value;

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

        private View headerView;
        public static readonly BindableProperty HeaderViewProperty = BindableProperty.Create(nameof(HeaderView), typeof(View), typeof(eliteHeader), default(View), BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {

                    ((eliteHeader)bindableObject).headerView = (View)value;
                    ((eliteHeader)bindableObject).headerView.Margin = new Thickness(20, 20);
                    ((eliteHeader)bindableObject).absoluteLayout.Children.Remove(((eliteHeader)bindableObject).headerView);
                    ((eliteHeader)bindableObject).absoluteLayout.Children.Add(((eliteHeader)bindableObject).headerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);

                }
            });
        /// <summary>
        /// 
        /// </summary>
        public View HeaderView
        {
            get
            {
                return (View)GetValue(HeaderViewProperty);
            }
            set
            {
                this.headerView = value;
                this.headerView.Margin = new Thickness(20, 20);
                this.absoluteLayout.Children.Remove(this.headerView);
                this.absoluteLayout.Children.Add(this.headerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);
                SetValue(HeaderViewProperty, value);
            }
        }

        public eliteHeader()
        {
            this.absoluteLayout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.headerShape = new eliteHeaderShape()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.absoluteLayout.Children.Add(this.headerShape, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            ContentView emptyView = new ContentView();
            this.headerView = emptyView;
            this.absoluteLayout.Children.Add(this.headerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);

            this.Content = this.absoluteLayout;
        }
    }

    class eliteHeaderShape : SKCanvasView
    {
        private Color colorPrimary = Color.FromHex("548EC1");
        private Color colorSecondary = Color.FromHex("254867");

        public Color ColorPrimary
        {
            get
            {
                return this.colorPrimary;
            }
            set
            {
                this.colorPrimary = value;
                this.InvalidateSurface();
            }
        }
        public Color ColorSecondary
        {
            get
            {
                return this.colorSecondary;
            }
            set
            {
                this.colorSecondary = value;
                this.InvalidateSurface();
            }
        }

        private float headerBackgroundGradientOffset = 0;
        private bool headerBackgroundGradientOffsetReversed = false;

        public eliteHeaderShape()
        {
            this.EnableTouchEvents = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(60), () =>
            {
                if (!this.headerBackgroundGradientOffsetReversed)
                {
                    this.headerBackgroundGradientOffset += 0.005f;

                    if ((double)this.headerBackgroundGradientOffset >= 0.4)
                    {
                        this.headerBackgroundGradientOffsetReversed = true;
                        this.headerBackgroundGradientOffset = 0.4f;
                    }
                }
                else
                {
                    this.headerBackgroundGradientOffset -= 0.005f;

                    if ((double)this.headerBackgroundGradientOffset <= 0)
                    {
                        this.headerBackgroundGradientOffsetReversed = false;
                        this.headerBackgroundGradientOffset = 0;
                    }
                }

                this.InvalidateSurface();
                return true;
            });
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            int canvasWidth = eventArgs.Info.Width;
            int canvasHeight = eventArgs.Info.Height;

            // Draw the background gradient rectangle
            SKRect backgroundRect = new SKRect(0, 0, canvasWidth, canvasHeight);
            SKPaint backgroundPaint = new SKPaint()
            {
                Shader = SKShader.CreateLinearGradient(
                    new SKPoint(backgroundRect.Left, backgroundRect.Top),
                    new SKPoint(backgroundRect.Right, backgroundRect.Bottom),
                    new SKColor[] {
                        this.colorPrimary.ToSKColor(),
                        this.colorSecondary.ToSKColor()
                    },
                    new float[] {
                        this.headerBackgroundGradientOffset,
                        1
                    },
                    SKShaderTileMode.Repeat
                )
            };
            givenCanvas.DrawRect(backgroundRect, backgroundPaint);

            // Draw the round path and clip it to the background gradient rectangle
            SKPath roundedPath = new SKPath();
            roundedPath.MoveTo(0, canvasHeight - 100);
            roundedPath.QuadTo(canvasWidth / 2, canvasHeight + 100, canvasWidth, canvasHeight - 100);
            roundedPath.LineTo(canvasWidth, canvasHeight);
            roundedPath.LineTo(0, canvasHeight);
            roundedPath.LineTo(0, canvasHeight - 100);
            SKPaint roundedPaint = new SKPaint()
            {
                BlendMode = SKBlendMode.SrcOut
            };
            givenCanvas.DrawPath(roundedPath, roundedPaint);
        }
    }
}
