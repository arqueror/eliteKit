using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    [ContentProperty("FooterView")]
    public class eliteFooter : ContentView
    {
        private AbsoluteLayout absoluteLayout;
        private eliteFooterShape footerShape;
        public EventHandler FinishedPresenting;

        /// <summary>
        /// 
        /// </summary>
        public SKPaintSurfaceEventArgs CanvasData
        {
            get
            {
                return footerShape?.CanvasData;
            }
        }

        public static readonly BindableProperty FooterPathProperty = BindableProperty.Create(nameof(FooterPath), typeof(SKPath), typeof(eliteFooter), (null), BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteFooter)bindableObject).footerShape.Custompath = (SKPath)value;
            }
        });
        /// <summary>
        /// 
        /// </summary>
        public SKPath FooterPath
        {
            get
            {
                return (SKPath)GetValue(FooterPathProperty);
            }
            set
            {
                SetValue(FooterPathProperty, value);
            }
        }


        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteFooter), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteFooter)bindableObject).footerShape.ColorPrimary = (Color)value;
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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteFooter), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteFooter)bindableObject).footerShape.ColorSecondary = (Color)value;
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

        public static readonly BindableProperty FooterViewProperty = BindableProperty.Create(nameof(FooterView), typeof(View), typeof(eliteFooter), default(View), BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    var val = (View)value;
                    ((eliteFooter)bindableObject).footerView = val;
                    ((eliteFooter)bindableObject).footerView.Margin = new Thickness(20, 20);
                    ((eliteFooter)bindableObject).absoluteLayout.Children.Remove(((eliteFooter)bindableObject).footerView);
                    ((eliteFooter)bindableObject).absoluteLayout.Children.Add(((eliteFooter)bindableObject).footerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);

                }
            });
        private View footerView;
        /// <summary>
        /// 
        /// </summary>
        public View FooterView
        {
            get
            {
                return (View)GetValue(FooterViewProperty);
            }
            set
            {
                this.footerView = value;
                this.footerView.Margin = new Thickness(20, 20);
                this.absoluteLayout.Children.Remove(this.footerView);
                this.absoluteLayout.Children.Add(this.footerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);
                SetValue(FooterViewProperty, value);
            }
        }

        public eliteFooter()
        {
            this.absoluteLayout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.footerShape = new eliteFooterShape()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.absoluteLayout.Children.Add(this.footerShape, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            ContentView emptyView = new ContentView();
            this.footerView = emptyView;
            this.absoluteLayout.Children.Add(this.footerView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);

            this.Content = this.absoluteLayout;

            VerticalOptions = LayoutOptions.End;
            HorizontalOptions = LayoutOptions.FillAndExpand;

            footerShape.FinishedPresenting += (s, a) => 
            {
                FinishedPresenting?.Invoke(this, null);
                footerShape.FinishedPresenting = null; 
            };
        }
    }

    class eliteFooterShape : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;
        private SKPath customPath = null;
        private SKPaintSurfaceEventArgs canvasData = null;
        public EventHandler FinishedPresenting;

        private Color colorPrimary = Color.FromHex("548EC1");
        private Color colorSecondary = Color.FromHex("254867");

        public SKPaintSurfaceEventArgs CanvasData
        {
            get
            {
                return canvasData;
            }
        }

        public SKPath Custompath
        {
            get
            {
                return customPath;
            }
            set
            {
                customPath = value;
                this.InvalidateSurface();
            }
        }

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

        public eliteFooterShape()
        {
            this.EnableTouchEvents = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            canvasData = eventArgs;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

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
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                )
            };
            givenCanvas.DrawRect(backgroundRect, backgroundPaint);

            // Draw the round path and clip it to the background gradient rectangle
            SKPath roundedPath = new SKPath();

            if (customPath == null)
            {
                roundedPath.MoveTo(0, 100);
                roundedPath.QuadTo(canvasWidth / 2, -100, canvasWidth, 100);
                roundedPath.LineTo(canvasWidth, 0);
                roundedPath.LineTo(0, 0);
            }
            else
            {
                roundedPath = customPath;
            }

            SKPaint roundedPaint = new SKPaint()
            {
                BlendMode = SKBlendMode.SrcOut,
                IsAntialias = true
            };

            givenCanvas.DrawPath(roundedPath, roundedPaint);


            FinishedPresenting?.Invoke(this, null);
        }
    }
}
