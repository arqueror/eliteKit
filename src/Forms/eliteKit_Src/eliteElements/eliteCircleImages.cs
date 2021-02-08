using eliteKit.eliteEnums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteCircleImages : ContentView
    {
        private ScrollView scrollView;
        private StackLayout stackLayoutContent;
        private List<eliteCircleImageShape> circleImageShapes;

        public eliteCircleImages()
        {
            this.SetContent();
            this.Content = this.scrollView;
        }

        #region PROPERTIES
        public static readonly BindableProperty ObservableCollectionCircleImagesProperty = BindableProperty.Create(nameof(CircleImages), typeof(ObservableCollection<ImageSource>), typeof(eliteCircleImages), new ObservableCollection<ImageSource>(), BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteCircleImages)bindableObject).circleImageShapes = new List<eliteCircleImageShape>();
                ((eliteCircleImages)bindableObject).stackLayoutContent.Children.Clear();

                var newCollection = value as ObservableCollection<ImageSource>;
                foreach (ImageSource circleImage in newCollection)
                {
                    eliteCircleImageShape imageShape = new eliteCircleImageShape(((eliteCircleImages)bindableObject), circleImage);
                    ((eliteCircleImages)bindableObject).stackLayoutContent.Children.Add(imageShape);

                    ((eliteCircleImages)bindableObject).circleImageShapes.Add(imageShape);
                }
                ((eliteCircleImages)bindableObject).UpdateDimension();
            }
        });
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ImageSource> CircleImages
        {
            get
            {
                return (ObservableCollection<ImageSource>)GetValue(ObservableCollectionCircleImagesProperty);
            }
            set
            {
                SetValue(ObservableCollectionCircleImagesProperty, value);
            }
        }

        public static readonly BindableProperty eliteCircleImageShapeSelectedImageShapeProperty = BindableProperty.Create(nameof(SelectedImageShape), typeof(eliteCircleImageShape), typeof(eliteCircleImages), null, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public eliteCircleImageShape SelectedImageShape
        {
            get
            {
                return (eliteCircleImageShape)GetValue(eliteCircleImageShapeSelectedImageShapeProperty);
            }
            set
            {
                SetValue(eliteCircleImageShapeSelectedImageShapeProperty, value);
            }
        }

        public static readonly BindableProperty intCircleDiameterProperty = BindableProperty.Create(nameof(CircleDiameter), typeof(int), typeof(eliteCircleImages), 50, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {

                foreach (eliteCircleImageShape imageShape in ((eliteCircleImages)bindableObject).circleImageShapes)
                {
                    imageShape.SetCircleDiameter((int)value);
                }
                 ((eliteCircleImages)bindableObject).UpdateDimension();
            
        });
        /// <summary>
        /// 
        /// </summary>
        public int CircleDiameter
        {
            get
            {
                return (int)GetValue(intCircleDiameterProperty);
            }
            set
            {

                SetValue(intCircleDiameterProperty, value);

            }
        }

        public static readonly BindableProperty floatAnimationDurationProperty = BindableProperty.Create(nameof(AnimationDuration), typeof(float), typeof(eliteCircleImages), 100.0f, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != floatAnimationDurationProperty.DefaultValue)
                {
                    ((eliteCircleImages)bindableObject).AnimationDuration = (float)floatAnimationDurationProperty.DefaultValue;
                    return;
                }
            
        });
        /// <summary>
        /// 
        /// </summary>
        public float AnimationDuration
        {
            get
            {
                return (float)GetValue(floatAnimationDurationProperty);
            }
            set
            {
                SetValue(floatAnimationDurationProperty, value);
            }
        }


        public static readonly BindableProperty boolCollapseAbleProperty = BindableProperty.Create(nameof(Collapsible), typeof(bool), typeof(eliteCircleImages), true, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public bool Collapsible
        {
            get
            {
                return (bool)GetValue(boolCollapseAbleProperty);
            }
            set
            {
                SetValue(boolCollapseAbleProperty, value);
            }
        }

        public static readonly BindableProperty enumOrientationProperty = BindableProperty.Create(nameof(ImageOrientation), typeof(eliteOrientation), typeof(eliteCircleImages), eliteOrientation.Horizontal, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteCircleImages)bindableObject).scrollView.Orientation = (eliteOrientation)value == eliteOrientation.Vertical ? ScrollOrientation.Vertical : ScrollOrientation.Horizontal;
                ((eliteCircleImages)bindableObject).stackLayoutContent.Orientation = (eliteOrientation)value == eliteOrientation.Vertical ? StackOrientation.Vertical : StackOrientation.Horizontal;
                ((eliteCircleImages)bindableObject).UpdateDimension();
            }
        });
        /// <summary>
        /// 
        /// </summary>
        public eliteOrientation ImageOrientation
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

        public void OnCircleImageClick(eliteCircleImageShape circleImage)
        {
            this.CircleImageClick?.Invoke(circleImage, EventArgs.Empty);
            CircleImageClickCommand?.Execute(circleImage);
        }
        public event EventHandler CircleImageClick;

        public static readonly BindableProperty CircleImageClickCommandProperty = BindableProperty.Create(nameof(CircleImageClickCommand), typeof(ICommand), typeof(eliteCircleImages));
        public ICommand CircleImageClickCommand
        {
            get => (ICommand)GetValue(CircleImageClickCommandProperty);
            set => SetValue(CircleImageClickCommandProperty, value);
        }
        #endregion

        private void SetContent()
        {
            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.Start;

            this.scrollView = new ScrollView()
            {
                Orientation = this.ImageOrientation == eliteOrientation.Vertical ? ScrollOrientation.Vertical : ScrollOrientation.Horizontal,
            };

            this.stackLayoutContent = new StackLayout()
            {
                Orientation = this.ImageOrientation == eliteOrientation.Vertical ? StackOrientation.Vertical : StackOrientation.Horizontal,
                Spacing = 0,
                Padding = 0,
                Margin = 0,
            };

            this.scrollView.Content = this.stackLayoutContent;
        }

        #region PUBLIC METHODS

        public void AddCircleImage(ImageSource imageSource)
        {
            if (imageSource != null)
            {
                eliteCircleImageShape imageShape = new eliteCircleImageShape(this, imageSource);
                this.stackLayoutContent.Children.Add(imageShape);

                this.CircleImages.Add(imageSource);
                this.circleImageShapes.Add(imageShape);

                this.UpdateDimension();
            }
        }

        public void RemoveCircleImage(int index)
        {
            if (this.stackLayoutContent.Children.Count > index && this.circleImageShapes.Count > index)
            {
                this.stackLayoutContent.Children.RemoveAt(index);

                eliteCircleImageShape imageShape = this.circleImageShapes[index];

                this.CircleImages.Remove(imageShape.CircleImageSource);
                this.circleImageShapes.Remove(imageShape);

                this.UpdateDimension();
            }
        }

        public List<eliteCircleImageShape> GetCircleImageShapes()
        {
            return this.circleImageShapes;
        }

        #endregion

        private void UpdateDimension()
        {
            if (this.circleImageShapes == null || this.circleImageShapes.Count == 0)
                return;

            if (ImageOrientation == eliteOrientation.Horizontal)
            {
                float widthRequest = this.CircleDiameter * this.circleImageShapes.Count;
                scrollView.WidthRequest = widthRequest;
                scrollView.HeightRequest = -1;
            }
            else if (ImageOrientation == eliteOrientation.Vertical)
            {
                float heightRequest = this.CircleDiameter * this.circleImageShapes.Count;
                scrollView.HeightRequest = heightRequest;
                scrollView.WidthRequest = -1;
            }

            this.ResetCircleImageShapes();
        }

        /// <summary>
        /// Reset the Margin of the Element Depending on the ImageOrientation
        /// Vertical -> -Margin at the bottom
        /// Horizontal -> -Margin at the right side
        /// </summary>
        private void ResetCircleImageShapes()
        {
            foreach (eliteCircleImageShape imageShape in this.circleImageShapes)
            {
                imageShape.IsSelected = false;
                Thickness margin = new Thickness();

                if (this.ImageOrientation == eliteOrientation.Horizontal)
                {
                    margin.Right = -(this.CircleDiameter / 4);
                }
                else if (this.ImageOrientation == eliteOrientation.Vertical)
                {
                    margin.Bottom = -(this.CircleDiameter / 4);
                }

                imageShape.SetFrameMargin(margin);
            }
        }
    }

    public class eliteCircleImageShape : ContentView
    {
        private Frame frameImage;
        private Image imageCircle;

        private eliteCircleImages eliteCircleImage;

        private float animationDuration;
        private float animationTimerTickRate = 5;

        public eliteCircleImageShape(eliteCircleImages eliteCircleImage, ImageSource imageSource)
        {
            this.EliteCircleImage = eliteCircleImage;
            this.CircleImageSource = imageSource;

            this.animationDuration = eliteCircleImage.AnimationDuration;
            this.animationTimerTickRate = 5;

            this.SetContent();
        }

        #region PROPERTIES
        public eliteCircleImages EliteCircleImage
        {
            get
            {
                return this.eliteCircleImage;
            }
            set
            {
                this.eliteCircleImage = value;
            }
        }

        public readonly BindableProperty ImageSourceCircleImageSourceProperty = BindableProperty.Create(nameof(CircleImageSource), typeof(ImageSource), typeof(eliteCircleImageShape), null, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public ImageSource CircleImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceCircleImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceCircleImageSourceProperty, value);
            }
        }

        public readonly BindableProperty boolIsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(eliteCircleImageShape), false, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return (bool)GetValue(boolIsSelectedProperty);
            }
            set
            {
                if (value != this.IsSelected)
                {
                    AnimationSelected(value);
                }

                SetValue(boolIsSelectedProperty, value);
            }
        }

        public readonly BindableProperty boolIsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating), typeof(bool), typeof(eliteCircleImageShape), false, BindingMode.TwoWay, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

            }
        });
        /// <summary>
        /// 
        /// </summary>
        public bool IsAnimating
        {
            get
            {
                return (bool)GetValue(boolIsAnimatingProperty);
            }
            set
            {
                SetValue(boolIsAnimatingProperty, value);
            }
        }
        #endregion

        #region EVENTS
        private void OnTapped(object sender, EventArgs args)
        {
            bool select = true;

            eliteCircleImageShape selectedShape = this.EliteCircleImage.SelectedImageShape;

            if (selectedShape == this)
            {
                selectedShape.IsSelected = !selectedShape.IsSelected;
                select = false;
            }

            if (select)
            {
                if (selectedShape != null)
                    selectedShape.IsSelected = false;

                this.EliteCircleImage.SelectedImageShape = this;

                this.IsSelected = !this.IsSelected;
            }

            this.EliteCircleImage.OnCircleImageClick(this);
        }
        #endregion

        #region ANIMATIONS
        private void AnimationSelected(bool isSelected)
        {
            if (!this.EliteCircleImage.Collapsible)
                return;

            float currentTime = 0.0f;
            bool isFirstShape = (this.EliteCircleImage.GetCircleImageShapes()[0] == this);

            IsAnimating = true;

            float marginAnimateTo = this.EliteCircleImage.CircleDiameter / 4;
            float selectedOffset = this.EliteCircleImage.CircleDiameter / 10;

            if (animationDuration == 0)
            {
                Thickness margin = this.frameImage.Margin;

                switch (this.EliteCircleImage.ImageOrientation)
                {
                    case eliteOrientation.Horizontal:

                        if (isSelected)
                        {
                            margin.Right = selectedOffset;

                            if (!isFirstShape)
                                margin.Left = marginAnimateTo + selectedOffset;
                        }
                        else
                        {
                            margin.Right = -marginAnimateTo;

                            if (!isFirstShape)
                                margin.Left = 0;

                        }
                        break;

                    case eliteOrientation.Vertical:

                        if (isSelected)
                        {
                            margin.Bottom = selectedOffset;

                            if (!isFirstShape)
                                margin.Top = marginAnimateTo + selectedOffset;

                        }
                        else
                        {
                            margin.Bottom = -marginAnimateTo;

                            if (!isFirstShape)
                                margin.Top = 0;
                        }

                        break;
                }

                this.frameImage.Margin = margin;
                IsAnimating = false;

                return;
            }

            float marginStep = ((marginAnimateTo + selectedOffset) / animationDuration) * animationTimerTickRate;
            Device.StartTimer(TimeSpan.FromMilliseconds(animationTimerTickRate), delegate
            {
                currentTime += animationTimerTickRate;
                if (currentTime <= animationDuration)
                {
                    Thickness margin = this.frameImage.Margin;

                    switch (this.EliteCircleImage.ImageOrientation)
                    {
                        //ANIMATION HORIZONTAL
                        case eliteOrientation.Horizontal:
                            if (isSelected)
                            {
                                margin.Right = margin.Right + +marginStep;

                                if (!isFirstShape)
                                    margin.Left = margin.Left + +marginStep;
                            }
                            else
                            {
                                margin.Right = margin.Right - +marginStep;

                                if (!isFirstShape)
                                    margin.Left = margin.Left - +marginStep;
                            }

                            break;
                        //ANIMATION VERTICAL
                        case eliteOrientation.Vertical:

                            if (isSelected)
                            {
                                margin.Bottom = margin.Bottom + +marginStep;

                                if (!isFirstShape)
                                    margin.Top = margin.Top + +marginStep;
                            }
                            else
                            {
                                margin.Bottom = margin.Bottom - +marginStep;

                                if (!isFirstShape)
                                    margin.Top = margin.Top - +marginStep;
                            }

                            break;
                    }

                    this.frameImage.Margin = margin;
                    return true;
                }

                IsAnimating = false;
                return false;
            });
        }
        #endregion

        #region CONTENT
        private void SetContent()
        {
            int circleDiameter = this.EliteCircleImage.CircleDiameter;

            Thickness frameMargin = new Thickness();
            switch (this.EliteCircleImage.ImageOrientation)
            {
                case eliteOrientation.Horizontal:
                    frameMargin.Right = -(circleDiameter / 4);
                    break;
                case eliteOrientation.Vertical:
                    frameMargin.Bottom = -(circleDiameter / 4);
                    break;
            }

            this.frameImage = new Frame()
            {
                WidthRequest = circleDiameter,
                HeightRequest = circleDiameter,
                Padding = 0,
                CornerRadius = circleDiameter,
                IsClippedToBounds = true,
                Margin = frameMargin
            };

            this.imageCircle = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = circleDiameter,
                HeightRequest = circleDiameter,
                Aspect = Aspect.AspectFill,
                Source = this.CircleImageSource,
            };

            TapGestureRecognizer tapGesture = new TapGestureRecognizer();
            this.imageCircle.GestureRecognizers.Add(tapGesture);
            tapGesture.Tapped += (eventSender, eventArgs) =>
            {
                this.OnTapped(this, eventArgs);
            };

            this.frameImage.Content = this.imageCircle;
            this.Content = this.frameImage;
        }
        #endregion

        #region PUBLIC METHODS
        public void SetFrameMargin(Thickness margin)
        {
            this.frameImage.Margin = margin;
        }

        public void SetCircleDiameter(int circleDiameter)
        {
            this.imageCircle.WidthRequest = circleDiameter;
            this.imageCircle.HeightRequest = circleDiameter;

            this.frameImage.WidthRequest = circleDiameter;
            this.frameImage.HeightRequest = circleDiameter;
            this.frameImage.CornerRadius = circleDiameter;
        }
        #endregion
    }
}
