using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace eliteKit.eliteElements
{
    public abstract class ElementBase : SKCanvasView
    {
        protected SKSurface Surface { get; private set; }

        public ElementBase()
        {
        }

        protected void Redraw()
        {
            InvalidateSurface();
        }

        protected abstract void Draw(SKCanvas canvas, SKRect canvasBounds);

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            Surface = e.Surface;
            var canvas = e.Surface.Canvas;
            var bounds = e.Info.Rect;

            canvas.Clear();

            Draw(canvas, bounds);
        }
    }
}
