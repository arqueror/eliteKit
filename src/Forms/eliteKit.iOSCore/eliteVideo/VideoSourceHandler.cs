using eliteKit.Infrastructure.eliteVideo;
using eliteKit.Interfaces;

namespace eliteKit.iOSCore.eliteVideo
{
    internal static class VideoSourceHandler
    {
        /// <summary>
        /// Creates a source handler capable of loading the video resource.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <returns>A compatible source handler.</returns>
        public static IVideoSourceHandler Create(VideoSource source)
        {
            if (source is FileVideoSource)
                return new FileVideoSourceHandler();

            if (source is StreamVideoSource)
                return new StreamVideoSourceHandler();

            return new UriVideoSourceHandler();
        }
    }
}