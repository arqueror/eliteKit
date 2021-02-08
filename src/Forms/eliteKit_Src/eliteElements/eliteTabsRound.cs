using eliteKit.eliteCore;
using eliteKit.eliteEventArgs;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteTabsRound : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private float tabItemPositionLeft = 0;
        private float tabItemPositionTop = 0;

        private float tabItemWidth = 0;
        private int tabItemSelected = 0;


        public static readonly BindableProperty TextAlignProperty = BindableProperty.Create(nameof(TextAlign), typeof(SKTextAlign), typeof(eliteTabsRound), SKTextAlign.Center, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
                ((eliteTabsRound)bindableObject).InvalidateSurface();
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

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float), typeof(eliteTabsRound), 40f, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

                eliteVideo kk = new eliteVideo();
                
            
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

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(eliteTabsRound), coreSettings.DefaultFontFamily, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();
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

        public static readonly BindableProperty TabsCollectionProperty = BindableProperty.Create(nameof(TabsCollection), typeof(ObservableCollection<string>), typeof(eliteTabsRound), default(ObservableCollection<string>), propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            ((eliteTabsRound)bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
        });
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> TabsCollection
        {
            get => (ObservableCollection<string>)GetValue(TabsCollectionProperty);
            set
            {

                SetValue(TabsCollectionProperty, value);
            }
        }
        void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var notifyCollection = newValue as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += (sender, e) =>
                {
                    if (e.NewItems != null)
                    {
                        var tempList = new List<string>();
                        if (e.NewItems != null)
                        {
                            foreach (string newItem in e.NewItems)
                            {
                                tempList.Add(newItem);
                            }
                            foreach (var newItem in TabsCollection)
                            {
                                var item = newItem as string;
                                tempList.Add(item);
                            }

                            TabsCollection = new ObservableCollection<string>(tempList);
                        }
                    }

                };
            }

        }

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteTabsRound), coreSettings.ColorPrimary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteTabsRound), coreSettings.ColorSecondary, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

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

        public static readonly BindableProperty ColorBackgroundProperty = BindableProperty.Create(nameof(ColorBackground), typeof(Color), typeof(eliteTabsRound), Color.LightGray, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

                }
            
        });
        /// <summary>
        ///   
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

        public static readonly BindableProperty ColorTabActiveTextProperty = BindableProperty.Create(nameof(ColorTabActiveText), typeof(Color), typeof(eliteTabsRound), Color.White, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

                }
            
        });
        /// <summary>
        ///   
        /// </summary>
        public Color ColorTabActiveText
        {
            get
            {
                return (Color)GetValue(ColorTabActiveTextProperty);
            }
            set
            {
                SetValue(ColorTabActiveTextProperty, value);
            }
        }

        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteTabsRound), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteTabsRound)bindableObject).InvalidateSurface();

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

        public eliteTabsRound()
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteTabsRoundTouched;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            if (TabsCollection == null) return;
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();
            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            this.tabItemWidth = this.canvasWidth / this.TabsCollection.Count;
            float tabItemRadius = this.canvasHeight / 2;

            SKRoundRect roundRectBackground = new SKRoundRect(new SKRect(0, 0, this.canvasWidth, this.canvasHeight), tabItemRadius, tabItemRadius);
            SKPaint paintBackground = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorBackground.ToSKColor(),
                Style = SKPaintStyle.Fill
            };
            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);

            SKRoundRect roundRectTabActive = new SKRoundRect(new SKRect((this.tabItemSelected * this.tabItemWidth), 0, (this.tabItemSelected * this.tabItemWidth) + tabItemWidth, this.canvasHeight), tabItemRadius, tabItemRadius);
            SKPaint paintTabActive = new SKPaint()
            {
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Fill
            };

            if (this.IsGradient)
                paintTabActive.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(roundRectTabActive.Rect.Left, roundRectTabActive.Rect.Top),
                    new SKPoint(roundRectTabActive.Rect.Right, roundRectTabActive.Rect.Bottom),
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

            givenCanvas.DrawRoundRect(roundRectTabActive, paintTabActive);

            SKPaint paintTabItemActiveText = new SKPaint()
            {
                TextSize = FontSize,
                IsAntialias = true,
                Color = this.ColorTabActiveText.ToSKColor(),
                TextAlign = TextAlign,
                Typeface = TextFont
            };
            SKPaint paintTabItemText = new SKPaint()
            {
                TextSize = FontSize,
                IsAntialias = true,
                Color = this.ColorPrimary.ToSKColor(),
                TextAlign = TextAlign,
                Typeface = TextFont
            };

            for (int i = 0; i < this.TabsCollection.Count; i++)
            {
                string tabItemTitle = this.TabsCollection[i];

                this.tabItemPositionLeft = ((tabItemWidth) * i) + (tabItemWidth / 2);
                this.tabItemPositionTop = (this.canvasHeight / 2) + 15f;

                givenCanvas.DrawText(tabItemTitle, this.tabItemPositionLeft, this.tabItemPositionTop, i == this.tabItemSelected ? paintTabItemActiveText : paintTabItemText);
            }
        }

        private void eliteTabsRoundTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        float positionX = eventArgs.Location.X;

                        for (int i = 0; i < this.TabsCollection.Count; i++)
                        {
                            float tabStartingPositionX = (i * this.tabItemWidth);

                            if (positionX >= tabStartingPositionX
                                && positionX <= (tabStartingPositionX + this.tabItemWidth))
                            {
                                this.tabItemSelected = i;
                                this.OnTabChanged();
                                this.InvalidateSurface();
                            }
                        }
                    }
                    break;
            }

            eventArgs.Handled = true;
        }

        public event EventHandler<EventArgsTabChanged> TabChanged;
        private void OnTabChanged()
        {
            this.TabChanged?.Invoke(this, new EventArgsTabChanged()
            {
                tabSelected = this.tabItemSelected
            });
            TabChangedCommand?.Execute(this.tabItemSelected);
        }

        public static readonly BindableProperty TabChangedCommandProperty = BindableProperty.Create(nameof(TabChangedCommand), typeof(ICommand), typeof(eliteTabsRound));
        /// <summary>
        ///   
        /// </summary>
        public ICommand TabChangedCommand
        {
            get => (ICommand)GetValue(TabChangedCommandProperty);
            set => SetValue(TabChangedCommandProperty, value);
        }
    }
}