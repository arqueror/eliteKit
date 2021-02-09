using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using eliteKit.eliteEnums;
using eliteKit.eliteCore;

namespace eliteKit.eliteElements
{
    [ContentProperty("BadgeView")]
    public class eliteBadge : ContentView
    {
        private AbsoluteLayout absoluteLayout;
        private eliteBadgeIcon badgeIcon;

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(eliteBadge), 10f, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteBadge)bindableObject).badgeIcon.FontSize = (float)value * (float)coreSettings.ScalingFactor;
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

        public static readonly BindableProperty ColorBadgeProperty = BindableProperty.Create(nameof(ColorBadge), typeof(Color), typeof(eliteBadge), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, newValue) =>
        {

            ((eliteBadge)bindableObject).badgeIcon.ColorBadge = (Color)newValue;

        });
        /// <summary>
        /// 
        /// </summary>
        public Color ColorBadge
        {
            get
            {
                //return this.badgeIcon.ColorBadge;
                return (Color)GetValue(ColorBadgeProperty);
            }
            set
            {
                SetValue(ColorBadgeProperty, value);
            }
        }

        public static readonly BindableProperty ContentBadgeProperty = BindableProperty.Create(nameof(ContentBadge), typeof(string), typeof(eliteBadge), "0", BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteBadge)bindableObject).badgeIcon.ContentBadge = (string)value;
        });
        /// <summary>
        /// 
        /// </summary>
        public string ContentBadge
        {
            get
            {
                return (string)GetValue(ContentBadgeProperty);
            }
            set
            {
                SetValue(ContentBadgeProperty, value);
            }
        }

        public static readonly BindableProperty BadgeWidthProperty = BindableProperty.Create(nameof(BadgeWidth), typeof(int), typeof(eliteBadge), 25, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            ((eliteBadge)bindableObject).badgeIcon.WidthRequest = (int)value;

        });
        /// <summary>
        /// 
        /// </summary>
        public int BadgeWidth
        {
            get
            {
                return (int)GetValue(BadgeWidthProperty);
            }
            set
            {
                SetValue(BadgeWidthProperty, value);
            }
        }


        public static readonly BindableProperty BadgeHeightProperty = BindableProperty.Create(nameof(BadgeHeight), typeof(int), typeof(eliteBadge), 25, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteBadge)bindableObject).badgeIcon.HeightRequest = (int)value;

        });
        /// <summary>
        /// 
        /// </summary>
        public int BadgeHeight
        {
            get
            {
                return (int)GetValue(BadgeHeightProperty);
            }
            set
            {
                SetValue(BadgeHeightProperty, value);
            }
        }

        public static readonly BindableProperty BadgeRadiusProperty = BindableProperty.Create(nameof(BadgeRadius), typeof(float), typeof(eliteBadge), 25f, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

            if (value != null)
                ((eliteBadge)bindableObject).badgeIcon.BadgeRadius = (float)value;

        });
        /// <summary>
        /// 
        /// </summary>
        public float BadgeRadius
        {
            get
            {
                return (float)GetValue(BadgeRadiusProperty);
            }
            set
            {
                SetValue(BadgeRadiusProperty, value);
            }
        }

        private View badgeView;
        public static readonly BindableProperty BadgeViewProperty = BindableProperty.Create(nameof(BadgeView), typeof(View), typeof(eliteBadge), default(View), propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                var bindable = bindableObject as eliteBadge;
                bindable.badgeView = (View)value;
                bindable.badgeView.Margin = new Thickness(bindable.BadgeWidth / 2);
                bindable.absoluteLayout.Children[0] = bindable.badgeView;
            }

        });
        /// <summary>
        /// 
        /// </summary>
        public View BadgeView
        {
            get
            {
                return (View)GetValue(BadgeViewProperty);
            }
            set
            {

                SetValue(BadgeViewProperty, value);
            }
        }

        public static readonly BindableProperty BadgeDirectionProperty = BindableProperty.Create(nameof(BadgeDirection), typeof(eliteBadgeDirection), typeof(eliteBadge), eliteBadgeDirection.TopRight, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                var val = (eliteBadgeDirection)value;
                switch (val)
                {
                    case eliteBadgeDirection.Top:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.Start;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.Center;
                        }
                        break;

                    case eliteBadgeDirection.TopRight:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.Start;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.End;
                        }
                        break;

                    case eliteBadgeDirection.TopLeft:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.Start;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.Start;
                        }
                        break;

                    case eliteBadgeDirection.Right:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.Center;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.End;
                        }
                        break;

                    case eliteBadgeDirection.Left:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.Center;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.Start;
                        }
                        break;

                    case eliteBadgeDirection.Bottom:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.End;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.Center;
                        }
                        break;

                    case eliteBadgeDirection.BottomRight:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.End;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.End;
                        }
                        break;

                    case eliteBadgeDirection.BottomLeft:
                        {
                            ((eliteBadge)bindableObject).badgeIcon.VerticalOptions = LayoutOptions.End;
                            ((eliteBadge)bindableObject).badgeIcon.HorizontalOptions = LayoutOptions.Start;
                        }
                        break;

                }
            }

        });
        /// <summary>
        /// 
        /// </summary>
        public eliteBadgeDirection BadgeDirection
        {
            get
            {
                return (eliteBadgeDirection)GetValue(BadgeDirectionProperty);
            }
            set
            {

                SetValue(BadgeDirectionProperty, value);
            }
        }

        public eliteBadge()
        {
            this.absoluteLayout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.badgeIcon = new eliteBadgeIcon()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                WidthRequest = this.BadgeWidth,
                HeightRequest = this.BadgeHeight
            };

            ContentView emptyView = new ContentView();
            this.badgeView = emptyView;

            this.absoluteLayout.Children.Add(this.badgeView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            this.absoluteLayout.Children.Add(this.badgeIcon, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            this.Content = this.absoluteLayout;

            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.Center;
        }
    }

    class eliteBadgeIcon : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private Color colorBadge = Color.Red;
        private string contentBadge = "0";
        private float fontSize = 10f * coreSettings.ScalingFactor;
        private float badgeRadius = 25;

        public float BadgeRadius
        {
            get
            {
                return this.badgeRadius;
            }
            set
            {
                this.badgeRadius = value;
                this.InvalidateSurface();
            }
        }

        public float FontSize
        {
            get
            {
                return this.fontSize;
            }
            set
            {
                this.fontSize = value;
                this.InvalidateSurface();
            }
        }

        public Color ColorBadge
        {
            get
            {
                return this.colorBadge;
            }
            set
            {
                this.colorBadge = value;
                this.InvalidateSurface();
            }
        }
        public string ContentBadge
        {
            get
            {
                return this.contentBadge;
            }
            set
            {
                this.contentBadge = value;
                this.InvalidateSurface();
            }
        }

        public eliteBadgeIcon() { }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            SKPaint paintBadge = new SKPaint()
            {
                IsAntialias = true,
                Color = this.colorBadge.ToSKColor()
            };
            SKRect rectBadge = new SKRect(0, 0, this.canvasWidth, this.canvasHeight);
            SKRoundRect roundRectBadge = new SKRoundRect(rectBadge, BadgeRadius, BadgeRadius);
            givenCanvas.DrawRoundRect(roundRectBadge, paintBadge);

            SKPaint paintBadgeText = new SKPaint()
            {
                TextSize = FontSize,
                IsAntialias = true,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center
            };

            SKRect rectBadgeText = new SKRect();
            paintBadgeText.MeasureText(this.contentBadge, ref rectBadgeText);

            givenCanvas.DrawText(this.contentBadge, this.canvasWidth / 2, this.canvasHeight / 2 - rectBadgeText.MidY, paintBadgeText);
        }
    }
}