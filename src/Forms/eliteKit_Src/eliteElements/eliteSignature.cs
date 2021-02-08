using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using eliteKit.eliteEnums;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

#pragma warning disable ALL
namespace eliteKit.eliteElements
{
    public class eliteSignature : BorderBase
    {
        SKPaint _signaturePaint;
        bool _awaitingSnapshot;

        List<SKPoint[]> _strokes;
        List<SKPoint> _points;
        TaskCompletionSource<Stream> _exportTcs;

        public static readonly BindableProperty PenColorProperty =
            BindableProperty.Create(nameof(PenColor), typeof(Color), typeof(eliteSignature), Color.Black, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var signature = (eliteSignature)bindableObject;
                        signature.CreateSignaturePaint();
                        signature.Redraw();
                    
                });

        public Color PenColor
        {
            get => (Color)GetValue(PenColorProperty);
            set => SetValue(PenColorProperty, value);
        }

        public static readonly BindableProperty PenThicknessProperty =
            BindableProperty.Create(nameof(PenColor), typeof(float), typeof(eliteSignature), 5.0f, BindingMode.OneWay,
                propertyChanged: (bindableObject, oldValue, value) =>
                {
                        var signature = (eliteSignature)bindableObject;
                        signature.CreateSignaturePaint();
                        signature.Redraw();
                    
                });

        public float PenThickness
        {
            get => (float)GetValue(PenThicknessProperty);
            set => SetValue(PenThicknessProperty, value);
        }

        public static readonly BindableProperty PenModeProperty =
            BindableProperty.Create(nameof(PenMode), typeof(SignaturePenMode), typeof(eliteSignature), SignaturePenMode.ActiveTrace, BindingMode.OneWay);

        public SignaturePenMode PenMode
        {
            get => (SignaturePenMode)GetValue(PenModeProperty);
            set => SetValue(PenModeProperty, value);
        }

        // Add draw mode

        public eliteSignature()
        {
            ResetStrokes();

            EnableTouchEvents = true;
        }

        public void Clear()
        {
            ResetStrokes();
        }

        void ResetStrokes()
        {
            _strokes = new List<SKPoint[]>();
            _points = new List<SKPoint>();
            Redraw();
        }

        void CreateSignaturePaint()
        {
            _signaturePaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = PenColor.ToSKColor(),
                StrokeWidth = PenThickness
            };
        }

        protected override void Draw(SKCanvas canvas, SKRect canvasBounds)
        {
            if (_awaitingSnapshot)
            {
                DrawSignature(canvas);

                var image = Surface.Snapshot();
                var data = image.Encode();
                _exportTcs.SetResult(data.AsStream());
                _awaitingSnapshot = false;

                canvas.Clear();
            }

            base.Draw(canvas, canvasBounds);

            DrawSignature(canvas);
        }

        void DrawSignature(SKCanvas canvas)
        {
            foreach (var stroke in _strokes)
            {
                canvas.DrawPoints(SKPointMode.Polygon, stroke, _signaturePaint);
            }

            var pointMode = SKPointMode.Polygon;
            switch (PenMode)
            {
                case SignaturePenMode.ActiveTrace:
                    pointMode = SKPointMode.Lines;
                    break;
                case SignaturePenMode.Solid:
                    pointMode = SKPointMode.Polygon;
                    break;
            }

            canvas.DrawPoints(pointMode, _points.ToArray(), _signaturePaint);
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    break;
                case SKTouchAction.Moved:
                    _points.Add(e.Location);
                    Redraw();
                    break;
                case SKTouchAction.Released:
                    _strokes.Add(_points.ToArray());
                    _points = new List<SKPoint>();
                    Redraw();
                    break;
                case SKTouchAction.Cancelled:
                    break;
            }

            e.Handled = true;
        }

        /// <summary>
        /// Returns a stream containing the current signature image data.
        /// </summary>
        /// <returns></returns>
        public Task<Stream> GetImageStream()
        {
            if (!_awaitingSnapshot)
            {
                _awaitingSnapshot = true;
                _exportTcs = new TaskCompletionSource<Stream>();

                Redraw();
            }

            return _exportTcs.Task;
        }
    }
}
