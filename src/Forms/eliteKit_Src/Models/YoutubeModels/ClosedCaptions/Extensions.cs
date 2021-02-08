using System;
using System.Linq;

namespace eliteKit.Models.YoutubeModels.ClosedCaptions
{
    /// <summary>
    /// Extensions for <see cref="ClosedCaptions"/>.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Gets caption displayed at the given point in time.
        /// Returns null if not found.
        /// </summary>
        public static ClosedCaption GetByTime(this ClosedCaptionTrack track, TimeSpan time) =>
            track.Captions.FirstOrDefault(c => time >= c.Offset && time <= c.Offset + c.Duration);
    }
}