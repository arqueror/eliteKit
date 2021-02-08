using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteChatMessageData
    {
        public bool IsClient { get; set; } = false;
        public string ChatMessage { get; set; } = "";
    }

    public class eliteChatMessages : ContentView
    {
        private bool isTyping = false;
        private Frame frameImageClient;
        private ScrollView scrollViewContainer;
        private StackLayout stackLayoutContainer;
        private ObservableCollection<eliteChatMessageItem> chatMessageCollection = new ObservableCollection<eliteChatMessageItem>();

        public static readonly BindableProperty ChatMessageCollectionProperty = BindableProperty.Create(nameof(ChatMessageCollection), typeof(ObservableCollection<eliteChatMessageData>), typeof(eliteChatMessages), default(ObservableCollection<eliteChatMessageData>), propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                ((eliteChatMessages)bindableObject).ItemsSourceChanged(bindableObject, oldValue, newValue);
                ((eliteChatMessages)bindableObject).renderChatMessagesDataCollection();
            });
        public ObservableCollection<eliteChatMessageData> ChatMessageCollection
        {
            get => (ObservableCollection<eliteChatMessageData>)GetValue(ChatMessageCollectionProperty);
            set => SetValue(ChatMessageCollectionProperty, value);

        }
        void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += (sender, e) =>
                {
                    if (e.NewItems != null)
                    {
                        var tempList = new List<eliteChatMessageData>();
                        if (e.NewItems != null)
                        {
                            foreach (eliteChatMessageData newItem in e.NewItems)
                            {
                                tempList.Add(newItem);
                            }
                            foreach (var newItem in ChatMessageCollection)
                            {
                                var item = (eliteChatMessageData)newItem;
                                tempList.Add(item);
                            }

                            ChatMessageCollection = new ObservableCollection<eliteChatMessageData>(tempList);

                        }
                    }

                };

            }

        }

        private Image imageClient;
        public static readonly BindableProperty ImageClientSourceProperty = BindableProperty.Create(nameof(ImageClientSource), typeof(ImageSource), typeof(eliteChatMessages), null, propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                if (newValue != null)
                {
                    var imgSource = newValue as ImageSource;
                    if(imgSource!=null)
                        ((eliteChatMessages) bindableObject).imageClient.Source = imgSource;
                }
            });
        public ImageSource ImageClientSource
        {
            get => (ImageSource)GetValue(ImageClientSourceProperty);
            set => SetValue(ImageClientSourceProperty, value);
        }

        public static readonly BindableProperty AutoScrollProperty = BindableProperty.Create(nameof(AutoScroll), typeof(bool), typeof(eliteChatMessages), false, propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                    if (newValue != AutoScrollProperty.DefaultValue)
                    {
                        ((eliteChatMessages)bindableObject).AutoScroll = (bool)AutoScrollProperty.DefaultValue;
                        return;
                    }
                
            });
        public bool AutoScroll
        {
            get => (bool)GetValue(AutoScrollProperty);
            set => SetValue(AutoScrollProperty, value);
        }

        public eliteChatMessages()
        {
            this.scrollViewContainer = new ScrollView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            this.stackLayoutContainer = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Spacing = 2
            };

            this.imageClient = new Image()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.LightGray
            };

            this.frameImageClient = new Frame()
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start,
                CornerRadius = 40,
                WidthRequest = 40,
                HeightRequest = 40,
                HasShadow = false,
                IsClippedToBounds = true,
                Content = this.imageClient,
                Padding = new Thickness(0)
            };

            this.scrollViewContainer.Content = this.stackLayoutContainer;
            this.Content = this.scrollViewContainer;
        }

        private void renderChatMessagesDataCollection()
        {
            this.stackLayoutContainer.Children.Clear();

            foreach(eliteChatMessageData chatMessageData in this.ChatMessageCollection)
            {
                this.addChatMessage(
                    chatMessageData.ChatMessage,
                    !chatMessageData.IsClient
                );
            }
        }

        private void scrollChatToBottom()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(3);
                await this.scrollViewContainer.ScrollToAsync(0, this.stackLayoutContainer.Height, false);
            });
        }

        public void showChatMessageTyping()
        {
            this.pushLeftLastChatMessage();

            eliteChatMessageTypingShape chatMessageTypingShape = new eliteChatMessageTypingShape()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 40,
                WidthRequest = 70
            };

            StackLayout stackLayoutTyping = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                Spacing = 10
            };

            stackLayoutTyping.Children.Add(this.frameImageClient);
            stackLayoutTyping.Children.Add(chatMessageTypingShape);
            this.stackLayoutContainer.Children.Add(stackLayoutTyping);

            this.isTyping = true;
        }

        public void hideChatMessageTyping()
        {
            this.isTyping = false;
        }

        private void pushLeftLastChatMessage()
        {
            foreach (object chatMessageView in this.stackLayoutContainer.Children)
            {
                if (chatMessageView is StackLayout)
                {
                    StackLayout stackLayoutMessageIteration = ((StackLayout)chatMessageView);

                    int requiredChatMessageItemIndex = 0;

                    if (stackLayoutMessageIteration.Children.Count == 2)
                        requiredChatMessageItemIndex = 1;

                    eliteChatMessageItem chatMessageItem = (eliteChatMessageItem)stackLayoutMessageIteration.Children[requiredChatMessageItemIndex];

                    if (!chatMessageItem.ChatMessageSender)
                    {
                        double marginTop = ((StackLayout)chatMessageView).Margin.Top;
                        ((StackLayout)chatMessageView).Margin = new Thickness(50, marginTop, 0, 0);
                    }
                }
            }
        }

        public void addChatMessage(string chatMessage, bool chatMessageSender = true)
        {
            bool isFirstCollectionMessage = false;
            bool isFirstMessage = false;
            bool isLastMessage = true;

            if (this.chatMessageCollection.Count == 0)
            {
                isFirstMessage = true;
                isFirstCollectionMessage = true;
            }
            else
            {
                if (this.chatMessageCollection.Last().ChatMessageSender
                    && !chatMessageSender)
                    isFirstMessage = true;

                if (!this.chatMessageCollection.Last().ChatMessageSender
                    && chatMessageSender)
                    isFirstMessage = true;

                if (this.chatMessageCollection.Last().ChatMessageSender
                    && chatMessageSender)
                    this.chatMessageCollection.Last().IsLastMessage = false;

                if (!this.chatMessageCollection.Last().ChatMessageSender
                    && !chatMessageSender)
                    this.chatMessageCollection.Last().IsLastMessage = false;
            }

            eliteChatMessageItem eliteChatMessageItem = new eliteChatMessageItem()
            {
                ChatMessage = chatMessage,
                ChatMessageSender = chatMessageSender,
                IsFirstMessage = isFirstMessage,
                IsLastMessage = isLastMessage
            };
            this.chatMessageCollection.Add(eliteChatMessageItem);

            if (!chatMessageSender)
            {
                this.pushLeftLastChatMessage();

                StackLayout stackLayoutMessage = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 10
                };

                stackLayoutMessage.Children.Add(this.frameImageClient);
                stackLayoutMessage.Children.Add(eliteChatMessageItem);

                this.stackLayoutContainer.Children.Add(stackLayoutMessage);
            }
            else
            {
                if(!this.isTyping)
                    this.stackLayoutContainer.Children.Add(eliteChatMessageItem);
                else
                {
                    int latestChatMessageIndex = this.stackLayoutContainer.Children.Count - 1;
                    this.stackLayoutContainer.Children.Insert(latestChatMessageIndex, eliteChatMessageItem);
                }
            }

            if(isFirstMessage
                && !isFirstCollectionMessage)
            {
                View stackLayoutContainerLastChildren = this.stackLayoutContainer.Children.Last();
                double marginLeft = stackLayoutContainerLastChildren.Margin.Left;
                stackLayoutContainerLastChildren.Margin = new Thickness(marginLeft, 10, 0, 0);
            }

            if (this.AutoScroll)
                this.scrollChatToBottom();
        }
    }

    class eliteChatMessageItem : ContentView
    {
        private AbsoluteLayout absoluteLayoutContainer;
        private Label labelChatMessage;
        private eliteChatMessageShape eliteChatMessageShape;

        private Color colorTextSender = Color.White;
        private Color colorTextClient = Color.Black;

        private string chatMessage = "";
        public string ChatMessage
        {
            get
            {
                return this.chatMessage;
            }
            set
            {
                this.chatMessage = value;
                this.labelChatMessage.Text = this.chatMessage;
            }
        }

        private bool chatMessageSender = true;
        public bool ChatMessageSender
        {
            get
            {
                return this.chatMessageSender;
            }
            set
            {
                this.chatMessageSender = value;
                this.eliteChatMessageShape.ChatMessageSender = value;
                this.labelChatMessage.TextColor = value ? this.colorTextSender : this.colorTextClient;

                if(!value)
                    this.absoluteLayoutContainer.HorizontalOptions = LayoutOptions.Start;
            }
        }

        private bool isFirstMessage = false;
        public bool IsFirstMessage
        {
            get
            {
                return this.isFirstMessage;
            }
            set
            {
                this.isFirstMessage = value;
                this.eliteChatMessageShape.IsFirstMessage = value;
            }
        }

        private bool isLastMessage = false;
        public bool IsLastMessage
        {
            get
            {
                return this.isLastMessage;
            }
            set
            {
                this.isLastMessage = value;
                this.eliteChatMessageShape.IsLastMessage = value;
            }
        }

        public eliteChatMessageItem()
        {
            this.absoluteLayoutContainer = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.End
            };

            this.labelChatMessage = new Label()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Text = this.chatMessage,
                TextColor = Color.White,
                FontSize = 16,
                Margin = new Thickness(15, 10),
                LineBreakMode = LineBreakMode.WordWrap
            };

            this.eliteChatMessageShape = new eliteChatMessageShape()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsFirstMessage = this.isFirstMessage,
                IsLastMessage = this.isLastMessage,
                ChatMessageSender = this.chatMessageSender
            };

            this.absoluteLayoutContainer.Children.Add(this.eliteChatMessageShape, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            this.absoluteLayoutContainer.Children.Add(this.labelChatMessage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            this.Content = this.absoluteLayoutContainer;
        }
    }

    class eliteChatMessageTypingShape : SKCanvasView
    {
        private int canvasHeight;
        private int canvasWidth;

        private int cornerRadius = 50;
        private float dotRadius = 10;
        private int dotPadding = 20;

        private SKColor colorBackground = SKColor.Parse("DCDCDC");
        private SKColor colorDots = SKColor.Parse("8D949E");

        private bool dotPositionSet = false;
        private int[] dotPositions = new int[3];
        private int dotAnimationIndex = 0;
        private int dotAnimationDuration = 0;
        private bool dotAnimationReverse = false;

        public eliteChatMessageTypingShape()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(5), () =>
            {
                this.dotAnimationDuration += 5;

                if(!this.dotAnimationReverse)
                {
                    this.dotPositions[this.dotAnimationIndex] -= 1;
                } else
                {
                    this.dotPositions[this.dotAnimationIndex] += 1;
                }

                if(this.dotAnimationDuration == 100)
                {
                    if (this.dotAnimationReverse == true)
                    {
                        if (this.dotAnimationIndex < 2)
                            this.dotAnimationIndex++;
                        else
                            this.dotAnimationIndex = 0;
                    }

                    this.dotAnimationDuration = 0;
                    this.dotAnimationReverse = !this.dotAnimationReverse;
                }

                this.InvalidateSurface();
                return true;
            });
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            canvasWidth = eventArgs.Info.Width;
            canvasHeight = eventArgs.Info.Height;

            if(!this.dotPositionSet)
            {
                this.dotPositions[0] = this.canvasHeight / 2;
                this.dotPositions[1] = this.dotPositions[0];
                this.dotPositions[2] = this.dotPositions[0];
                this.dotPositionSet = true;
            }

            SKRoundRect roundRectBackground = new SKRoundRect(
                new SKRect(
                    0,
                    0,
                    this.canvasWidth,
                    this.canvasHeight
                ),
                this.cornerRadius,
                this.cornerRadius
            );

            SKPaint paintBackground = new SKPaint()
            {
                Color = this.colorBackground,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);

            SKPaint paintDot = new SKPaint()
            {
                Color = this.colorDots,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            int dotMargin = 10;
            int dotSpace = (this.canvasWidth - (this.dotPadding * 2)) - (dotMargin * 2) - (((int)this.dotRadius * 2) * 3);

            int dotOne = dotSpace;
            int dotTwo = (((int)this.dotRadius * 2) + (dotMargin * 1) * 1) + dotSpace;
            int dotThree = (((int)this.dotRadius * 2) + (dotMargin * 2) * 2) + dotSpace;

            givenCanvas.DrawCircle(
                new SKPoint(
                    dotOne,
                    this.dotPositions[0]
                ),
                this.dotRadius,
                paintDot
            );

            givenCanvas.DrawCircle(
                new SKPoint(
                    dotTwo,
                    this.dotPositions[1]
                ),
                this.dotRadius,
                paintDot
            );

            givenCanvas.DrawCircle(
                new SKPoint(
                    dotThree,
                    this.dotPositions[2]
                ),
                this.dotRadius,
                paintDot
            );
        }
    }

    class eliteChatMessageShape : SKCanvasView
    {
        private int canvasHeight;
        private int canvasWidth;

        private int cornerRadius = 50;
        private int cornerRadiusConnected = 15;

        private SKColor colorBackgroundSender = SKColor.Parse("548EC1");
        private SKColor colorBackgroundClient = SKColor.Parse("DCDCDC");

        private bool chatMessageSender = true;
        public bool ChatMessageSender
        {
            get
            {
                return this.chatMessageSender;
            }
            set
            {
                this.chatMessageSender = value;
                this.InvalidateSurface();
            }
        }

        private bool isFirstMessage = false;
        public bool IsFirstMessage
        {
            get
            {
                return this.isFirstMessage;
            }
            set
            {
                this.isFirstMessage = value;
                this.InvalidateSurface();
            }
        }

        private bool isLastMessage = false;
        public bool IsLastMessage
        {
            get
            {
                return this.isLastMessage;
            }
            set
            {
                this.isLastMessage = value;
                this.InvalidateSurface();
            }
        }

        private SKPoint[] getRadiusPoints()
        {
            SKPoint[] radiusPoints = new SKPoint[4];

            if (this.chatMessageSender)
            {
                radiusPoints[0] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[1] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                radiusPoints[2] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                radiusPoints[3] = new SKPoint(this.cornerRadius, this.cornerRadius);

                if (this.IsFirstMessage)
                {
                    radiusPoints[0] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[1] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[2] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                    radiusPoints[3] = new SKPoint(this.cornerRadius, this.cornerRadius);
                }

                if (this.IsLastMessage)
                {
                    radiusPoints[0] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[1] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                    radiusPoints[2] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[3] = new SKPoint(this.cornerRadius, this.cornerRadius);
                }
            } else
            {
                radiusPoints[0] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                radiusPoints[1] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[2] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[3] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);

                if (this.IsFirstMessage)
                {
                    radiusPoints[0] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[1] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[2] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[3] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                }

                if (this.IsLastMessage)
                {
                    radiusPoints[0] = new SKPoint(this.cornerRadiusConnected, this.cornerRadiusConnected);
                    radiusPoints[1] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[2] = new SKPoint(this.cornerRadius, this.cornerRadius);
                    radiusPoints[3] = new SKPoint(this.cornerRadius, this.cornerRadius);
                }
            }

            if (this.IsFirstMessage
                && this.IsLastMessage)
            {
                radiusPoints[0] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[1] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[2] = new SKPoint(this.cornerRadius, this.cornerRadius);
                radiusPoints[3] = new SKPoint(this.cornerRadius, this.cornerRadius);
            }

            return radiusPoints;
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs eventArgs)
        {
            var givenCanvas = eventArgs.Surface.Canvas;
            givenCanvas.Clear();

            canvasWidth = eventArgs.Info.Width;
            canvasHeight = eventArgs.Info.Height;

            SKColor colorBackground = this.chatMessageSender ? this.colorBackgroundSender : this.colorBackgroundClient;

            SKRect rectBackground = new SKRect(
                0,
                0,
                this.canvasWidth,
                this.canvasHeight
            );

            SKRoundRect roundRectBackground = new SKRoundRect(
                rectBackground,
                0,
                0
            );

            roundRectBackground.SetRectRadii(
                rectBackground,
                this.getRadiusPoints()
            );

            SKPaint paintBackground = new SKPaint()
            {
                Color = colorBackground,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            givenCanvas.DrawRoundRect(roundRectBackground, paintBackground);
        }
    }
}
