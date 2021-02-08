using System;

namespace eliteKit.Infrastructure.eliteVideo.Exceptions
{
    /// <summary>
    /// Thrown when YoutubeExplode fails to extract required information.
    /// This usually happens when YouTube makes changes that break YoutubeExplode.
    /// </summary>
    internal class UnrecognizedStructureException : Exception
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnrecognizedStructureException"/>.
        /// </summary>
        public UnrecognizedStructureException(string message)
            : base(message)
        {
        }
    }
}