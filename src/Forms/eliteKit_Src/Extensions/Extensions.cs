using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace eliteKit.Extensions
{
    public static class Extensions
    {
        public static SKPoint ToPixelSKPoint(this Point pt, SKCanvasView canvasView)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        public static bool IsInsideCircle(this SKPoint location, SKPoint center, float radius)
        {
            if (radius < 0) return false;

            var distance = Math.Sqrt(Math.Pow((location.X - center.X), 2f) +
                                        Math.Pow((location.Y - center.Y), 2f));
            return distance < radius;
        }

        public static async Task<Stream> ToStreamAsync(this StreamImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageSource.Stream != null)
            {
                return await imageSource.Stream(cancellationToken);
            }
            return null;
        }

        public static Stream GetAssemblyResourceStream(this object type, string resourceId)
        {
            try
            {
                var assembly = type.GetType().GetTypeInfo().Assembly;
                return assembly.GetManifestResourceStream(resourceId);
            }
            catch { }
            return null;
        }
    }
}
