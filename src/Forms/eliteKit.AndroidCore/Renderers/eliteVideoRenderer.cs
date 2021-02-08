using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using eliteKit.eliteElements;
using eliteKit.Renderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(eliteVideo), typeof(RoundedCornerViewRenderer))]
namespace eliteKit.Renderers
{
    internal class RoundedCornerViewRenderer : ViewRenderer
    {
        float originalX;
        float originalY;
        float dX;
        float dY;
        bool firstTime = true;
        bool touchedDown = false;
        bool hasmoved = false;
        private DisplayMetrics displayMetrics;
        int sH, sW;
        int axisAdditionX = 140;
        int axisAdditionY = 140;

        public RoundedCornerViewRenderer(Context context) : base(context)
        { }

        public override bool OnTouchEvent(MotionEvent e)
        {

            float x = e.RawX;
            float y = e.RawY;
            var dragView = Element as eliteVideo;

            //if (!dragView.IsOpened)
            //{
            switch (e.Action)
            {
                case MotionEventActions.Down:

                    if (dragView.DragMode == eliteVideo.DragMod.Touch)
                    {
                        if (!touchedDown)
                        {
                            if (firstTime)
                            {
                                originalX = GetX();
                                originalY = GetY();
                                firstTime = false;
                            }

                            dragView.DragStarted();
                        }

                        touchedDown = true;
                    }
                    
                    dragView.controlsShow();
                    dX = x - this.GetX();
                    dY = y - this.GetY();

                    break;
                case MotionEventActions.Move:
                    float newX = x - dX;
                    var newY = y - dY;
                    if (touchedDown)
                    {
                        if (dragView.DragDirection == eliteVideo.DragDirectionType.All ||
                            dragView.DragDirection == eliteVideo.DragDirectionType.Horizontal)
                        {
                            if ((newX <= 0 || newX >= sW - Width))
                                break;

                            SetX(newX);
                        }

                        if (dragView.DragDirection == eliteVideo.DragDirectionType.All ||
                            dragView.DragDirection == eliteVideo.DragDirectionType.Vertical)
                        {
                            if ((newY <= 0 || newY >= sH - Height))
                                break;
                            SetY(newY);
                        }
                        hasmoved = true;
                    }

                    break;
                case MotionEventActions.Up:
                    touchedDown = false;
                    dragView.DragEnded();
                    if (hasmoved)
                    {
                        //dragView.IsOpened = false;
                        hasmoved = false;
                    }

                    break;
                case MotionEventActions.Cancel:
                    touchedDown = false;
                    break;
                    //}
            }

            return base.OnTouchEvent(e);
        }

        protected override void OnVisibilityChanged(Android.Views.View changedView, [GeneratedEnum] ViewStates visibility)
        {
            base.OnVisibilityChanged(changedView, visibility);
            if (visibility == ViewStates.Visible)
            {

            }
        }

        public override bool OnInterceptTouchEvent(MotionEvent e)
        {
            var dragView = Element as eliteVideo;

            BringToFront();
            if (!dragView.IsPopup)
                return false;
            if (dragView.AreControlsVisible)
                return false;
            return true;

        }

        private void HandleLongClick(object sender, LongClickEventArgs e)
        {
            var dragView = Element as eliteVideo;
            if (firstTime)
            {
                originalX = GetX();
                originalY = GetY();
                firstTime = false;
            }
            if (dragView.IsPopup)
            {
                dragView.DragStarted();
                touchedDown = true;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);


            if (e.OldElement != null)
            {
                LongClick -= HandleLongClick;
            }
            if (e.NewElement != null)
            {
                displayMetrics = Context.Resources.DisplayMetrics;
                sH = displayMetrics.HeightPixels;
                sW = displayMetrics.WidthPixels;
                //For larger DPI, double value for containment logic so it can adjust accordingly to screen real size
                if (displayMetrics.Density >= 4.0)
                {
                    //"xxxhdpi";
                    axisAdditionX = 280;
                    axisAdditionY = 280;
                }
                else if (displayMetrics.Density >= 3.0 && displayMetrics.Density < 4.0)
                {
                    //xxhdpi
                    axisAdditionX = 280;
                    axisAdditionY = 280;
                }
                else if (displayMetrics.Density >= 2.0)
                {
                    //xhdpi
                }
                else if (displayMetrics.Density >= 1.5 && displayMetrics.Density < 2.0)
                {
                    //hdpi
                }
                else if (displayMetrics.Density >= 1.0 && displayMetrics.Density < 1.5)
                {
                    //mdpi
                }

                LongClick += HandleLongClick;
                var dragView = Element as eliteVideo;
                //Click += DraggableMenuRenderer_Click;

                dragView.RestorePositionCommand = new Command(() =>
                {
                    if (!firstTime)
                    {
                        SetX(originalX);
                        SetY(originalY);
                    }

                });
            }

        }


        protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
        {
            if (Element == null) return false;

            var control = (eliteVideo)Element;

            //var drawable = GenerateBackgroundWithShadow(control, child, Color.White, Color.Black, 10, GravityFlags.Top);
            //return base.DrawChild(canvas, child, drawingTime);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                if (control.IsPopup)
                {
                    this.Elevation = 12;
                    this.TranslationZ = 12;
                    //ViewCompat.SetElevation(this, Context.ToPixels(10));
                    this.OutlineProvider = new RoundedCornerOutlineProvider(control, Context.ToPixels);
                    this.ClipToOutline = true;
                }
            }

            SetClipChildren(true);

            control.Padding = new Thickness(0, 0, 0, 0);

            //Create path to clip the child         
            var path = new Path();
            path.AddRoundRect(new RectF(0, 0, Width, Height),
                              GetRadii(control),
                              Path.Direction.Ccw);

            canvas.Save();
            canvas.ClipPath(path);


            //var drawable = GenerateBackgroundWithShadow(control, child, Android.Graphics.Color.White, Android.Graphics.Color.Black, 10, GravityFlags.Bottom);
            // Draw the child first so that the border shows up above it.        
            var result = base.DrawChild(canvas, child, drawingTime);

            canvas.Restore();

            DrawBorder(canvas, control, path);
            //this.SetBackground(drawable);
       
            //Properly dispose        
            path.Dispose();
            return true;
        }

        //public static Drawable GenerateBackgroundWithShadow(eliteVideo control, Android.Views.View child, Android.Graphics.Color backgroundColor,
        //                                                    Android.Graphics.Color shadowColor,
        //                                                    int elevation,
        //                                                    GravityFlags shadowGravity)
        //{
        //    var radii = GetRadii(control);

        //    int DY;
        //    switch (shadowGravity)
        //    {
        //        case GravityFlags.Center:
        //            DY = 0;
        //            break;
        //        case GravityFlags.Top:
        //            DY = -1 * elevation / 3;
        //            break;
        //        default:
        //        case GravityFlags.Bottom:
        //            DY = elevation / 3;
        //            break;
        //    }

        //    var shapeDrawable = new ShapeDrawable();

        //    shapeDrawable.Paint.Color = backgroundColor;
        //    shapeDrawable.Paint.SetShadowLayer(elevation, 0, DY, shadowColor);

        //    child.SetLayerType(LayerType.Software, shapeDrawable.Paint);

        //    shapeDrawable.Shape = new RoundRectShape(radii, null, null);

        //    var drawable = new LayerDrawable(new Drawable[] { shapeDrawable });
        //    drawable.SetLayerInset(0, elevation, elevation, elevation, elevation);

        //    child.Background = drawable;
        //    return drawable;

        //}

        public static float[] GetRadii(eliteVideo control)
        {
            var radius = (float)(control.CornerRadius);
            radius *= 2;

            var topLeft = control.CornerRounded.ToLower().Contains("topleft") ? radius : 0;
            var topRight = control.CornerRounded.ToLower().Contains("topright") ? radius : 0;
            var bottomLeft = control.CornerRounded.ToLower().Contains("bottomleft") ? radius : 0;
            var bottomRight = control.CornerRounded.ToLower().Contains("bottomright") ? radius : 0;

            if (control.CornerRounded.ToLower().Contains("all"))
                topLeft = topRight = bottomLeft = bottomRight = radius;

            var radii = new[] { topLeft, topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft };
            return radii;
        }

        private static void DrawBorder(Canvas canvas, eliteVideo control, Path path)
        {
            if (control.BorderColor == Xamarin.Forms.Color.Transparent ||
                control.BorderThickness <= 0) return;

            var paint = new Paint();
            paint.AntiAlias = true;
            paint.StrokeWidth = control.BorderThickness;
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = control.BorderColor.ToAndroid();

            canvas.DrawPath(path, paint);

            paint.Dispose();
        }
    }

    public class RoundedCornerOutlineProvider : ViewOutlineProvider
    {
        private readonly eliteVideo _pancake;
        private readonly Func<double, float> _convertToPixels;
        private readonly float[] radii;

        public RoundedCornerOutlineProvider(eliteVideo pancake, Func<double, float> convertToPixels)
        {
            _pancake = pancake;
            _convertToPixels = convertToPixels;
            radii = RoundedCornerViewRenderer.GetRadii(pancake);
        }

        public override void GetOutline(global::Android.Views.View view, Outline outline)
        {

                var path = ShapeUtils.CreateRoundedRectPath(view.Width, view.Height,
                    _convertToPixels(radii[2]),
                    _convertToPixels(radii[2]),
                    _convertToPixels(radii[5]),
                    _convertToPixels(radii[6]));

                if (path.IsConvex)
                {
                    outline.SetConvexPath(path);
                }
            
        }
    }


    public static class ShapeUtils
    {
        public static Path CreateRoundedRectPath(float rectWidth, float rectHeight, float topLeft, float topRight, float bottomRight, float bottomLeft)
        {
            var path = new Path();
            var radii = new[] { topLeft, topLeft,
                                topRight, topRight,
                                bottomRight, bottomRight,
                                bottomLeft, bottomLeft };


            path.AddRoundRect(new RectF(0, 0, rectWidth, rectHeight), radii, Path.Direction.Ccw);
            path.Close();

            return path;
        }

        public static Path CreatePolygonPath(double rectWidth, double rectHeight, int sides, double cornerRadius = 0.0, double rotationOffset = 0.0)
        {
            var offsetRadians = rotationOffset * Math.PI / 180;

            var path = new Path();
            var theta = 2 * Math.PI / sides;

            // depends on the rotation
            var width = (-cornerRadius + Math.Min(rectWidth, rectHeight)) / 2;
            var center = new Point(rectWidth / 2, rectHeight / 2);

            var radius = width + cornerRadius - (Math.Cos(theta) * cornerRadius) / 2;

            var angle = offsetRadians;
            var corner = new Point(center.X + (radius - cornerRadius) * Math.Cos(angle), center.Y + (radius - cornerRadius) * Math.Sin(angle));
            path.MoveTo((float)(corner.X + cornerRadius * Math.Cos(angle + theta)), (float)(corner.Y + cornerRadius * Math.Sin(angle + theta)));

            for (var i = 0; i < sides; i++)
            {
                angle += theta;
                corner = new Point(center.X + (radius - cornerRadius) * Math.Cos(angle), center.Y + (radius - cornerRadius) * Math.Sin(angle));
                var tip = new Point(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                var start = new Point(corner.X + cornerRadius * Math.Cos(angle - theta), corner.Y + cornerRadius * Math.Sin(angle - theta));
                var end = new Point(corner.X + cornerRadius * Math.Cos(angle + theta), corner.Y + cornerRadius * Math.Sin(angle + theta));

                path.LineTo(start.X, start.Y);
                path.QuadTo(tip.X, tip.Y, end.X, end.Y);
            }

            path.Close();

            return path;
        }

        public class Point
        {
            public float X { get; set; }
            public float Y { get; set; }
            public Point(double X, double Y)
            {
                this.X = (float)X;
                this.Y = (float)Y;
            }
        }
    }
}