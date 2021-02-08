using eliteKit.Infrastructure.eliteVideo;
using eliteKit.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace eliteKit.iOSCore.eliteVideo
{
    internal sealed class UriVideoSourceHandler : IVideoSourceHandler
    {
        /// <summary>
        /// Loads the video from the specified source.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The path to the video file.</returns>
        public Task<string> LoadVideoAsync(VideoSource source, CancellationToken cancellationToken = default(CancellationToken))
        {
            string path = null;
            var uriVideoSource = source as UriVideoSource;

            if (uriVideoSource?.Uri != null)
            {
                path = uriVideoSource.Uri.AbsoluteUri;
            }

            return Task.FromResult(path);
        }
    }
}