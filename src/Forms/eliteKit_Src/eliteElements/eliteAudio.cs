using eliteKit.eliteCore;
using eliteKit.eliteDependencies;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace eliteKit.eliteElements
{
    public class eliteAudioItem
    {
        public string AudioTitle { get; set; }
        public string AudioSource { get; set; }
    }

   public class eliteAudio : SKCanvasView
    {
        private int canvasHeight;
        private int canvasWidth;

        private int toggleButtonSize = 125;
        private bool toggleButtonState = true;

        private int switchButtonSize = 100;

        private SKPoint toggleButtonLocation;
        private SKRect backwardButtonLocation;
        private SKRect forwardButtonLocation;

        private int currentBarWidth = 0;
        private int maximalBarWidth = 0;

        private int offsetVertical = 30; // Top & Bottom padding
        private int offsetHorizontal = 30; // Left & Right padding

        private string audioTitle = "Alan Walker - Force";
        private string audioTime = "00:00 / 00:00";
        private string audioTimeDuration = "00:00 / 00:00";

        private bool playerLoaded = false;
        private bool playerPlaylist = false;
        private int playerCurrent = 1;
        private bool playerPrepared = false;
        private int playerCount = 0;

        private bool isFirstTime = true;

        public static readonly BindableProperty AudioCollectionProperty = BindableProperty.Create(nameof(AudioCollection), typeof(ObservableCollection<eliteAudioItem>), typeof(eliteAudio), default(ObservableCollection<eliteAudioItem>), propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            ((eliteAudio)bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
        });
        public ObservableCollection<eliteAudioItem> AudioCollection
        {
            get => (ObservableCollection<eliteAudioItem>)GetValue(AudioCollectionProperty);
            set => SetValue(AudioCollectionProperty, value);

        }
        void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += (sender, e) =>
                {
                    if (e.NewItems != null)
                    {
                        var tempList = new List<eliteAudioItem>();
                        if (e.NewItems != null)
                        {
                            foreach (eliteAudioItem newItem in e.NewItems)
                            {
                                tempList.Add(newItem);
                            }

                            foreach (var newItem in AudioCollection)
                            {
                                var item = (eliteAudioItem)newItem;
                                tempList.Add(item);
                            }

                            AudioCollection = new ObservableCollection<eliteAudioItem>(tempList);
                            if (AudioCollection?.Count > 1)
                            {
                                this.playerPlaylist = true;
                                this.DisplaySwitchButtons = true;
                                this.InvalidateSurface();
                            }
                        }
                    }

                };
                if (isFirstTime)
                {
                    if (AudioCollection?.Count > 1)
                    {
                        this.playerPlaylist = true;
                        this.DisplaySwitchButtons = true;
                        this.InvalidateSurface();
                    }

                    isFirstTime = false;
                }
            }
        }

        // Show only audio control buttons (No progressbar, title and current time)
        public static readonly BindableProperty OnlyButtonsProperty = BindableProperty.Create(nameof(OnlyButtons), typeof(bool), typeof(eliteAudio), false, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public bool OnlyButtons
        {
            get => (bool)GetValue(OnlyButtonsProperty);
            set => SetValue(OnlyButtonsProperty, value);
        }

        // Make the audio control buttons round
        public static readonly BindableProperty RoundButtonsProperty = BindableProperty.Create(nameof(RoundButtons), typeof(bool), typeof(eliteAudio), true, propertyChanged: (bindableObject, oldValue, value) =>
        {

                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public bool RoundButtons
        {
            get => (bool)GetValue(RoundButtonsProperty);
            set => SetValue(RoundButtonsProperty, value);
        }

        // Display Previous & Next controls
        public static readonly BindableProperty DisplaySwitchButtonsProperty = BindableProperty.Create(nameof(DisplaySwitchButtons), typeof(bool), typeof(eliteAudio), false, propertyChanged: (bindableObject, oldValue, value) =>
        {

                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public bool DisplaySwitchButtons
        {
            get => (bool)GetValue(DisplaySwitchButtonsProperty);
            set => SetValue(DisplaySwitchButtonsProperty, value);
        }

        // Whether the song should be played automatically when prepared
        public static readonly BindableProperty AutoPlaySongProperty = BindableProperty.Create(nameof(AutoPlaySong), typeof(bool), typeof(eliteAudio), false, propertyChanged: (bindableObject, oldValue, value) =>
        {
            if (value != null)
            {
                if ((bool)value)
                {
                    ((eliteAudio)bindableObject).playAudio();
                }
                ((eliteAudio)bindableObject).InvalidateSurface();
            }
        });
        public bool AutoPlaySong
        {
            get => (bool)GetValue(AutoPlaySongProperty);
            set => SetValue(AutoPlaySongProperty, value);
        }

        // The primary color of this element
        public static readonly BindableProperty ColorPrimaryProperty = BindableProperty.Create(nameof(ColorPrimary), typeof(Color), typeof(eliteAudio), coreSettings.ColorPrimary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorPrimary
        {
            get => (Color)GetValue(ColorPrimaryProperty);
            set => SetValue(ColorPrimaryProperty, value);
        }

        // The secondary color of this element
        public static readonly BindableProperty ColorSecondaryProperty = BindableProperty.Create(nameof(ColorSecondary), typeof(Color), typeof(eliteAudio), coreSettings.ColorSecondary, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public Color ColorSecondary
        {
            get => (Color)GetValue(ColorSecondaryProperty);
            set => SetValue(ColorSecondaryProperty, value);
        }

        // The is gradient property. When set to true, ColorPrimary and ColorSecondary are combined to a linear gradient. When set to false, ColorPrimary is used to define the background color
        public static readonly BindableProperty IsGradientProperty = BindableProperty.Create(nameof(IsGradient), typeof(bool), typeof(eliteAudio), true, propertyChanged: (bindableObject, oldValue, value) =>
        {
                if (value != null)
                {
                    ((eliteAudio)bindableObject).InvalidateSurface();
                }
            
        });
        public bool IsGradient
        {
            get => (bool)GetValue(IsGradientProperty);
            set => SetValue(IsGradientProperty, value);
        }

        public eliteAudio()
        {
            // Default view properties
            this.HeightRequest = 60;
            this.VerticalOptions = LayoutOptions.Start;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            if (this.OnlyButtons)
            {
                if (this.DisplaySwitchButtons)
                    this.WidthRequest = 175;
                else
                    this.WidthRequest = 60;

                this.HorizontalOptions = LayoutOptions.Start;
            }

            this.EnableTouchEvents = true;
            this.Touch += eliteAudioTouched;

            DependencyService.Get<IDependencyAudio>().OnPrepared = new Action(() =>
            {
                this.playerPrepared = true;

                TimeSpan timespanDurationTime = TimeSpan.FromMilliseconds(DependencyService.Get<IDependencyAudio>().getDuration());

                this.audioTimeDuration = string.Format("00:00 / {0:D2}:{1:D2}",
                    timespanDurationTime.Minutes,
                    timespanDurationTime.Seconds
                );

                this.audioTime = this.audioTimeDuration;

                if (this.AutoPlaySong)
                    this.playAudio();

                this.InvalidateSurface();
            });
            DependencyService.Get<IDependencyAudio>().OnFinishedPlaying = new Action(() => {
                this.toggleButtonState = true;

                this.audioTime = this.audioTimeDuration;

                // Switch to next song when playlist
            });
        }

        private void playAudio()
        {
            DependencyService.Get<IDependencyAudio>().Play();

            Device.StartTimer(TimeSpan.FromMilliseconds(1), () =>
            {
                if (this.toggleButtonState)
                    return false;

                TimeSpan timespanCurrentTime = TimeSpan.FromMilliseconds(DependencyService.Get<IDependencyAudio>().getCurrentPosition());
                TimeSpan timespanDurationTime = TimeSpan.FromMilliseconds(DependencyService.Get<IDependencyAudio>().getDuration());

                this.audioTime = string.Format("{0:D2}:{1:D2} / {2:D2}:{3:D2}",
                    timespanCurrentTime.Minutes,
                    timespanCurrentTime.Seconds,
                    timespanDurationTime.Minutes,
                    timespanDurationTime.Seconds);

                this.InvalidateSurface();
                return true;
            });
        }

        private void pauseAudio()
        {
            DependencyService.Get<IDependencyAudio>().Pause();
        }

        private decimal getCurrentBarWidth()
        {
            decimal currentPositionInPixel = 0;

            if (this.playerCount > 0)
            {
                int currentPosition = DependencyService.Get<IDependencyAudio>().getCurrentPosition();
                int audioDuration = DependencyService.Get<IDependencyAudio>().getDuration();

                decimal oneSecondInPercent = 100m / audioDuration;
                decimal currentPositionInPercent = oneSecondInPercent * currentPosition;

                decimal onePercentInPixel = this.maximalBarWidth / 100m;
                currentPositionInPixel = onePercentInPixel * currentPositionInPercent;
            }

            return currentPositionInPixel;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            this.canvasWidth = eventArgs.Info.Width;
            this.canvasHeight = eventArgs.Info.Height;

            if (AudioCollection != null)
                this.playerCount = AudioCollection.Count;

            if (!this.playerLoaded && this.playerCount > 0 && AudioCollection != null)
            {
                eliteAudioItem firstAudioItem = AudioCollection.First();
                this.setSong(firstAudioItem);
                this.playerLoaded = true;
            }

            string buttonIcon = this.playerPrepared ? (this.toggleButtonState ? "\uf04b" : "\uf04c") : "\uf110";
            int buttonRadius = this.RoundButtons ? this.toggleButtonSize / 2 : this.toggleButtonSize / 6;

            SKRoundRect roundRectBackground = new SKRoundRect(new SKRect(0, 0, this.canvasWidth, this.canvasHeight), this.canvasHeight / 6, this.canvasHeight / 6);
            SKPaint paintBackground = new SKPaint()
            {
                Color = this.ColorPrimary.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            if (this.IsGradient)
                paintBackground.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(roundRectBackground.Rect.Left, roundRectBackground.Rect.Top),
                    new SKPoint(roundRectBackground.Rect.Right, roundRectBackground.Rect.Bottom),
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

            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);

            float roundRectToggleButtonLeft = this.offsetHorizontal;
            float roundRectToggleButtonRight = this.toggleButtonSize;
            float roundRectToggleButtonTop = (this.canvasHeight / 2) + (this.toggleButtonSize / 2) - (this.offsetVertical / 2);
            float roundRectToggleButtonBottom = (this.canvasHeight / 2) - (this.toggleButtonSize / 2) + (this.offsetVertical / 2);

            if (this.OnlyButtons)
            {
                roundRectToggleButtonLeft = (this.canvasWidth / 2) - (this.toggleButtonSize / 2) + (this.offsetHorizontal / 2);
                roundRectToggleButtonRight = (this.canvasWidth / 2) + (this.toggleButtonSize / 2) - (this.offsetHorizontal / 2);
                roundRectToggleButtonTop = (this.canvasHeight / 2) + (this.toggleButtonSize / 2) - (this.offsetVertical / 2);
                roundRectToggleButtonBottom = (this.canvasHeight / 2) - (this.toggleButtonSize / 2) + (this.offsetVertical / 2);
            }

            SKPaint paintToggleButton = new SKPaint()
            {
                Color = Color.FromRgba(0, 0, 0, 75).ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            SKPaint paintTitle = new SKPaint()
            {
                TextSize = 36f,
                IsAntialias = true,
                Color = Color.White.ToSKColor(),
                TextAlign = SKTextAlign.Left
            };
            SKRect rectTitle = new SKRect();
            paintTitle.MeasureText(this.audioTitle, ref rectTitle);

            SKPaint paintBar = new SKPaint()
            {
                Color = Color.FromRgba(255, 255, 255, 100).ToSKColor(),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = rectTitle.Height
            };

            SKPaint paintBarCurrent = new SKPaint()
            {
                Color = Color.White.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                StrokeWidth = rectTitle.Height
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

            SKPaint paintTime = new SKPaint()
            {
                TextSize = 28f,
                IsAntialias = true,
                Color = Color.White.ToSKColor(),
                TextAlign = SKTextAlign.Right
            };
            SKRect rectTime = new SKRect();
            paintTime.MeasureText(this.audioTime, ref rectTime);

            // Switch to previous / next song buttons
            SKPaint paintSwitchButton = new SKPaint()
            {
                Color = Color.FromRgba(0, 0, 0, 50).ToSKColor(),
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

            if (this.OnlyButtons)
            {
                previousButtonLeft = roundRectToggleButtonLeft - this.switchButtonSize;
                previousButtonRight = roundRectToggleButtonLeft - this.offsetHorizontal;

                nextButtonLeft = roundRectToggleButtonRight + this.offsetHorizontal;
                nextButtonRight = roundRectToggleButtonRight + this.switchButtonSize;
            }

            SKRoundRect roundRectPreviousButton = new SKRoundRect(
                new SKRect(
                    previousButtonLeft,
                    previousButtonTop,
                    previousButtonRight,
                    previousButtonBottom
                ),
                this.RoundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6,
                this.RoundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6
            );

            SKRoundRect roundRectNextButton = new SKRoundRect(
                new SKRect(
                    nextButtonLeft,
                    nextButtonTop,
                    nextButtonRight,
                    nextButtonBottom
                ),
                this.RoundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6,
                this.RoundButtons ? this.switchButtonSize / 2 : this.switchButtonSize / 6
            );

            if (this.DisplaySwitchButtons)
            {
                roundRectToggleButtonLeft = roundRectPreviousButton.Rect.Left + roundRectPreviousButton.Width + this.offsetHorizontal;
                roundRectToggleButtonRight = roundRectPreviousButton.Rect.Left + roundRectPreviousButton.Width + this.toggleButtonSize;

                if (this.playerCount > 0)
                {
                    givenCanvas.DrawRoundRect(roundRectPreviousButton, paintSwitchButton);
                    givenCanvas.DrawRoundRect(roundRectNextButton, paintSwitchButton);

                    givenCanvas.DrawText(iconPrevious, roundRectPreviousButton.Rect.Left + (roundRectPreviousButton.Width / 2), (this.canvasHeight / 2) - rectPreviousIcon.MidY, paintSwitchIcon);
                    givenCanvas.DrawText(iconNext, roundRectNextButton.Rect.Left + (roundRectNextButton.Width / 2), (this.canvasHeight / 2) - rectNextIcon.MidY, paintSwitchIcon);
                }
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

            if (!this.DisplaySwitchButtons)
            {
                this.maximalBarWidth = this.canvasWidth - (this.offsetHorizontal * 2) - this.toggleButtonSize;
                this.currentBarWidth = this.toggleButtonSize + this.offsetHorizontal + (int)this.getCurrentBarWidth();
            }
            else
            {
                this.maximalBarWidth = this.canvasWidth - (this.offsetHorizontal * 2) - this.toggleButtonSize - this.switchButtonSize * 2;
                this.currentBarWidth = this.switchButtonSize + this.offsetHorizontal + this.toggleButtonSize + (int)this.getCurrentBarWidth();
            }

            if (!this.OnlyButtons)
            {
                int metaSpacing = 10;
                float progressBarY = (this.canvasHeight / 2) + (rectTitle.Height / 2) + metaSpacing;

                float audioTitleY = (this.canvasHeight / 2) + (rectTitle.Height / 2) - (rectTitle.Height / 2) - metaSpacing;
                float audioTitleX = roundRectToggleButton.Width + (this.offsetHorizontal * 2);
                float audioTimeY = (this.canvasHeight / 2) + (rectTime.Height / 2) - (rectTitle.Height / 2) - metaSpacing;
                float audioTimeX = this.canvasWidth - this.offsetHorizontal;

                float progressBarStartX = roundRectToggleButton.Width + (this.offsetHorizontal * 2);
                float progressBarEndX = this.canvasWidth - this.offsetHorizontal;
                float progressBarCurrentStartX = progressBarStartX;
                float progressBarCurrentEndX = this.currentBarWidth;

                if (this.DisplaySwitchButtons)
                {
                    audioTitleX = roundRectToggleButton.Rect.Left + roundRectToggleButton.Width + this.offsetHorizontal;
                    audioTimeX = this.canvasWidth - this.offsetHorizontal - this.switchButtonSize;

                    progressBarStartX = roundRectToggleButton.Rect.Left + roundRectToggleButton.Width + this.offsetHorizontal;
                    progressBarEndX = this.canvasWidth - this.offsetHorizontal - roundRectNextButton.Width - this.offsetHorizontal;
                    progressBarCurrentStartX = progressBarStartX;
                }

                if (this.playerCount > 0)
                {
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
                    givenCanvas.DrawText(this.audioTitle, audioTitleX, audioTitleY, paintTitle);
                    givenCanvas.DrawText(this.audioTime, audioTimeX, audioTimeY, paintTime);
                }
            }

            float buttonToggleIconX = roundRectToggleButton.Rect.Left + (roundRectToggleButton.Width / 2);
            float buttonToggleIconY = (this.canvasHeight / 2) - rectIcon.MidY;

            if (this.OnlyButtons)
            {
                buttonToggleIconX = (this.canvasWidth / 2);
                buttonToggleIconY = (this.canvasHeight / 2) - rectIcon.MidY;
            }

            if (this.playerCount > 0)
            {
                givenCanvas.DrawRoundRect(roundRectToggleButton, paintToggleButton);
                givenCanvas.DrawText(buttonIcon, buttonToggleIconX, buttonToggleIconY, paintIcon);
            }

            this.toggleButtonLocation = new SKPoint(
                roundRectToggleButtonLeft,
                roundRectToggleButtonTop
            );

            this.backwardButtonLocation = roundRectPreviousButton.Rect;
            this.forwardButtonLocation = roundRectNextButton.Rect;

            if (this.playerCount == 0)
            {
                SKPaint paintTextEmpty = new SKPaint()
                {
                    Color = Color.White.ToSKColor(),
                    IsAntialias = false,
                    TextSize = 40
                };

                SKRect rectTextEmpty = new SKRect();
                paintTextEmpty.MeasureText("Playlist is empty", ref rectTextEmpty);

                givenCanvas.DrawText("Playlist is empty", this.canvasWidth / 2 - rectTextEmpty.MidX, this.canvasHeight / 2 - rectTextEmpty.MidY, paintTextEmpty);
            }
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

        private void eliteAudioTouched(object eventSender, SKTouchEventArgs eventArgs)
        {
            SKPoint touchLocation = eventArgs.Location;

            float backwardButtonSize = backwardButtonLocation.Bottom - backwardButtonLocation.Top;
            float forwardButtonSize = forwardButtonLocation.Bottom - forwardButtonLocation.Top;

            switch (eventArgs.ActionType)
            {
                case SKTouchAction.Released:
                    {
                        if ((touchLocation.X >= this.toggleButtonLocation.X && touchLocation.X <= (this.toggleButtonLocation.X + this.toggleButtonSize - this.offsetHorizontal))
                            && (touchLocation.Y >= (this.toggleButtonLocation.Y - this.toggleButtonSize + this.offsetVertical) && touchLocation.Y <= this.toggleButtonLocation.Y))
                        {
                            if (this.playerPrepared)
                            {
                                this.toggleButtonState = !this.toggleButtonState;
                                this.InvalidateSurface();

                                if (this.toggleButtonState)
                                    this.pauseAudio();
                                else
                                    this.playAudio();
                            }
                        }

                        if ((touchLocation.X >= this.backwardButtonLocation.Left && touchLocation.X <= (this.backwardButtonLocation.Left + backwardButtonSize))
                            && (touchLocation.Y >= this.backwardButtonLocation.Top && touchLocation.Y <= (this.backwardButtonLocation.Top + backwardButtonSize)))
                        {
                            if (this.playerPlaylist)
                            {
                                int playlistCount = AudioCollection?.Count ?? 0;

                                if ((this.playerCurrent - 1) < 1)
                                    this.playerCurrent = playlistCount;
                                else
                                    this.playerCurrent--;

                                eliteAudioItem audioItemRequired = this.AudioCollection?[this.playerCurrent - 1];
                                this.setSong(audioItemRequired, !this.toggleButtonState);
                            }
                        }

                        if ((touchLocation.X >= this.forwardButtonLocation.Left && touchLocation.X <= (this.forwardButtonLocation.Left + forwardButtonSize))
                            && (touchLocation.Y >= this.forwardButtonLocation.Top && touchLocation.Y <= (this.forwardButtonLocation.Top + forwardButtonSize)))
                        {
                            if (this.playerPlaylist)
                            {
                                int playlistCount = AudioCollection?.Count ?? 0;

                                if ((this.playerCurrent + 1) > playlistCount)
                                    this.playerCurrent = 1;
                                else
                                    this.playerCurrent++;

                                eliteAudioItem audioItemRequired = this.AudioCollection?[this.playerCurrent - 1];
                                this.setSong(audioItemRequired, !this.toggleButtonState);
                            }
                        }
                    }
                    break;
            }

            eventArgs.Handled = true;
        }

        private void setSong(eliteAudioItem audioItem, bool autoPlay = false)
        {
            if (audioItem == null) return;
            this.playerPrepared = false;
            this.AutoPlaySong = autoPlay;
            this.audioTitle = audioItem.AudioTitle;

            DependencyService.Get<IDependencyAudio>().PrepareAudio(audioItem.AudioSource);

            this.toggleButtonState = !autoPlay;
            this.pauseAudio();

            this.InvalidateSurface();
        }
    }
}
