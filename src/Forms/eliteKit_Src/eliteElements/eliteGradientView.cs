using eliteKit.eliteEnums;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    [ContentProperty("ContentView")]
    public class eliteGradientView : ContentView
    {
        private AbsoluteLayout absoluteLayout;
        private eliteGradientViewShape gradientViewShape;

        private View contentView;
        public View ContentView
        {
            get
            {
                return this.contentView;
            }
            set
            {
                this.contentView = value;
                this.contentView.Margin = new Thickness(20, 20);
                this.absoluteLayout.Children.Remove(this.contentView);
                this.absoluteLayout.Children.Add(this.contentView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);
            }
        }


        public static readonly BindableProperty CustomTransitionSpeedProperty = BindableProperty.Create(nameof(CustomTransitionSpeed), typeof(float), typeof(eliteGradientView), 0f, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                var customSpeed = (float)newValue;
                thisView.gradientViewShape.CustomTransitionSpeed = customSpeed;
            
        });
        public float CustomTransitionSpeed
        {
            get
            {
                return (float)GetValue(CustomTransitionSpeedProperty);
            }
            set
            {
                SetValue(CustomTransitionSpeedProperty, value);
            }
        }


        public static readonly BindableProperty Color1Property = BindableProperty.Create(nameof(Color1), typeof(Color), typeof(eliteGradientView), Color.FromRgb(62, 35, 200), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {

            var thisView = (eliteGradientView)bindable;
            var newC = (Color)newValue;
            thisView.gradientViewShape.Color1 = newC.ToSKColor();
        });
        public Color Color1
        {
            get
            {
                return (Color)GetValue(Color1Property);
            }
            set
            {
                SetValue(Color1Property, value);
            }
        }

        public static readonly BindableProperty Color2Property = BindableProperty.Create(nameof(Color2), typeof(Color), typeof(eliteGradientView), Color.FromRgb(60, 255, 200), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {

            // We can call something here to update the UI
            var thisView = (eliteGradientView)bindable;

            var newC = (Color)newValue;
            // Update the UI to display
            thisView.gradientViewShape.Color2 = newC.ToSKColor();
        });
        public Color Color2
        {
            get
            {
                return (Color)GetValue(Color2Property);
            }
            set
            {
                SetValue(Color2Property, value);

            }
        }

        public static readonly BindableProperty GradientTypeProperty = BindableProperty.Create(nameof(GradientType), typeof(GradientType), typeof(eliteGradientView), GradientType.Radial, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                var gType = (GradientType)newValue;
                thisView.gradientViewShape.GradientType = gType;
            
        });
        public GradientType GradientType
        {
            get
            {
                return (GradientType)GetValue(GradientTypeProperty);
            }
            set
            {
                SetValue(GradientTypeProperty, value);

            }
        }

        public static readonly BindableProperty TileModeProperty = BindableProperty.Create(nameof(TileMode), typeof(SKShaderTileMode), typeof(eliteGradientView), SKShaderTileMode.Clamp, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                var gType = (SKShaderTileMode)newValue;
                thisView.gradientViewShape.TileMode = gType;
            
        });
        public SKShaderTileMode TileMode
        {
            get
            {
                return (SKShaderTileMode)GetValue(TileModeProperty);
            }
            set
            {
                SetValue(TileModeProperty, value);

            }
        }


        public static readonly BindableProperty Color3Property = BindableProperty.Create(nameof(Color3), typeof(Color), typeof(eliteGradientView), Color.FromRgb(255, 35, 98), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {

            // We can call something here to update the UI
            var thisView = (eliteGradientView)bindable;

            var newC = (Color)newValue;
            // Update the UI to display
            thisView.gradientViewShape.Color3 = newC.ToSKColor();
        });
        public Color Color3
        {
            get
            {
                return (Color)GetValue(Color3Property);
            }
            set
            {
                SetValue(Color3Property, value);

            }
        }

        public static readonly BindableProperty Color4Property = BindableProperty.Create(nameof(Color4), typeof(Color), typeof(eliteGradientView), Color.FromRgb(45, 175, 200), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {

            // We can call something here to update the UI
            var thisView = (eliteGradientView)bindable;

            var newC = (Color)newValue;
            // Update the UI to display
            thisView.gradientViewShape.Color4 = newC.ToSKColor();
        });
        public Color Color4
        {
            get
            {
                return (Color)GetValue(Color4Property);
            }
            set
            {
                SetValue(Color4Property, value);

            }
        }

        public static readonly BindableProperty ChangeColorsAutomaticallyProperty = BindableProperty.Create(nameof(ChangeColorsAutomatically), typeof(bool), typeof(eliteGradientView), true, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                thisView.gradientViewShape.ChangeColorsAutomatically = (bool)newValue;
            
        });
        public bool ChangeColorsAutomatically
        {
            get
            {
                return (bool)GetValue(ChangeColorsAutomaticallyProperty);
            }
            set
            {
                SetValue(ChangeColorsAutomaticallyProperty, value);
            }
        }

        public static readonly BindableProperty GradientAnimationEnabledProperty = BindableProperty.Create(nameof(GradientAnimationEnabled), typeof(bool), typeof(eliteGradientView), true, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                thisView.gradientViewShape.GradientAnimationEnabled = (bool)newValue;
            
        });
        public bool GradientAnimationEnabled
        {
            get
            {
                return (bool)GetValue(GradientAnimationEnabledProperty);
            }
            set
            {
                SetValue(GradientAnimationEnabledProperty, value);
            }
        }

        public static readonly BindableProperty TransitionSpeedProperty = BindableProperty.Create(nameof(TransitionSpeed), typeof(eliteSpeed), typeof(eliteGradientView), eliteSpeed.Normal, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
                var thisView = (eliteGradientView)bindable;
                thisView.gradientViewShape.TransitionSpeed = (eliteSpeed)newValue;
            
        });
        public eliteSpeed TransitionSpeed
        {
            get
            {
                return (eliteSpeed)GetValue(TransitionSpeedProperty);
            }
            set
            {
                SetValue(TransitionSpeedProperty, value);

            }
        }

        public eliteGradientView()
        {

            this.absoluteLayout = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.gradientViewShape = new eliteGradientViewShape()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand

            };

            this.absoluteLayout.Children.Add(this.gradientViewShape, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            ContentView emptyView = new ContentView();
            this.contentView = emptyView;
            this.absoluteLayout.Children.Add(this.contentView, new Rectangle(.5, 0, 1, 1), AbsoluteLayoutFlags.All);
            this.Content = this.absoluteLayout;


        }

    }

    class eliteGradientViewShape : SKCanvasView
    {
        private SKCanvas canvas;
        private Stack<double> ReverseStack = new Stack<double>();
        private bool IsRevertingGradient = false;

        //transition speed
        static float gradientSpeed = 0.003f;
        static double step = 0;

        // color table indices for:
        // current color left
        // next color left
        // current color right
        // next color right
        static int[] colorIndices = { 0, 1, 2, 3 };

        // background brush
        SKPaint backgroundBrush = new SKPaint()
        {
            Style = SKPaintStyle.Fill
        };

        //Default colors
        static int[][] colors =
        {
            new int[] {62,35,200},
            new int[] {60, 255, 200},
            new int[] {255, 35, 98},
            new int[] { 45, 175, 200 }
        };
        public float CustomTransitionSpeed
        {
            set => gradientSpeed = value;
            get => gradientSpeed;
        }
        #region Properties


        public GradientType GradientType { get; set; }

        public SKShaderTileMode TileMode { get; set; }

        public SKColor Color1
        {
            set => colors[0] = new int[] { value.Red, value.Green, value.Blue };
        }

        public SKColor Color2
        {
            set => colors[1] = new int[] { value.Red, value.Green, value.Blue };
        }

        public SKColor Color3
        {
            set => colors[2] = new int[] { value.Red, value.Green, value.Blue };
        }

        public SKColor Color4
        {
            set => colors[3] = new int[] { value.Red, value.Green, value.Blue };
        }

        private bool changeColorsAutomatically = true;
        public bool ChangeColorsAutomatically
        {
            get { return changeColorsAutomatically; }
            set
            {
                //IsRevertingGradient = !value;
                changeColorsAutomatically = value;
            }
        }

        private bool gradientAnimationEnabled = true;
        public bool GradientAnimationEnabled
        {
            get { return gradientAnimationEnabled; }
            set { gradientAnimationEnabled = value; }
        }

        public eliteSpeed TransitionSpeed
        {
            set
            {
                switch (value)
                {
                    case eliteSpeed.Slow:
                        gradientSpeed = 0.001f;
                        break;
                    case eliteSpeed.Normal:
                        gradientSpeed = 0.003f;
                        break;
                    case eliteSpeed.Fast:
                        gradientSpeed = 0.006f;
                        break;
                }
            }
        }
        #endregion

        public eliteGradientViewShape()
        {

        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            // set the canvas and properties
            canvas = e.Surface.Canvas;


            // get the screen density for scaling
            var scale = (float)(e.Info.Width / this.Width);

            // handle the device screen density
            canvas.Scale(scale);

            canvas.Clear(SKColors.White);
            if (step >= 1 || IsRevertingGradient)
            {

                IsRevertingGradient = ReverseStack.Any() && !ChangeColorsAutomatically;

                if (IsRevertingGradient && ReverseStack.Any())
                    DrawGradient(e, true);

                if (!ReverseStack.Any())
                {
                    IsRevertingGradient = false;
                    step %= 1;
                }

                if (!IsRevertingGradient && ChangeColorsAutomatically)
                {
                    MoveToNextColor();
                    DrawGradient(e);
                }

            }
            else
            {
                try
                {
                    DrawGradient(e);
                    step += gradientSpeed;
                }
                catch { }
            }
        }

        public static void MoveToNextColor()
        {
            try
            {
                //Move to new color
                step %= 1;
                colorIndices[0] = colorIndices[1];
                colorIndices[2] = colorIndices[3];

                //pick two new target color indices
                //do not pick the same as the current one
                colorIndices[1] =
                    (int)(colorIndices[1] + Math.Floor((decimal)1 + new Random().Next(100) * (colors.Length - 1))) %
                    colors.Length;
                colorIndices[3] =
                    (int)(colorIndices[3] + Math.Floor((decimal)1 + new Random().Next(100) * (colors.Length - 1))) %
                    colors.Length;
            }
            catch { }
        }

        private void DrawGradient(SKPaintSurfaceEventArgs args, bool reverseCurrent = false)
        {
            var c0_0 = colors[colorIndices[0]];
            var c0_1 = colors[colorIndices[1]];
            var c1_0 = colors[colorIndices[2]];
            var c1_1 = colors[colorIndices[3]];
            SKColor color1, color2;
            if (!reverseCurrent)
            {
                var istep = 1 - step;
                ReverseStack.Push(step);
                var r1 = Convert.ToByte(Math.Round(istep * c0_0[0] + step * c0_1[0]));
                var g1 = Convert.ToByte(Math.Round(istep * c0_0[1] + step * c0_1[1]));
                var b1 = Convert.ToByte(Math.Round(istep * c0_0[2] + step * c0_1[2]));
                color1 = new SKColor(r1, g1, b1);

                var r2 = Convert.ToByte(Math.Round(istep * c1_0[0] + step * c1_1[0]));
                var g2 = Convert.ToByte(Math.Round(istep * c1_0[1] + step * c1_1[1]));
                var b2 = Convert.ToByte(Math.Round(istep * c1_0[2] + step * c1_1[2]));
                color2 = new SKColor(r2, g2, b2);
            }
            else
            {
                var kstep = ReverseStack.Pop();
                step = 1 - kstep;

                var r1 = Convert.ToByte(Math.Round(step * c0_0[0] + kstep * c0_1[0]));
                var g1 = Convert.ToByte(Math.Round(step * c0_0[1] + kstep * c0_1[1]));
                var b1 = Convert.ToByte(Math.Round(step * c0_0[2] + kstep * c0_1[2]));
                color1 = new SKColor(r1, g1, b1);

                var r2 = Convert.ToByte(Math.Round(step * c1_0[0] + kstep * c1_1[0]));
                var g2 = Convert.ToByte(Math.Round(step * c1_0[1] + kstep * c1_1[1]));
                var b2 = Convert.ToByte(Math.Round(step * c1_0[2] + kstep * c1_1[2]));
                color2 = new SKColor(r2, g2, b2);
            }


            backgroundBrush.IsAntialias = true;

            switch (GradientType)
            {
                case GradientType.Radial:
                    backgroundBrush.Shader = SKShader.CreateRadialGradient(
                    new SKPoint(-60, args.Info.Height * .8f),
                    args.Info.Height * 1.5f,
                    new SKColor[] { color1, color2 },
                    new float[] { GradientAnimationEnabled ? 0 : 1f, 1f },
                    TileMode);
                    break;
                case GradientType.Linear:
                    backgroundBrush.Shader = SKShader.CreateLinearGradient(
                      new SKPoint(-60, -60),
                      new SKPoint(args.Info.Width, args.Info.Height * 1.5f),
                      new SKColor[] { color1, color2 },
                      new float[] { GradientAnimationEnabled ? 0 : 1f, 1f },
                      TileMode);
                    break;
                case GradientType.Sweep:
                    SKPoint center = new SKPoint(args.Info.Rect.MidX + 60, args.Info.Rect.MidY + 60);
                    backgroundBrush.Shader = SKShader.CreateSweepGradient(center,
                        new SKColor[] { color1, color2 },
                        new float[] { GradientAnimationEnabled ? 0 : 1f, 1f });
                    break;
            }
            // gradient background with 3 colors


            SKRect backgroundBounds = new SKRect(0, 0, args.Info.Width, args.Info.Height);
            canvas.DrawRect(backgroundBounds, backgroundBrush);
            Device.BeginInvokeOnMainThread(this.InvalidateSurface);
        }
    }
}