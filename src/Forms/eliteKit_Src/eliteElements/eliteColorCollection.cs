using eliteKit.eliteEnums;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    /// <summary>
    /// EliteColorCollection ContentView MainClass
    /// </summary>
    public class eliteColorCollection : ContentView
    {
        private ScrollView scrollView;
        private StackLayout stackLayoutContent;

        private List<eliteColorShape> colorShapeCollection = new List<eliteColorShape>();

        #region PROPERTIES
        public static readonly BindableProperty BoolIsMultiSelectProperty = BindableProperty.Create(nameof(IsMultiSelect), typeof(bool), typeof(eliteColorCollection), false, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != BoolIsMultiSelectProperty.DefaultValue)
                {
                    ((eliteColorCollection)bindableObject).IsMultiSelect = (bool)BoolIsMultiSelectProperty.DefaultValue;
                    return;
                }
            
        });
        /// <summary>
        /// 
        /// </summary>
        public bool IsMultiSelect
        {
            get
            {
                return (bool)GetValue(BoolIsMultiSelectProperty);
            }
            set
            {
                SetValue(BoolIsMultiSelectProperty, value);
            }
        }

        public static readonly BindableProperty ObservableCollectionColorShapesProperty = BindableProperty.Create(nameof(ColorShapes), typeof(ObservableCollection<string>), typeof(eliteColorCollection), new ObservableCollection<string>(), BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteColorCollection)bindableObject).stackLayoutContent.Children.Clear();
                ((eliteColorCollection)bindableObject).colorShapeCollection = new List<eliteColorShape>();

                var newCollection = value as ObservableCollection<string>;
                foreach (string color in newCollection)
                {
                    eliteColorShape colorShape = new eliteColorShape(((eliteColorCollection)bindableObject), Color.FromHex(color));
                    ((eliteColorCollection)bindableObject).stackLayoutContent.Children.Add(colorShape);
                    ((eliteColorCollection)bindableObject).colorShapeCollection.Add(colorShape);
                }
            }
        });
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> ColorShapes
        {
            get
            {
                return (ObservableCollection<string>)GetValue(ObservableCollectionColorShapesProperty);
            }
            set
            {
                SetValue(ObservableCollectionColorShapesProperty, value);
            }
        }

        public static readonly BindableProperty EnumColorCollectionAnimationTypeProperty = BindableProperty.Create(nameof(AnimationType), typeof(eliteAnimationType), typeof(eliteColorCollection), eliteAnimationType.BorderOut, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != EnumColorCollectionAnimationTypeProperty.DefaultValue)
                {
                    ((eliteColorCollection)bindableObject).AnimationType = (eliteAnimationType)EnumColorCollectionAnimationTypeProperty.DefaultValue;
                    return;
                }
            
        });
        /// <summary>
        /// 
        /// </summary>
        public eliteAnimationType AnimationType
        {
            get
            {
                return (eliteAnimationType)GetValue(EnumColorCollectionAnimationTypeProperty);
            }
            set
            {
                SetValue(EnumColorCollectionAnimationTypeProperty, value);
            }
        }

        public static readonly BindableProperty intMaxSelectedItemsProperty = BindableProperty.Create(nameof(MaxSelectedItems), typeof(int), typeof(eliteColorCollection), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public int MaxSelectedItems
        {
            get
            {
                return (int)GetValue(intMaxSelectedItemsProperty);
            }
            set
            {
                SetValue(intMaxSelectedItemsProperty, value);
            }
        }

        public static readonly BindableProperty intCurrentSelectedItemsProperty = BindableProperty.Create(nameof(CurrentSelectedItems), typeof(int), typeof(eliteColorCollection), 0, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public int CurrentSelectedItems
        {
            get
            {
                return (int)GetValue(intCurrentSelectedItemsProperty);
            }
            set
            {
                SetValue(intCurrentSelectedItemsProperty, value);
            }
        }

        public static readonly BindableProperty enumOrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(eliteOrientation), typeof(eliteColorCollection), eliteOrientation.Horizontal, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                ((eliteColorCollection)bindableObject).scrollView.Orientation = (eliteOrientation)value == eliteOrientation.Horizontal ? ScrollOrientation.Horizontal : ScrollOrientation.Vertical;
                ((eliteColorCollection)bindableObject).stackLayoutContent.Orientation = (eliteOrientation)value == eliteOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
            
        });
        /// <summary>
        /// 
        /// </summary>
        public eliteOrientation Orientation
        {
            get
            {
                return (eliteOrientation)GetValue(enumOrientationProperty);
            }
            set
            {
                SetValue(enumOrientationProperty, value);
            }
        }

        public void OnColorShapeClick(eliteColorShape colorShape)
        {
            this.ColorShapeClick?.Invoke(colorShape, EventArgs.Empty);
            ColorShapeClickCommand?.Execute(colorShape);
        }
        public event EventHandler ColorShapeClick;

        public static readonly BindableProperty ColorShapeClickCommandProperty = BindableProperty.Create(nameof(ColorShapeClickCommand), typeof(ICommand), typeof(eliteColorCollection));
        public ICommand ColorShapeClickCommand
        {
            get => (ICommand)GetValue(ColorShapeClickCommandProperty);
            set => SetValue(ColorShapeClickCommandProperty, value);
        }
        #endregion

        private void SetContent()
        {
            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.Start;

            this.scrollView = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.stackLayoutContent = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };

            this.scrollView.Content = this.stackLayoutContent;
        }

        public eliteColorCollection()
        {
            this.SetContent();

            this.Content = this.scrollView;
        }

        #region PUBLIC METHODS
        public void AddColorShape(string color)
        {
            ObservableCollection<string> shapes = new ObservableCollection<string>(this.ColorShapes);
            shapes.Add(color);

            this.ColorShapes = shapes;
        }

        public void RemoveColorShape(int index)
        {
            if (this.ColorShapes.Count > index)
            {
                ObservableCollection<string> shapes = new ObservableCollection<string>(this.ColorShapes);
                shapes.RemoveAt(index);

                this.ColorShapes = shapes;
            }
        }

        public List<eliteColorShape> GetColorShapeCollection()
        {
            return this.colorShapeCollection;
        }
        #endregion
    }

    /// <summary>
    /// EliteColorShape Class
    /// </summary>
    public class eliteColorShape : SKCanvasView
    {
        private SKPaint fillPaint;
        private SKPaint outlinePaint;

        private int elementHeightRequest = 50;
        private int elementWidthRequest = 50;

        private float outlineRadius = 0;
        private float innerRadius = 0;

        private Color fillColor;
        private bool isSelected;
        private eliteColorCollection colorCollectionBase;

        private bool isAnimating = false;
        private float animationDuration = 200;
        private float animationTimerTickRate = 10;

        public eliteColorShape(eliteColorCollection colorCollectionBase, Color fillColor)
        {
            this.EnableTouchEvents = true;
            this.Touch += this.eliteColorShapeTouched;

            this.ColorCollectionBase = colorCollectionBase;
            this.FillColor = fillColor;

            Margin = new Thickness(5, 0, 5, 0);
            HeightRequest = elementHeightRequest;
            WidthRequest = elementWidthRequest;

            fillPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = this.FillColor.ToSKColor()
            };

            outlinePaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 7,
                Color = this.FillColor.ToSKColor()
            };
        }

        #region PROPERTIES
        public Color FillColor
        {
            get
            {
                return this.fillColor;
            }
            set
            {
                this.fillColor = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (value != this.isSelected)
                {
                    AnimationSelected(value);
                }

                this.isSelected = value;
            }
        }

        private eliteColorCollection ColorCollectionBase
        {
            get
            {
                return this.colorCollectionBase;
            }
            set
            {
                this.colorCollectionBase = value;
            }
        }

        public bool IsAnimating
        {
            get
            {
                return this.isAnimating;
            }
            set
            {
                this.isAnimating = value;
            }
        }
        #endregion

        #region EVENTS

        private void eliteColorShapeTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:

                    bool preventTouch = false;

                    if (!ColorCollectionBase.IsMultiSelect)
                    {
                        if (!this.isSelected == true)
                        {
                            foreach (eliteColorShape colorShape in ColorCollectionBase.GetColorShapeCollection())
                            {
                                if (colorShape.IsSelected && colorShape != this)
                                {
                                    colorShape.IsSelected = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!IsSelected == true) //SELECT
                        {
                            if (ColorCollectionBase.MaxSelectedItems == 0)
                            {
                                ColorCollectionBase.CurrentSelectedItems++;
                            }
                            else
                            {
                                if (ColorCollectionBase.CurrentSelectedItems >= ColorCollectionBase.MaxSelectedItems)
                                    preventTouch = true;
                                else
                                    ColorCollectionBase.CurrentSelectedItems++;
                            }
                        }
                        else //DESELECT
                        {
                            ColorCollectionBase.CurrentSelectedItems--;
                        }
                    }

                    if (!preventTouch)
                    {
                        IsSelected = !IsSelected;
                    }

                    ColorCollectionBase.OnColorShapeClick(this);
                    break;
            }

            eventArgs.Handled = true;
        }
        #endregion

        #region ANIMATIONS
        private void AnimationSelected(bool isSelected)
        {
            float currentTime = 0.0f;
            IsAnimating = true;

            float radiusMin;
            float radiusMax;
            float radiusDiff;

            switch (ColorCollectionBase.AnimationType)
            {
                case eliteAnimationType.BorderIn:
                    radiusMin = (CanvasSize.Height - 50.0f) / 2;
                    radiusMax = (CanvasSize.Height - 30.0f) / 2;

                    outlineRadius = isSelected ? radiusMin : radiusMax;
                    radiusDiff = radiusMax - radiusMin;

                    Device.StartTimer(TimeSpan.FromMilliseconds(animationTimerTickRate), delegate
                    {
                        currentTime += animationTimerTickRate;

                        if (currentTime <= animationDuration)
                        {
                            float radiusStep = (radiusDiff / animationDuration) * currentTime;

                            if (isSelected)
                            {
                                innerRadius -= radiusStep;

                                if (innerRadius < radiusMin)
                                {
                                    innerRadius = radiusMin;

                                    InvalidateSurface();
                                    IsAnimating = false;
                                    return false;
                                }
                            }
                            else
                            {
                                innerRadius += radiusStep;

                                if (innerRadius > radiusMax)
                                {
                                    innerRadius = radiusMax;

                                    InvalidateSurface();
                                    IsAnimating = false;
                                    return false;
                                }
                            }

                            InvalidateSurface();
                            return true;
                        }

                        IsAnimating = false;
                        return false;
                    });
                    break;

                case eliteAnimationType.BorderOut:
                    radiusMin = (CanvasSize.Height - 30.0f) / 2;
                    radiusMax = (CanvasSize.Height - 10.0f) / 2;

                    outlineRadius = isSelected ? radiusMin : radiusMax;
                    radiusDiff = radiusMax - radiusMin;

                    Device.StartTimer(TimeSpan.FromMilliseconds(animationTimerTickRate), delegate
                    {
                        currentTime += animationTimerTickRate;

                        if (currentTime <= animationDuration)
                        {
                            float radiusStep = (radiusDiff / animationDuration) * currentTime;

                            if (isSelected)
                            {
                                outlineRadius += radiusStep;

                                if (outlineRadius > radiusMax)
                                {
                                    outlineRadius = radiusMax;

                                    InvalidateSurface();
                                    IsAnimating = false;
                                    return false;
                                }
                            }
                            else
                            {
                                outlineRadius -= radiusStep;

                                if (outlineRadius < radiusMin)
                                {
                                    outlineRadius = radiusMin;

                                    InvalidateSurface();
                                    IsAnimating = false;
                                    return false;
                                }
                            }

                            InvalidateSurface();
                            return true;
                        }

                        IsAnimating = false;
                        return false;
                    });
                    break;
            }

            InvalidateSurface();
        }
        #endregion

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            int width = info.Width;
            int height = info.Height;

            canvas.Clear();

            //ONLY UPDATE THE RADIUS IF ITS NOT ANIMATING
            if (!isAnimating && !isSelected)
                innerRadius = (height - 30) / 2;

            switch (ColorCollectionBase.AnimationType)
            {
                case eliteAnimationType.BorderIn: //ALWAYS DRAW THE BORDER AT THE SAME POS LIKE THE CIRCLE
                    canvas.DrawCircle(width / 2, height / 2, (height - 30) / 2, outlinePaint);
                    break;
                case eliteAnimationType.BorderOut: //DRAW THE BORDER ONLY IF ITS SELECTED OR ANIMATING

                    if (isSelected || isAnimating)
                    {
                        canvas.DrawCircle(width / 2, height / 2, outlineRadius, outlinePaint);
                    }
                    break;
            }

            canvas.DrawCircle(width / 2, height / 2, innerRadius, fillPaint);
        }
    }
}
