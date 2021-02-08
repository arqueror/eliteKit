using System;
using System.Linq;
using SkiaSharp;

namespace eliteKit.Infrastructure.eliteQRCode
{
    /// <summary>
    /// QRCode renderer for SkiaSharp.
    /// </summary>
    internal class QRCodeRenderer : IDisposable
    {
        public SKPaint Paint { get; } = new SKPaint();

        public void Render(SKCanvas canvas, SKRect area, QRCodeData data)
        {
            if (data != null)
            {
                var rows = data.ModuleMatrix.Count;
                var columns = data.ModuleMatrix.Select(x => x.Count).Max();
                var cellHeight = area.Height / rows;
                var cellWidth = area.Width / columns;

                for (int y = 0; y < rows; y++)
                {
                    var row = data.ModuleMatrix.ElementAt(y);
                    for (int x = 0; x < row.Count; x++)
                    {
                        if (row[x])
                        {
                            var rect = SKRect.Create(area.Left + x * cellWidth, area.Top + y * cellHeight, cellWidth, cellHeight);
                            canvas.DrawRect(rect, Paint);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Paint.Dispose();
        }
    }
}
