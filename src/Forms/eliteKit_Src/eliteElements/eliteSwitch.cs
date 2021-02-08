using eliteKit.eliteCore;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteSwitch : SKCanvasView
    {
        #region Members
        private int canvasWidth = 0;
        private int canvasHeight = 0;

        private int switchToggleWidth = 0;
        private int switchToggleHeight = 0;
        private int switchToggleLeft = 15;
        private int switchToggleLeftUpdated = 0;
        private bool switchToggleUpdated = false;

        private bool IsAnimating = false;
        private int AnimatingLastXIndex = 0;
        #endregion

        #region Properties
        public static readonly BindableProperty ToggledCommandProperty = BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(eliteSwitch));
        /// <summary>
        ///   Command that returns a bool value indicating if Switch is toggled
        /// </summary>
        public ICommand ToggledCommand
        {
            get => (ICommand)GetValue(ToggledCommandProperty);
            set => SetValue(ToggledCommandProperty, value);
        }

        public static readonly BindableProperty AnimationSpeedProperty = BindableProperty.Create(nameof(AnimationSpeed), typeof(int), typeof(eliteSwitch), 15, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSwitch)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        ///     Switch toggled animation speed .Default value is 15
        /// </summary>
        public int AnimationSpeed
        {
            get
            {
                return (int)GetValue(AnimationSpeedProperty);
            }
            set
            {
                if (value <= 0) return;
                SetValue(AnimationSpeedProperty, value);
            }
        }

        public static readonly BindableProperty SwitchToggledProperty = BindableProperty.Create(nameof(SwitchToggled), typeof(bool), typeof(eliteSwitch), false, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                if (value != null)
                {
                    ((eliteSwitch)bindableObject).InvalidateSurface();

                }
            });
        /// <summary>
        ///     Switch toggled property. Default value is false
        /// </summary>
        public bool SwitchToggled
        {
            get
            {
                return (bool)GetValue(SwitchToggledProperty);
            }
            set
            {
                SetValue(SwitchToggledProperty, value);
            }
        }

        public static readonly BindableProperty ToggledBackgroundColorProperty = BindableProperty.Create(nameof(ToggledBackgroundColor), typeof(Color), typeof(eliteSwitch), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteSwitch)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        ///     Toggled switch background color. Default value is LightGray
        /// </summary>
        public Color ToggledBackgroundColor
        {
            get
            {
                return (Color)GetValue(ToggledBackgroundColorProperty);
            }
            set
            {
                SetValue(ToggledBackgroundColorProperty, value);
            }
        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteSwitch), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteSwitch)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        ///    Switch toggle color. Default value is LightGray
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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteSwitch), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteSwitch)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        ///    Switch toggle gradient secondary color if IsGradient property is being set to true. Default value is LightGray
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

        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteSwitch), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteSwitch)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        ///    Switch background color. Default value is LightGray
        /// </summary>
        public Color ColorBackground
        {
            get
            {
                return (Color)GetValue(ColorBackgroundProperty);
            }
            set
            {
                SetValue(ColorBackgroundProperty, value);
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteSwitch), false, propertyChanged: (bindableObject, oldValue, value) =>
            {
                    if (value != null)
                    {
                        ((eliteSwitch)bindableObject).InvalidateSurface();

                    }
                
            });
        /// <summary>
        ///    If true, toggle is drawn using a gradient. Default value is false
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
        #endregion

        #region Constructor
        public eliteSwitch()
        {
            EnableTouchEvents = true;
            Touch += eliteSwitchTouched;
        }
        #endregion  

        #region Events
        private void OnChecked()
        {
            Checked?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Checked;

        private void OnUnchecked() => Unchecked?.Invoke(this, EventArgs.Empty);
        public event EventHandler Unchecked;

        private void OnToggled()
        {
            Toggled?.Invoke(this, SwitchToggled);
            if (ToggledCommand != null)
                ToggledCommand.Execute(SwitchToggled);
        }
        public event EventHandler<bool> Toggled;

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            canvasWidth = eventArgs.Info.Width;
            canvasHeight = eventArgs.Info.Height;

            switchToggleWidth = canvasHeight - 15;
            switchToggleHeight = canvasHeight - 15;
            switchToggleLeft = canvasHeight / 2;

            if (SwitchToggled)
            {
                switchToggleUpdated = true;
                switchToggleLeftUpdated = canvasWidth - canvasHeight / 2;
            }

            SKRoundRect switchRoundRectBackground = new SKRoundRect(new SKRect(0, 0, canvasWidth, canvasHeight), canvasHeight / 2, (canvasHeight / 2));
            SKPaint switchPaintBackground = new SKPaint()
            {
                Color = ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            if (SwitchToggled)
                switchPaintBackground.Color = ToggledBackgroundColor.ToSKColor();

            givenCanvas.DrawRoundRect(switchRoundRectBackground, switchPaintBackground);

            SKPaint switchPaintToggle = new SKPaint()
            {
                Color = ColorPrimary.ToSKColor(),
                IsAntialias = true
            };

            if (IsGradient)
            {
                switchPaintToggle.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(SwitchToggled ? (canvasWidth - canvasHeight) + AnimatingLastXIndex : 0, AnimatingLastXIndex),
                    new SKPoint(SwitchToggled ? canvasWidth : canvasHeight + AnimatingLastXIndex, canvasHeight + AnimatingLastXIndex),
                    new[]
                    {
                        ColorPrimary.ToSKColor(),
                        ColorSecondary.ToSKColor()
                    },
                    new float[]
                    {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );
            }


            SKPath switchPathToggle = new SKPath();
            switchPathToggle.MoveTo(15, 15);
            if (IsAnimating)
            {
                if (!SwitchToggled) //On right side
                {
                    if (AnimatingLastXIndex > switchToggleLeftUpdated)
                        AnimatingLastXIndex -= AnimationSpeed;
                    else IsAnimating = false;
                }
                else//On left side
                {
                    if (AnimatingLastXIndex < switchToggleLeftUpdated)
                        AnimatingLastXIndex += AnimationSpeed;
                    else IsAnimating = false;
                }
            }

            if (!IsAnimating)
            {
                AnimatingLastXIndex = switchToggleUpdated ? switchToggleLeftUpdated : switchToggleLeft;
                switchPathToggle.AddCircle(AnimatingLastXIndex, canvasHeight / 2, canvasHeight / 2 - 15);
                givenCanvas.DrawPath(switchPathToggle, switchPaintToggle);
            }
            else
            {
                switchPathToggle.AddCircle(AnimatingLastXIndex, canvasHeight / 2, canvasHeight / 2 - 15);
                givenCanvas.DrawPath(switchPathToggle, switchPaintToggle);
                InvalidateSurface();
            }
        }

        private void eliteSwitchTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        switchToggleUpdated = true;
                        IsAnimating = true;

                        if (!SwitchToggled)
                            switchToggleLeftUpdated = canvasWidth - canvasHeight / 2;
                        else
                            switchToggleLeftUpdated = canvasHeight / 2;

                        SwitchToggled = !SwitchToggled;

                        if (SwitchToggled)
                            OnChecked();
                        else
                            OnUnchecked();

                        OnToggled();

                        InvalidateSurface();
                    }
                    break;
            }

            eventArgs.Handled = true;
        }
        #endregion
    }
}
