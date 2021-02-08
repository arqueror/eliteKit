    using System;
    using System.ComponentModel;
    using CoreAnimation;
    using CoreGraphics;
    using eliteKit.eliteElements;
    using eliteKit.iOSCore.Renderers;
    using Foundation;
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;
  

    [assembly: ExportRenderer(typeof(eliteVideo), typeof(CustomRoundedContentViewRenderer))]

    namespace eliteKit.iOSCore.Renderers
    {
        internal class CustomRoundedContentViewRenderer : ViewRenderer
        {
            private bool _isDisposed;
            bool longPress = false;
            bool firstTime = true;
            double lastTimeStamp = 0f;
            UIPanGestureRecognizer panGesture;
            CGPoint lastLocation;
            CGPoint originalPosition;
            UIGestureRecognizer.Token panGestureToken;
            CGRect displayMetrics;
            nfloat sH, sW;
            bool isFixingfMenuPosition = false;
            void DetectPan()
            {
                var dragView = Element as eliteElements.eliteVideo;
                var ne = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(Element).NativeView;
                if (longPress || dragView.DragMode == eliteElements.eliteVideo.DragMod.Touch)
                {
                    if (panGesture.State == UIGestureRecognizerState.Began)
                    {
                        dragView.DragStarted();
                        if (firstTime)
                        {
                            originalPosition = Center;
                            firstTime = false;
                        }
                    }

                    CGPoint translation = panGesture.TranslationInView(Superview);
                    var currentCenterX = Center.X;
                    var currentCenterY = Center.Y;
                    if (dragView.DragDirection == eliteElements.eliteVideo.DragDirectionType.All || dragView.DragDirection == eliteElements.eliteVideo.DragDirectionType.Horizontal)
                    {
                        currentCenterX = lastLocation.X + translation.X;
                    }

                    if (dragView.DragDirection == eliteElements.eliteVideo.DragDirectionType.All || dragView.DragDirection == eliteElements.eliteVideo.DragDirectionType.Vertical)
                    {
                        currentCenterY = lastLocation.Y + translation.Y;
                    }
                    if (((currentCenterX >= 30 && currentCenterX <= sW - 30)) && ((currentCenterY >= 30 && currentCenterY <= sH - 30)))
                        Center = new CGPoint(currentCenterX, currentCenterY);

                    if (panGesture.State == UIGestureRecognizerState.Ended)
                    {

                        dragView.DragEnded();
                        longPress = false;
                        lastLocation = Center;
                    }
                }
            }







            protected override void OnElementChanged(ElementChangedEventArgs<View> e)
            {
                base.OnElementChanged(e);

                if (e.OldElement != null)
                {
                    RemoveGestureRecognizer(panGesture);
                    panGesture.RemoveTarget(panGestureToken);

                }
                if (e.NewElement != null)
                {
                    displayMetrics = UIScreen.MainScreen.Bounds;
                    sH = displayMetrics.Height;
                    sW = displayMetrics.Width;

                    var dragView = Element as eliteElements.eliteVideo;
                    panGesture = new UIPanGestureRecognizer();
                    panGestureToken = panGesture.AddTarget(DetectPan);
                    AddGestureRecognizer(panGesture);


                    dragView.RestorePositionCommand = new Command(() =>
                    {
                        if (!firstTime)
                        {

                            Center = originalPosition;
                        }
                    });

                }
            }


            protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (_isDisposed || NativeView == null) return;

                NativeView.SetNeedsDisplay();
                NativeView.SetNeedsLayout();

                //if (e.PropertyName == "IsOpened")
                //{
                var dragView = Element as eliteElements.eliteVideo;
                CGPoint translation = panGesture.TranslationInView(Superview);
                var currentCenterX = Center.X;
                var currentCenterY = Center.Y;
                int axisAdditionX = 20+(int)(dragView.Width/2);
                int axisAdditionY = 50+(int)(dragView.Height/2);
                //if (dragView.IsMenuSandboxEnabled)
                //{
                //SMART CONTAINMENT LOGIC
                //Is on the left side?
                if (currentCenterX <= axisAdditionX)
                            {
                                currentCenterX += (axisAdditionX - currentCenterX);
                                //Upper Y axis
                                if (currentCenterY <= axisAdditionY)
                                {
                                    currentCenterY += (axisAdditionY - currentCenterY);
                                }
                                //Bottom Y Axis
                                if ((currentCenterY + axisAdditionY) >= sH)
                                {
                                    currentCenterY = (sH - axisAdditionY);
                                }

                                Center = new CGPoint(currentCenterX, currentCenterY);
                            }

                            //Left X is good but Y top is not
                            if (currentCenterY <= axisAdditionY)
                            {
                                currentCenterY += (axisAdditionY - currentCenterY);
                                if (currentCenterX <= axisAdditionX)
                                {
                                    currentCenterX += (axisAdditionX - currentCenterX);
                                }
                                Center = new CGPoint(currentCenterX, currentCenterY);
                            }

                            //Left X is good but Y bottom is not
                            if ((currentCenterY + axisAdditionY) >= sH)
                            {
                                currentCenterY = (sH - axisAdditionY);
                                Center = new CGPoint(currentCenterX, currentCenterY);
                            }

                            //Is on the right side?
                            if ((currentCenterX + axisAdditionX) >= sW)
                            {
                                currentCenterX = (sW - axisAdditionX);

                                //Upper Y axis
                                if (currentCenterY <= axisAdditionY)
                                {
                                    currentCenterY += (axisAdditionY - currentCenterY);
                                }
                                //Bottom Y Axis
                                if ((currentCenterY + axisAdditionY) >= sH)
                                {
                                    currentCenterY = (sH - axisAdditionY);
                                }

                                Center = new CGPoint(currentCenterX, currentCenterY);
                            }

                            lastLocation = Center;
                        //}

                    //}
            
                base.OnElementPropertyChanged(sender, e);
            }

            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
                lastTimeStamp = evt.Timestamp;
                Superview.BringSubviewToFront(this);

                lastLocation = Center;
                (Element as eliteElements.eliteVideo).controlsShow();

        }
            public override void TouchesMoved(NSSet touches, UIEvent evt)
            {
            if (!(Element as eliteElements.eliteVideo).IsPopup) return;
            if (evt.Timestamp - lastTimeStamp >= 0.5)
                {
                    longPress = true;
                }
                base.TouchesMoved(touches, evt);
            }


            public override void Draw(CGRect rect)
            {
                var view = (eliteElements.eliteVideo)Element;

                UIRectCorner corners = 0;

                if (view.CornerRounded.ToLower().Contains("topleft"))
                    corners = corners | UIRectCorner.TopLeft;

                if (view.CornerRounded.ToLower().Contains("topright"))
                    corners = corners | UIRectCorner.TopRight;

                if (view.CornerRounded.ToLower().Contains("bottomright"))
                    corners = corners | UIRectCorner.BottomRight;

                if (view.CornerRounded.ToLower().Contains("bottomleft"))
                    corners = corners | UIRectCorner.BottomLeft;

                if (view.CornerRounded.ToLower().Contains("all"))
                    corners = UIRectCorner.AllCorners;

                var mPath = UIBezierPath.FromRoundedRect(Layer.Bounds, corners, new CGSize(view.CornerRadius, view.CornerRadius)).CGPath;

                if (view.IsPopup)
                {
                    Layer.ShadowColor = view.ShadowColor.ToCGColor();
                    Layer.ShadowOffset = new CGSize(view.ShadowHorizontalOffset, view.ShadowVerticalOffset);
                    Layer.ShadowOpacity = view.ShadowOpacity;
                    Layer.ShadowRadius = view.ShadowRadius;
                }

                if (Layer.Sublayers == null || Layer.Sublayers.Length <= 0) return;

                var subLayer = this.Layer.Sublayers[0];
                subLayer.CornerRadius = (float)view.CornerRadius;
                subLayer.Mask = new CAShapeLayer
                {
                    Frame = Layer.Bounds,
                    Path = mPath
                };
                subLayer.BorderColor = view.BorderColor.ToCGColor();
                subLayer.BorderWidth = view.BorderThickness;
            }

            protected override void Dispose(bool disposing)
            {
                //Element.PropertyChanged -= OnElementOnPropertyChanged;
                base.Dispose(disposing);
                _isDisposed = true;
            }

        }

    }