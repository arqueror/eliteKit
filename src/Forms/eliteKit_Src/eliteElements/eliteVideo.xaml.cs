using eliteKit.eliteCore;
using eliteKit.eliteEnums;
using eliteKit.eliteEventArgs;
using eliteKit.MarkupExtensions;
using eliteKit.Models;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    //TODO Add retry logic and expose error events. Add MoveToNext in case of error flag and Add Videos[] to support playlists
    public partial class eliteVideo : ContentView
    {
        private static Thickness tempMargin;
        private int currentPlayingOrderedItem = 0;
        private double tempCornerRadius;
        private bool isFirstTime = true;
        private bool _IsSetToPopup = false;
        private bool _vimeoVideoCompletedAndNoNext = false;
        private bool _controlsAreVisible = false;
        public event EventHandler<elitePlayerState> OnPlayerStatusChanged;

        public bool IsSetToPopup
        {
            get => _IsSetToPopup;
            private set => _IsSetToPopup = value;
        }

        public Thickness TempMargin
        {
            get => tempMargin;
            private set => tempMargin = value;
        }

        public double TempCornerRadius
        {
            get => tempCornerRadius;
            private set => tempCornerRadius = value;
        }

        public bool AreControlsVisible
        {
            get => VideoPlayerControls.IsVisible;
        }

        #region Properties
        public static readonly BindableProperty CurrentVideoPreferredQualityProperty = BindableProperty.Create(nameof(CurrentVideoPreferredQuality), typeof(eliteVideoQuality), typeof(eliteVideo), eliteVideoQuality.Low, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                var newEnum = (eliteVideoQuality)value;
                if (((eliteVideo)bindableObject).CurrentPlayingVideoQuality == newEnum) return;

                ((eliteVideo)bindableObject).VideoPlayerOverlay.FadeTo(1, 250, Easing.Linear);
                ((eliteVideo)bindableObject).VideoPlayerOverlay.IsVisible = true;

                switch (((eliteVideo)bindableObject).VideoProvider)
                {
                    case eliteVideoProvider.Vimeo:
                        List<eliteVideoQuality> availableQualities = new List<eliteVideoQuality>();
                        eliteVideoQuality currQuality = eliteVideoQuality.Low;
                        ((eliteVideo)bindableObject).VideoPlayer.Source = VimeoVideoIdExtension.Convert(((eliteVideo)bindableObject).VideoSource, newEnum, ref availableQualities, ref currQuality);
                        ((eliteVideo)bindableObject)._currentVideoAvailableQualities = availableQualities;
                        ((eliteVideo)bindableObject)._currentPlayingVideoQuality = currQuality;
                        break;
                    default:
                        ((eliteVideo)bindableObject).VideoPlayer.Source = ((eliteVideo)bindableObject).VideoSource;
                        break;

                }

            }
        });

        public static readonly BindableProperty CornerRoundedProperty = BindableProperty.Create(nameof(CornerRounded), typeof(string), typeof(eliteVideo), "all", validateValue: OnCornerRoundedPropertyValidateValue);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(eliteVideo), Convert.ToDouble(11), propertyChanged: (bindableObject, oldValue, value) =>
        {
           ((eliteVideo)bindableObject).CornerRadius = (double)value;
        });

        public static readonly BindableProperty VideoProviderProperty = BindableProperty.Create(nameof(VideoProvider), typeof(eliteVideoProvider), typeof(eliteVideo), eliteVideoProvider.Default, propertyChanged: (bindableObject, oldValue, value) =>
        {

        });
        public static readonly BindableProperty VideoSourceProperty = BindableProperty.Create(nameof(VideoSource), typeof(string), typeof(eliteVideo), default(string), propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                ((eliteVideo)bindableObject).VideoPlayerOverlay.FadeTo(1, 250, Easing.Linear);
                ((eliteVideo)bindableObject).VideoPlayerOverlay.IsVisible = true;

                switch (((eliteVideo)bindableObject).VideoProvider)
                {
                    case eliteVideoProvider.Vimeo:
                        List<eliteVideoQuality> availableQualities = new List<eliteVideoQuality>();
                        eliteVideoQuality currQuality = eliteVideoQuality.Low;
                        var vimeoSorce = VimeoVideoIdExtension.Convert((string)value, ((eliteVideo)bindableObject).CurrentVideoPreferredQuality, ref availableQualities, ref currQuality);
                        if (vimeoSorce == null)
                        {
                            ((eliteVideo)bindableObject).PlayNextVideo(true);
                        }
                        else
                        {
                            ((eliteVideo)bindableObject).VideoPlayer.Source = vimeoSorce;
                            ((eliteVideo)bindableObject)._currentVideoAvailableQualities = availableQualities;
                            ((eliteVideo)bindableObject)._currentPlayingVideoQuality = currQuality;
                        }
                        break;
                    default:
                        ((eliteVideo)bindableObject).VideoPlayer.Source = (string)value;
                        break;

                }

            }
        });
        public static readonly BindableProperty VideoCollectionProperty = BindableProperty.Create(nameof(VideoCollection), typeof(ObservableCollection<eliteVideoItem>), typeof(eliteVideo), default(ObservableCollection<eliteVideoItem>), propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            ((eliteVideo)bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
        });
        public static readonly BindableProperty SeekSecondsProperty = BindableProperty.Create(nameof(SeekSeconds), typeof(int), typeof(eliteVideo), 10);

        public static readonly BindableProperty ShadowVerticalOffsetProperty = BindableProperty.Create(nameof(ShadowVerticalOffset), typeof(double), typeof(eliteVideo), -1d);
        public static readonly BindableProperty ShadowHorizontalOffsetProperty = BindableProperty.Create(nameof(ShadowHorizontalOffset), typeof(double), typeof(eliteVideo), 1d);
        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create(nameof(ShadowOpacity), typeof(float), typeof(eliteVideo), 0.8f);
        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.Create(nameof(ShadowRadius), typeof(float), typeof(eliteVideo), 3.0f);
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create(nameof(ShadowColor), typeof(Color), typeof(eliteVideo), Color.Black);

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(eliteVideo), Color.Transparent);
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(float), typeof(eliteVideo), 2f);

        public static readonly BindableProperty IsPopupProperty = BindableProperty.Create(nameof(IsPopup), typeof(bool), typeof(eliteVideo), false);
        public static readonly BindableProperty IsLoopEnabledProperty = BindableProperty.Create(nameof(IsLoopEnabled), typeof(bool), typeof(eliteVideo), false, propertyChanged: (bindableObject, oldValue, value) =>
        {

                if (value != null)
                {
                    ((eliteVideo)bindableObject).VideoPlayer.Repeat = (bool)value;
                }
            
        });
        public static readonly BindableProperty IsFullScreenProperty = BindableProperty.Create(nameof(IsFullScreen), typeof(bool), typeof(eliteVideo), false, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    if ((bool)value)
                    {
                        if (((eliteVideo)bindableObject).IsPopup)
                        {
                            ((eliteVideo)bindableObject).IsSetToPopup = true;
                            ((eliteVideo)bindableObject).TempCornerRadius = ((eliteVideo)bindableObject).CornerRadius;
                            ((eliteVideo)bindableObject).CornerRadius = 0;
                            //Restore Popup original position
                            ((eliteVideo)bindableObject).RestorePositionCommand?.Execute(null);
                        }
                        ((eliteVideo)bindableObject).TempMargin = ((eliteVideo)bindableObject).Margin;
                        ((eliteVideo)bindableObject).Margin = 0;

                        var currTop = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Top;
                        var currLeft = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Left;
                        var currRight = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Right;
                        var currBottom = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Bottom;


                        if (Device.RuntimePlatform == Device.iOS)
                            ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin = new Thickness(currLeft, currTop + 60, currRight, currBottom);
                        ((eliteVideo)bindableObject).IsPopup = false;
                        ((eliteVideo)bindableObject).VerticalOptions = LayoutOptions.FillAndExpand;
                        ((eliteVideo)bindableObject).HorizontalOptions = LayoutOptions.FillAndExpand;
                        ((eliteVideo)bindableObject).VideoPlayerControls.IsVisible = false;
                        ((eliteVideo)bindableObject).VideoPlayer.DisplayControls = true;
                        ((eliteVideo)bindableObject).VideoPlayer.FillMode = eliteFillMode.ResizeAspect;

                    }
                    else
                    {
                        if (((eliteVideo)bindableObject).IsSetToPopup)
                        {
                            //If Popup mode before going fullscreen. Restore CornerRadius and update UI
                            ((eliteVideo)bindableObject).CornerRadius = ((eliteVideo)bindableObject).TempCornerRadius;
                            //Restore Popup original position
                            ((eliteVideo)bindableObject).IsPopup = true;
                            ((eliteVideo)bindableObject).RestorePositionCommand?.Execute(null);
                        }
                        var currTop = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Top;
                        var currLeft = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Left;
                        var currRight = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Right;
                        var currBottom = ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin.Bottom;
                        if (Device.RuntimePlatform == Device.iOS)
                            ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.Margin = new Thickness(currLeft, currTop - 60, currRight, currBottom);

                        ((eliteVideo)bindableObject).Margin = ((eliteVideo)bindableObject).TempMargin;
                        ((eliteVideo)bindableObject).VerticalOptions = LayoutOptions.CenterAndExpand;
                        ((eliteVideo)bindableObject).HorizontalOptions = LayoutOptions.CenterAndExpand;
                        ((eliteVideo)bindableObject).VideoPlayer.DisplayControls = false;
                        ((eliteVideo)bindableObject).VideoPlayerControls.IsVisible = true;
                        ((eliteVideo)bindableObject).VideoPlayer.FillMode = eliteFillMode.ResizeAspectFill;



                    }
                    ((eliteVideo)bindableObject).UpdateChildrenLayout();

                }
        });

        public static readonly BindableProperty AutoplayProperty = BindableProperty.Create(nameof(Autoplay), typeof(bool), typeof(eliteVideo), true, propertyChanged: (bindableObject, oldValue, value) =>
        {

                if (value != null)
                {
                    ((eliteVideo)bindableObject).VideoPlayer.AutoPlay = (bool)value;
                }
            
        });
        public static readonly BindableProperty ShowFullScreenButtonProperty = BindableProperty.Create(nameof(ShowFullScreenButton), typeof(bool), typeof(eliteVideo), false, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {

               ((eliteVideo)bindableObject).ShowFullScreenButton = (bool)value;
            }
        });

        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteVideo), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
        {

                if (value != null)
                {
                    ((eliteVideo)bindableObject).VideoPlayerControls.ColorPrimary = (Color)value;
                    ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.ColorPrimary = (Color)value;
                }
            
        });
        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteVideo), coreSettings.ColorSecondary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteVideo)bindableObject).VideoPlayerControls.ColorSecondary = (Color)value;
                    ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.ColorSecondary = (Color)value;
                }
            
        });
        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteVideo), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteVideo)bindableObject).VideoPlayerControls.IsGradient = (bool)value;
                    ((eliteVideo)bindableObject).VideoPlayerControlFullscreen.IsGradient = (bool)value;
                }
            
        });

        /// <summary>
        ///  The primary color of this element
        /// </summary>
        public Color ColorPrimary
        {
            get => (Color)GetValue(ColorPrimaryProperty);
            set => SetValue(ColorPrimaryProperty, value);
        }
        /// <summary>
        ///  The secondary color of this element
        /// </summary>
        public Color ColorSecondary
        {
            get => (Color)GetValue(ColorSecondaryProperty);
            set => SetValue(ColorSecondaryProperty, value);
        }
        /// <summary>
        ///  Define if the controls are painted with a linear gradient. Default value is true. When false, ColorPrimary will be used as painting color.
        /// </summary>
        public bool IsGradient
        {
            get => (bool)GetValue(IsGradientProperty);
            set => SetValue(IsGradientProperty, value);
        }

        private eliteVideoQuality _currentPlayingVideoQuality = eliteVideoQuality.Low;
        /// <summary>
        ///   Current playing video quality
        /// </summary>
        public eliteVideoQuality CurrentPlayingVideoQuality
        {
            get => _currentPlayingVideoQuality;
        }

        /// <summary>
        ///   List of current video source available qualities.
        /// </summary>
        public eliteVideoQuality CurrentVideoPreferredQuality
        {
            get => (eliteVideoQuality)GetValue(CurrentVideoPreferredQualityProperty);
            set => SetValue(CurrentVideoPreferredQualityProperty, value);
        }

        private List<eliteVideoQuality> _currentVideoAvailableQualities = new List<eliteVideoQuality>();
        /// <summary>
        ///   List of current video source available qualities.
        /// </summary>
        public List<eliteVideoQuality> CurrentVideoAvailableQualities
        {
            get => _currentVideoAvailableQualities;
        }

        /// <summary>
        ///  Hide/Show Full Screen button overlay which normal behavior is to expand eliteVideo to parent's container size
        /// </summary>
        public bool ShowFullScreenButton
        {
            get => (bool)GetValue(ShowFullScreenButtonProperty);
            set => SetValue(ShowFullScreenButtonProperty, value);
        }

        /// <summary>
        ///  Specifies seconds to use in Seek function when no VideoElement list is provided. Default value is 10.
        /// </summary>
        public int SeekSeconds
        {
            get => (int)GetValue(SeekSecondsProperty);
            set => SetValue(SeekSecondsProperty, value);
        }

        /// <summary>
        ///  When true, eliteVideo will expand and take parent container size. Default value is false
        /// </summary>
        public bool IsFullScreen
        {
            get => (bool)GetValue(IsFullScreenProperty);
            set => SetValue(IsFullScreenProperty, value);
        }

        /// <summary>
        ///  Videos to play
        /// </summary>
        public ObservableCollection<eliteVideoItem> VideoCollection
        {
            get => (ObservableCollection<eliteVideoItem>)GetValue(VideoCollectionProperty);
            set => SetValue(VideoCollectionProperty, value);
        }
        void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += (sender, e) =>
                {
                    if (e.NewItems != null)
                    {
                        var tempList = new List<eliteVideoItem>();
                        if (e.NewItems != null)
                        {
                            foreach (eliteVideoItem newItem in e.NewItems)
                            {
                                tempList.Add(newItem);
                            }
                            foreach (var newItem in VideoCollection)
                            {
                                var item = (eliteVideoItem)newItem;
                                tempList.Add(item);
                            }

                            VideoCollection = new ObservableCollection<eliteVideoItem>(tempList.OrderBy(x => x.VideoOrder));

                        }
                    }

                };

                if (isFirstTime)
                {
                    var vids = newValue as ObservableCollection<eliteVideoItem>;
                    vids = new ObservableCollection<eliteVideoItem>(vids.OrderBy(x => x.VideoOrder));
                    currentPlayingOrderedItem = 0;
                    if (VideoCollection?.Count > 0)
                    {
                        var nextitemToPlay = VideoCollection[currentPlayingOrderedItem];
                        VideoProvider = nextitemToPlay.VideoProvider;
                        VideoSource = nextitemToPlay.VideoSource;
                    }

                    isFirstTime = false;
                }
            }

        }

        /// <summary>
        /// Current video path. For Youtube and Vimeo current video id.
        /// </summary>
        public string VideoSource
        {
            get => (string)GetValue(VideoSourceProperty);
            set
            {
                SetValue(VideoSourceProperty, value);

            }
        }

        /// <summary>
        ///  Current video provider. Allowed options: Default, Vimeo, Youtube.
        /// </summary>
        public eliteVideoProvider VideoProvider
        {
            get => (eliteVideoProvider)GetValue(VideoProviderProperty);
            set => SetValue(VideoProviderProperty, value);
        }

        /// <summary>
        /// When true, current video will loop when finished playing. Default value is false.
        /// </summary>
        public bool IsLoopEnabled
        {
            get => (bool)GetValue(IsLoopEnabledProperty);
            set => SetValue(IsLoopEnabledProperty, value);
        }

        /// <summary>
        /// When true video will start playing right away after setting VideoSource property. Default value is true.
        /// </summary>
        public bool Autoplay
        {
            get => (bool)GetValue(AutoplayProperty);
            set => SetValue(AutoplayProperty, value);
        }

        /// <summary>
        /// The default value is "all" witch makes all corners rounded. Use none to not set any corner rounded.
        /// To round the corners individually, uses a combination of these values "topleft, topright, bottomleft, bottomright" separated by comma.
        /// </summary>
        public string CornerRounded
        {
            get => (string)GetValue(CornerRoundedProperty);
            set => SetValue(CornerRoundedProperty, value);
        }

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public float BorderThickness
        {
            get => (float)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// Popup shadow color. Only visible when IsPopup property is true.
        ///   iOS. Fully Customizable
        ///   Android. Not customizable and only available for API >= 21 (Lollipop)
        /// </summary>
        public Color ShadowColor
        {
            get => (Color)GetValue(ShadowColorProperty);
            set => SetValue(ShadowColorProperty, value);
        }

        /// <summary>
        /// Popup shadow offset. Only visible when IsPopup property is true.
        ///   iOS. Fully Customizable
        ///   Android. Not customizable and only available for API >= 21 (Lollipop)
        /// </summary>
        public double ShadowVerticalOffset
        {
            get => (double)GetValue(ShadowVerticalOffsetProperty);
            set => SetValue(ShadowVerticalOffsetProperty, value);
        }

        /// <summary>
        /// Popup horizontal shadow color. Only visible when IsPopup property is true.
        ///   iOS. Fully Customizable
        ///   Android. Not customizable and only available for API >= 21 (Lollipop)
        /// </summary>
        public double ShadowHorizontalOffset
        {
            get => (double)GetValue(ShadowHorizontalOffsetProperty);
            set => SetValue(ShadowHorizontalOffsetProperty, value);
        }

        /// <summary>
        /// Popup shadow opacity. Only visible when IsPopup property is true.
        ///   iOS. Fully Customizable
        ///   Android. Not customizable and only available for API >= 21 (Lollipop)
        /// </summary>
        public float ShadowOpacity
        {
            get => (float)GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }

        /// <summary>
        /// Popup shadow radius. Only visible when IsPopup property is true.
        ///   iOS. Fully Customizable
        ///   Android. Not customizable and only available for API >= 21 (Lollipop)
        /// </summary>
        public float ShadowRadius
        {
            get => (float)GetValue(ShadowRadiusProperty);
            set => SetValue(ShadowRadiusProperty, value);
        }

        private static bool OnCornerRoundedPropertyValidateValue(BindableObject bindable, object value)
        {      
            var player = bindable as eliteVideo;
            var allowedValues = new string[] { "topleft", "topright", "bottomleft", "bottomright", "all", "none" };

            if (value.Equals("none"))
            {
                player.CornerRadius = 0;
            }

            return value.ToString().Split(',').Select(x => x.Trim().ToLower())
                                              .All(item => allowedValues.Contains(item));
        }

        /// <summary>
        /// When true, allows eliteVideo to remain in Popup mode.
        /// </summary>
        public bool IsPopup
        {
            get => (bool)GetValue(IsPopupProperty);
            set => SetValue(IsPopupProperty, value);
        }
        #endregion

        #region DraggableMembers
        internal event EventHandler DragStart = delegate { };
        internal event EventHandler DragEnd = delegate { };

        public static readonly BindableProperty DragDirectionProperty = BindableProperty.Create(
            propertyName: "DragDirection",
            returnType: typeof(DragDirectionType),
            declaringType: typeof(eliteVideo),
            defaultValue: DragDirectionType.All,
            defaultBindingMode: BindingMode.TwoWay);

        internal enum DragMod
        {
            Touch,
            LongPress
        }
        internal enum DragDirectionType
        {
            All,
            Vertical,
            Horizontal
        }
        internal DragDirectionType DragDirection
        {
            get { return (DragDirectionType)GetValue(DragDirectionProperty); }
            set { SetValue(DragDirectionProperty, value); }
        }

        public static readonly BindableProperty DragModeProperty = BindableProperty.Create(
           propertyName: "DragMode",
           returnType: typeof(DragMod),
           declaringType: typeof(eliteVideo),
           defaultValue: DragMod.LongPress,
           defaultBindingMode: BindingMode.TwoWay);

        internal DragMod DragMode
        {
            get { return (DragMod)GetValue(DragModeProperty); }
            set { SetValue(DragModeProperty, value); }
        }

        public static readonly BindableProperty IsDraggingProperty = BindableProperty.Create(
          propertyName: "IsDragging",
          returnType: typeof(bool),
          declaringType: typeof(eliteVideo),
          defaultValue: false,
          defaultBindingMode: BindingMode.TwoWay);

        internal bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }

        public static readonly BindableProperty RestorePositionCommandProperty = BindableProperty.Create(nameof(RestorePositionCommand), typeof(ICommand), typeof(eliteVideo), default(ICommand), BindingMode.TwoWay, null, OnRestorePositionCommandPropertyChanged);

        static void OnRestorePositionCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var source = bindable as eliteVideo;
            if (source == null)
            {
                return;
            }
            source.OnRestorePositionCommandChanged();
        }

        private void OnRestorePositionCommandChanged()
        {
            OnPropertyChanged("RestorePositionCommand");
        }

        internal ICommand RestorePositionCommand
        {
            get
            {
                return (ICommand)GetValue(RestorePositionCommandProperty);
            }
            set
            {
                SetValue(RestorePositionCommandProperty, value);
            }
        }

        internal void DragStarted()
        {
            DragStart(this, default(EventArgs));
            IsDragging = true;
        }

        internal void DragEnded()
        {
            IsDragging = false;
            DragEnd(this, default(EventArgs));
        }
        #endregion

        public eliteVideo()
        {
            InitializeComponent();
            VideoPlayerControlFullscreen.OnFullScreenTapped += (s, a) =>
            {
                IsFullScreen = !IsFullScreen;
            };
            _IsSetToPopup = IsPopup;

            if (IsFullScreen)
            {
                this.VerticalOptions = LayoutOptions.FillAndExpand;
                this.HorizontalOptions = LayoutOptions.FillAndExpand;
                VideoPlayerControls.IsVisible = false;
                VideoPlayer.DisplayControls = true;
                VideoPlayer.FillMode = eliteFillMode.Resize;
                UpdateChildrenLayout();
            }
            else
            {
                this.VerticalOptions = LayoutOptions.CenterAndExpand;
                this.HorizontalOptions = LayoutOptions.CenterAndExpand;
                UpdateChildrenLayout();
            }

            this.VideoPlayer.Repeat = IsLoopEnabled;
            this.VideoPlayer.AutoPlay = Autoplay;

            this.VideoPlayer.PlayerStateChanged += videoPlayerStateChanged;

            TapGestureRecognizer tapGestureVideoPlayer = new TapGestureRecognizer();
            tapGestureVideoPlayer.Tapped += this.tapGestureControlsShow;
            tapGestureVideoPlayer.NumberOfTapsRequired = 1;
            this.VideoPlayer.GestureRecognizers.Add(tapGestureVideoPlayer);
            this.VideoPlayerControls.VideoControlsClicked += (eventSender, eventArgs) =>
            {
                this.controlsShow();
            };

            this.VideoPlayerControls.BackNextButton += (s, buttonTapped) =>
            {
                if (buttonTapped == "backward")
                {
                    PlayLastVideo();
                }
                else if (buttonTapped == "forward")
                {
                    PlayNextVideo();
                }
            };

            this.VideoPlayerOverlay.BackgroundColor = Color.FromRgba(0, 0, 0, 100);

            this.VideoPlayerControls.VideoPlay += (eventSender, eventArgs) =>
            {
                //Vimeo videos autoplay themselves. This is a litle hack for stop it doing so
                if (_vimeoVideoCompletedAndNoNext)
                {
                    _vimeoVideoCompletedAndNoNext = false;

                    eliteVideoQuality currQuality = eliteVideoQuality.Low;
                    List<eliteVideoQuality> availableQualities = new List<eliteVideoQuality>();
                    VideoPlayer.Source = VimeoVideoIdExtension.Convert(VideoSource, CurrentVideoPreferredQuality, ref availableQualities, ref currQuality);
                    _currentVideoAvailableQualities = availableQualities;
                    _currentPlayingVideoQuality = currQuality;
                }
                else
                {
                    this.VideoPlayer.Play();

                }
            };

            this.VideoPlayerControls.VideoPause += (eventSender, eventArgs) =>
            {
                this.VideoPlayer.Pause();
            };

        }

        public void PlayLastVideo()
        {
            if (VideoCollection?.Count > 0)
            {
                currentPlayingOrderedItem--;
                if (currentPlayingOrderedItem >= 0)
                {
                    VideoPlayer.Pause();
                    VideoPlayerOverlay.FadeTo(1, 250, Easing.Linear);
                    this.VideoPlayerOverlay.IsVisible = true;
                    var nextitemToPlay = VideoCollection[currentPlayingOrderedItem];
                    if (VideoProvider != nextitemToPlay.VideoProvider)
                        VideoProvider = nextitemToPlay.VideoProvider;
                    if (VideoSource != nextitemToPlay.VideoSource)
                        VideoSource = nextitemToPlay.VideoSource;

                }
                else
                {
                    currentPlayingOrderedItem++;
                }
            }
            else
            {
                VideoPlayer.Seek(SeekSeconds * -1);
            }
        }

        /// <summary>
        /// Use this method to manually seek forward or backward in current video
        /// </summary>
        /// <param name="seconds"></param>
        public void Seek(int seconds)
        {
            VideoPlayer?.Seek(seconds);
        }

        /// <summary>
        /// Use this method to manually pause current video
        /// </summary>
        public void Pause()
        {
            if (VideoPlayer.State == elitePlayerState.Playing)
                VideoPlayer?.Pause();
        }

        /// <summary>
        /// Use this method to manually Play current video
        /// </summary>
        public void Play()
        {
            if (VideoPlayer.State == elitePlayerState.Paused || VideoPlayer.State == elitePlayerState.Prepared || VideoPlayer.State == elitePlayerState.Completed)
                VideoPlayer?.Play();
        }

        public void PlayNextVideo(bool currentSourceFailed = false)
        {

            if (VideoCollection?.Count > 0)
            {
                if (!currentSourceFailed)
                    currentPlayingOrderedItem++;
                else
                {
                    VideoCollection.RemoveAt(currentPlayingOrderedItem);

                    //Reset counter in case all next videos are failing and list only have 1 item left
                    if (VideoCollection?.Count <= 1)
                        currentPlayingOrderedItem = 0;

                    //Last item is invalid. Reset counter to last one that worked
                    if (VideoCollection?.Count <= currentPlayingOrderedItem)
                        currentPlayingOrderedItem--;

                    //Notify the user of this error
                    this.OnPlayerStatusChanged?.Invoke(this, elitePlayerState.SourceFailedToLoad);
                }

                if (currentPlayingOrderedItem < VideoCollection.Count)
                {
                    VideoPlayer.Pause();
                    VideoPlayerOverlay.FadeTo(1, 250, Easing.Linear);
                    this.VideoPlayerOverlay.IsVisible = true;

                    var nextitemToPlay = VideoCollection[currentPlayingOrderedItem];
                    if (VideoProvider != nextitemToPlay.VideoProvider)
                        VideoProvider = nextitemToPlay.VideoProvider;
                    if (VideoSource != nextitemToPlay.VideoSource)
                        VideoSource = nextitemToPlay.VideoSource;

                }
                else
                {
                    currentPlayingOrderedItem--;
                }
            }
            else
            {
                VideoPlayer.Seek(SeekSeconds);
            }
        }

        public void controlsShow()
        {
            if (IsFullScreen)
            {
                VideoPlayerControls.IsVisible = false;
            }
            this.controlsTimerCurrent = 0;

            if (!VideoPlayerControls.IsVisible && !VideoPlayerOverlay.IsVisible && !IsFullScreen)
            {
                VideoPlayerControls.IsVisible = true;
                VideoPlayerControls.FadeTo(1, 250, Easing.Linear);
                this.startControlsTimer();
            }

            if (!VideoPlayerControlFullscreen.IsVisible && ShowFullScreenButton)
            {
                VideoPlayerControlFullscreen.IsVisible = true;
                VideoPlayerControlFullscreen.FadeTo(1, 250, Easing.Linear);
                this.startControlsTimer();
            }
        }

        private void tapGestureControlsShow(object eventSender, EventArgs eventArgs)
        {
            this.controlsShow();
        }

        private int controlsTimerCurrent = 0;
        private bool controlsTimerStarted = false;
        private void startControlsTimer()
        {
            if (!this.controlsTimerStarted)
            {
                this.controlsTimerStarted = true;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    this.controlsTimerCurrent++;

                    if (this.VideoPlayerControls.IsVisible
                    && this.controlsTimerCurrent == 3)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await VideoPlayerControls.FadeTo(0, 250, Easing.Linear);
                            VideoPlayerControls.IsVisible = false;
                        });
                    }

                    if (this.VideoPlayerControlFullscreen.IsVisible
                    && this.controlsTimerCurrent == 3)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await VideoPlayerControlFullscreen.FadeTo(0, 250, Easing.Linear);
                            VideoPlayerControlFullscreen.IsVisible = false;
                        });
                    }

                    if (this.controlsTimerCurrent == 3)
                    {
                        this.controlsTimerStarted = false;
                        return false;
                    }

                    return true;
                });
            }
        }

        private async void videoPlayerStateChanged(object sender, EventArgsVideoPlayerStateChanged eventArgs)
        {
            if (eventArgs.CurrentState == elitePlayerState.Prepared)
            {
                VideoPlayerControls.DurationTime = VideoPlayer.VideoDuration;

                // Play & Pause immediately to show the preview image (otherwise it's black)
                VideoPlayer.Play();
                if (!Autoplay)
                {
                    await Task.Delay(10);
                    VideoPlayer.Pause();
                }

                // Hide the loader
                await this.VideoPlayerOverlay.FadeTo(0, 250, Easing.Linear);
                this.VideoPlayerOverlay.IsVisible = false;

                this.VideoPlayerControls.ToggleButtonState = !VideoPlayer.AutoPlay;
            }

            if (eventArgs.CurrentState == elitePlayerState.Playing)
            {
                VideoPlayerControls.DurationTime = eventArgs.Duration;

                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    if (!VideoPlayerControls.ToggleButtonState)
                        VideoPlayerControls.CurrentTime = VideoPlayer.CurrentTime;
                    return !VideoPlayerControls.ToggleButtonState;
                });

            }

            if (eventArgs.CurrentState == elitePlayerState.Completed)
            {
                VideoPlayerControls.ToggleButtonState = true;

                VideoPlayerControls.CurrentTime = TimeSpan.FromMilliseconds(0);
                VideoPlayer.Pause();

                if (VideoProvider == eliteVideoProvider.Vimeo)
                {
                    if ((VideoCollection == null || VideoCollection?.Count <= 0))
                    {
                        VideoPlayerControls.ToggleButtonState = false;
                        eliteVideoQuality currQuality = eliteVideoQuality.Low;
                        List<eliteVideoQuality> availableQualities = new List<eliteVideoQuality>();
                        if (IsLoopEnabled)
                        {
                            VideoPlayer.Source = VimeoVideoIdExtension.Convert(VideoSource, CurrentVideoPreferredQuality, ref availableQualities, ref currQuality);
                            _currentVideoAvailableQualities = availableQualities;
                            _currentPlayingVideoQuality = currQuality;
                        }
                        else
                        {
                            VideoPlayerControls.ToggleButtonState = true;
                        }
                    }
                }

                //Items in List and Loop is not enabled?, then try to play next video in list
                OnCompletedPlayNextVideo();
            }

            if (eventArgs.CurrentState == elitePlayerState.Error)
            {
                VideoPlayerControls.ToggleButtonState = true;
                VideoPlayerControls.CurrentTime = TimeSpan.FromMilliseconds(0);
                VideoPlayer.Source = null;
                VideoPlayerOverlay.IsVisible = false;

                //Error while playing video?, then go to the last one
                if (VideoCollection?.Count > 0)
                {
                    PlayLastVideo();
                }
            }

            if (eventArgs.CurrentState == elitePlayerState.Idle)
            {

            }

            if (eventArgs.CurrentState == elitePlayerState.Paused)
            {

            }

            if (eventArgs.CurrentState == elitePlayerState.Initialized)
            {

            }

            if (eventArgs.CurrentState == elitePlayerState.Buffering)
            {
                await VideoPlayerOverlay.FadeTo(1, 250, Easing.Linear);
                this.VideoPlayerOverlay.IsVisible = true;
            }

            if (eventArgs.CurrentState == elitePlayerState.FinishedBuffering)
            {
                await VideoPlayerOverlay.FadeTo(0, 250, Easing.Linear);
                this.VideoPlayerOverlay.IsVisible = false;
            }

            this.OnPlayerStatusChanged?.Invoke(this, eventArgs.CurrentState);
        }

        private void OnCompletedPlayNextVideo()
        {
            //Items in List and Loop is not enabled?, then try to play next video in list
            if (VideoCollection?.Count > 0 && !IsLoopEnabled && !IsFullScreen)
            {
                VideoCollection = new ObservableCollection<eliteVideoItem>(VideoCollection.OrderBy(x => x.VideoOrder));
                if (VideoCollection?.Count <= 1)
                    currentPlayingOrderedItem = 0;
                else
                    currentPlayingOrderedItem++;

                if (currentPlayingOrderedItem < VideoCollection.Count && currentPlayingOrderedItem >= 0)
                {
                    VideoSource = null;
                    var nextitemToPlay = VideoCollection[currentPlayingOrderedItem];
                    if (VideoProvider != nextitemToPlay.VideoProvider)
                        VideoProvider = nextitemToPlay.VideoProvider;
                    if (VideoSource != nextitemToPlay.VideoSource)
                        VideoSource = nextitemToPlay.VideoSource;
                }

                //Reached end of list
                else if (currentPlayingOrderedItem + 1 >= VideoCollection.Count)
                {
                    currentPlayingOrderedItem = 0;
                    var nextitemToPlay = VideoCollection[currentPlayingOrderedItem];
                    VideoProvider = nextitemToPlay.VideoProvider;
                    VideoSource = null;
                    VideoSource = nextitemToPlay.VideoSource;

                    // Hide the loader
                    this.VideoPlayerOverlay.FadeTo(0, 250, Easing.Linear);
                    this.VideoPlayerOverlay.IsVisible = false;
                    return;
                }


            }
        }
    }

    internal class eliteVideoControlsShape : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;

        private int toggleButtonSize = 125;
        private bool toggleButtonState = true;
        public bool ToggleButtonState
        {
            get
            {
                return this.toggleButtonState;
            }
            set
            {
                this.toggleButtonState = value;
                this.InvalidateSurface();
            }
        }

        private SKPoint toggleButtonLocation;

        private int switchButtonSize = 100;

        private int currentBarWidth = 0;
        private int maximalBarWidth = 0;

        private int offsetVertical = 30; // Top & Bottom padding
        private int offsetHorizontal = 30; // Left & Right padding

        private Color colorPrimary = coreSettings.ColorPrimary;
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

        private Color colorSecondary = coreSettings.ColorSecondary;
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

        private bool isGradient = true;
        public bool IsGradient
        {
            get
            {
                return this.isGradient;
            }
            set
            {
                this.isGradient = value;
                this.InvalidateSurface();
            }
        }

        private TimeSpan currentTime = TimeSpan.FromMilliseconds(0);
        public TimeSpan CurrentTime
        {
            get
            {
                return this.currentTime;
            }
            set
            {
                this.currentTime = value;
                this.InvalidateSurface();
            }
        }

        private TimeSpan durationTime = TimeSpan.FromMilliseconds(1);
        public TimeSpan DurationTime
        {
            get
            {
                return this.durationTime;
            }
            set
            {
                this.durationTime = value;
                this.InvalidateSurface();
            }
        }

        public event EventHandler VideoPlay;
        private void OnVideoPlay()
        {
            this.VideoPlay?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler VideoPause;
        private void OnVideoPause()
        {
            this.VideoPause?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<string> BackNextButton;
        private void OnBackNextButton(string buttonTapped)
        {
            BackNextButton?.Invoke(this, buttonTapped);
        }

        public event EventHandler<string> VideoControlsClicked;
        private void OnVideoControlsClicked()
        {
            this.VideoControlsClicked?.Invoke(this, null);
        }

        // Make the audio control buttons round
        public bool roundButtons = true;

        // Display Previous & Next controls
        public bool displaySwitchButtons = true;

        public eliteVideoControlsShape()
        {
            this.IsVisible = false;
            this.Opacity = 0;

            this.EnableTouchEvents = true;
            this.Touch += eliteVideoControlsShapeTouched;
        }

        private decimal getCurrentBarWidth()
        {
            try
            {
                int currentTime = (int)this.currentTime.TotalMilliseconds;
                int durationTime = (int)this.durationTime.TotalMilliseconds;

                decimal oneSecondInPercent = 100m / durationTime;
                decimal currentPositionInPercent = oneSecondInPercent * currentTime;

                decimal onePercentInPixel = this.maximalBarWidth / 100m;
                decimal currentPositionInPixel = onePercentInPixel * currentPositionInPercent;

                return currentPositionInPixel;
            }
            catch
            {
                return 0;
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;
            string buttonIcon = this.toggleButtonState ? "\uf04b" : "\uf04c";
            int buttonRadius = this.roundButtons ? this.toggleButtonSize / 2 : this.toggleButtonSize / 6;

            SKRect rectBackground = new SKRect(0, 0, this.canvasWidth, this.canvasHeight);
            SKPaint paintBackground = new SKPaint()
            {
                Color = this.colorPrimary.ToSKColor().WithAlpha(200),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            if (this.IsGradient)
                paintBackground.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(rectBackground.Left, rectBackground.Top),
                    new SKPoint(rectBackground.Right, rectBackground.Bottom),
                    new SKColor[] {
                        this.colorPrimary.ToSKColor(),
                        this.colorSecondary.ToSKColor()
                    },
                    new float[] {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

            givenCanvas.DrawRect(rectBackground, paintBackground);

            float roundRectToggleButtonLeft = this.offsetHorizontal;
            float roundRectToggleButtonRight = this.toggleButtonSize;
            float roundRectToggleButtonTop = (this.canvasHeight / 2) + (this.toggleButtonSize / 2) - (this.offsetVertical / 2);
            float roundRectToggleButtonBottom = (this.canvasHeight / 2) - (this.toggleButtonSize / 2) + (this.offsetVertical / 2);

            SKPaint paintToggleButton = new SKPaint()
            {
                Color = Color.FromRgba(0, 0, 0, 75).ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            SKPaint paintBar = new SKPaint()
            {
                Color = Color.FromRgba(255, 255, 255, 150).ToSKColor(),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 30
            };

            SKPaint paintBarCurrent = new SKPaint()
            {
                Color = Color.White.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = 30
            };

            SKPaint paintIcon = new SKPaint()
            {
                TextSize = 40f,
                IsAntialias = true,
                Color = Color.White.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf")
            };
            SKRect rectIcon = new SKRect();
            paintIcon.MeasureText(buttonIcon, ref rectIcon);

            // Switch to previous / next song buttons
            SKPaint paintSwitchButton = new SKPaint()
            {
                Color = Color.FromRgba(0, 0, 0, 75).ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            string iconPrevious = "\uf04a";
            string iconNext = "\uf04e";

            SKPaint paintSwitchIcon = new SKPaint()
            {
                TextSize = 30f,
                IsAntialias = true,
                Color = Color.White.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf")
            };

            SKRect rectPreviousIcon = new SKRect();
            paintSwitchIcon.MeasureText(iconPrevious, ref rectPreviousIcon);

            SKRect rectNextIcon = new SKRect();
            paintSwitchIcon.MeasureText(iconNext, ref rectNextIcon);

            float previousButtonLeft = this.offsetHorizontal;
            float previousButtonTop = (this.canvasHeight / 2) + (this.switchButtonSize / 2) - (this.offsetVertical / 2);
            float previousButtonRight = this.switchButtonSize;
            float previousButtonBottom = (this.canvasHeight / 2) - (this.switchButtonSize / 2) + (this.offsetVertical / 2);

            float nextButtonLeft = this.canvasWidth - this.switchButtonSize;
            float nextButtonTop = (this.canvasHeight / 2) + (this.switchButtonSize / 2) - (this.offsetVertical / 2);
            float nextButtonRight = this.canvasWidth - this.offsetHorizontal;
            float nextButtonBottom = (this.canvasHeight / 2) - (this.switchButtonSize / 2) + (this.offsetVertical / 2);

            SKRoundRect roundRectPreviousButton = new SKRoundRect(
                new SKRect(
                    previousButtonLeft,
                    previousButtonTop,
                    previousButtonRight,
                    previousButtonBottom
                ),
                this.roundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6,
                this.roundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6
            );

            SKRoundRect roundRectNextButton = new SKRoundRect(
                new SKRect(
                    nextButtonLeft,
                    nextButtonTop,
                    nextButtonRight,
                    nextButtonBottom
                ),
                this.roundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6,
                this.roundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6
            );

            if (this.displaySwitchButtons)
            {
                roundRectToggleButtonLeft = roundRectPreviousButton.Rect.Left + roundRectPreviousButton.Width + this.offsetHorizontal;
                roundRectToggleButtonRight = roundRectPreviousButton.Rect.Left + roundRectPreviousButton.Width + this.toggleButtonSize;

                givenCanvas.DrawRoundRect(roundRectPreviousButton, paintSwitchButton);
                givenCanvas.DrawRoundRect(roundRectNextButton, paintSwitchButton);

                givenCanvas.DrawText(iconPrevious, roundRectPreviousButton.Rect.Left + (roundRectPreviousButton.Width / 2), (this.canvasHeight / 2) - rectPreviousIcon.MidY, paintSwitchIcon);
                givenCanvas.DrawText(iconNext, roundRectNextButton.Rect.Left + (roundRectNextButton.Width / 2), (this.canvasHeight / 2) - rectNextIcon.MidY, paintSwitchIcon);
            }

            SKRoundRect roundRectToggleButton = new SKRoundRect(
                new SKRect(
                    roundRectToggleButtonLeft,
                    roundRectToggleButtonTop,
                    roundRectToggleButtonRight,
                    roundRectToggleButtonBottom
                ),
                buttonRadius,
                buttonRadius
            );

            if (!this.displaySwitchButtons)
            {
                this.maximalBarWidth = this.canvasWidth - (this.offsetHorizontal * 2) - this.toggleButtonSize;
                this.currentBarWidth = this.toggleButtonSize + this.offsetHorizontal + (int)this.getCurrentBarWidth();
            }
            else
            {
                this.maximalBarWidth = this.canvasWidth - (this.offsetHorizontal * 2) - this.toggleButtonSize - this.switchButtonSize * 2;
                this.currentBarWidth = this.switchButtonSize + this.offsetHorizontal + this.toggleButtonSize + (int)this.getCurrentBarWidth();
            }

            float progressBarY = this.canvasHeight / 2;

            float progressBarStartX = roundRectToggleButton.Width + (this.offsetHorizontal * 2);
            float progressBarEndX = this.canvasWidth - this.offsetHorizontal;
            float progressBarCurrentStartX = progressBarStartX;
            float progressBarCurrentEndX = this.currentBarWidth;

            if (this.displaySwitchButtons)
            {
                progressBarStartX = roundRectToggleButton.Rect.Left + roundRectToggleButton.Width + this.offsetHorizontal;
                progressBarEndX = this.canvasWidth - this.offsetHorizontal - roundRectNextButton.Width - this.offsetHorizontal;
                progressBarCurrentStartX = progressBarStartX;
            }

            givenCanvas.DrawLine(
                new SKPoint(progressBarStartX, progressBarY),
                new SKPoint(progressBarEndX, progressBarY),
                paintBar
            );
            givenCanvas.DrawLine(
                new SKPoint(progressBarCurrentStartX, progressBarY),
                new SKPoint(progressBarCurrentEndX, progressBarY),
                paintBarCurrent
            );

            float buttonToggleIconX = roundRectToggleButton.Rect.Left + (roundRectToggleButton.Width / 2);
            float buttonToggleIconY = (this.canvasHeight / 2) - rectIcon.MidY;

            givenCanvas.DrawRoundRect(roundRectToggleButton, paintToggleButton);
            givenCanvas.DrawText(buttonIcon, buttonToggleIconX, buttonToggleIconY, paintIcon);

            this.toggleButtonLocation = new SKPoint(
                roundRectToggleButtonLeft,
                this.offsetVertical
            );
        }

        private void eliteVideoControlsShapeTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            SKPoint touchLocation = eventArgs.Location;
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        if ((touchLocation.X >= this.toggleButtonLocation.X && touchLocation.X <= (this.toggleButtonLocation.X + this.toggleButtonSize - this.offsetHorizontal))
                            && (touchLocation.Y >= this.toggleButtonLocation.Y && touchLocation.Y <= (this.toggleButtonLocation.Y + this.toggleButtonSize - this.offsetVertical)))
                        {
                            this.toggleButtonState = !this.toggleButtonState;

                            if (this.toggleButtonState)
                                this.OnVideoPause();
                            else
                                this.OnVideoPlay();
                            this.InvalidateSurface();
                        }
                        else if ((touchLocation.X <= 200 && touchLocation.X <= (this.toggleButtonLocation.X + this.toggleButtonSize - this.offsetHorizontal))
                            && (touchLocation.Y >= this.toggleButtonLocation.Y && touchLocation.Y <= (this.toggleButtonLocation.Y + this.toggleButtonSize - this.offsetVertical)))
                        {

                            OnBackNextButton("backward");
                            this.InvalidateSurface();
                        }

                        else if ((touchLocation.X > (this.canvasWidth - 100) && (touchLocation.X < (this.canvasWidth - 40)))
                            && (touchLocation.Y >= this.toggleButtonLocation.Y && touchLocation.Y <= (this.toggleButtonLocation.Y + this.toggleButtonSize - this.offsetVertical)))
                        {

                            OnBackNextButton("forward");
                            this.InvalidateSurface();
                        }
                    }
                    break;
            }

            eventArgs.Handled = true;
            this.OnVideoControlsClicked();
        }

        public SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("eliteKit.eliteFonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }
    }

    internal class eliteVideoControlFullscreenShape : SKCanvasView
    {
        private int canvasWidth;
        private int canvasHeight;
        public event EventHandler OnFullScreenTapped;

        private Color colorPrimary = coreSettings.ColorPrimary;
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

        private Color colorSecondary = coreSettings.ColorSecondary;
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

        private bool isGradient = true;
        public bool IsGradient
        {
            get
            {
                return this.isGradient;
            }
            set
            {
                this.isGradient = value;
                this.InvalidateSurface();
            }
        }

        public eliteVideoControlFullscreenShape()
        {
            this.IsVisible = false;
            this.Opacity = 0;

            this.EnableTouchEvents = true;
            this.Touch += eliteVideoControlFullscreenShapeTouched;
        }

        private void eliteVideoControlFullscreenShapeTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        OnFullScreenTapped?.Invoke(this, null);
                    }
                    break;
            }

            eventArgs.Handled = true;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;
            SKRoundRect roundRectBackground = new SKRoundRect(
                new SKRect(
                    0,
                    0,
                    this.canvasWidth,
                    this.canvasHeight
                ),
                10,
                10
            );

            SKPaint paintBackground = new SKPaint()
            {
                Color = this.colorPrimary.ToSKColor().WithAlpha(200),
                Style = SKPaintStyle.Fill,
                IsAntialias = false
            };

            if (this.IsGradient)
                paintBackground.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(roundRectBackground.Rect.Left, roundRectBackground.Rect.Top),
                    new SKPoint(roundRectBackground.Rect.Right, roundRectBackground.Rect.Bottom),
                    new SKColor[] {
                        this.colorPrimary.ToSKColor(),
                        this.colorSecondary.ToSKColor()
                    },
                    new float[] {
                        0,
                        1
                    },
                    SKShaderTileMode.Repeat
                );

            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);

            string iconFullscreen = "\uf065";
            SKPaint paintIconFullscreen = new SKPaint()
            {
                TextSize = 40f,
                IsAntialias = true,
                Color = Color.White.ToSKColor(),
                TextAlign = SKTextAlign.Center,
                Typeface = this.GetTypeface("fa-solid-900.ttf")
            };

            SKRect rectIconFullscreen = new SKRect();
            paintIconFullscreen.MeasureText(iconFullscreen, ref rectIconFullscreen);

            givenCanvas.DrawText(iconFullscreen, (this.canvasWidth / 2), (this.canvasHeight / 2) - rectIconFullscreen.MidY, paintIconFullscreen);
        }

        public SKTypeface GetTypeface(string fullFontName)
        {
            SKTypeface result;

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("eliteKit.eliteFonts." + fullFontName);
            if (stream == null)
                return null;

            result = SKTypeface.FromStream(stream);
            return result;
        }
    }
}
