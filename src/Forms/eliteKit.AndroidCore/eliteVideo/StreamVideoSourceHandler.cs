﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using eliteKit.Infrastructure.eliteVideo;
using eliteKit.Interfaces;

namespace eliteKit.AndroidCore.eliteVideo
{
    internal sealed class StreamVideoSourceHandler : IVideoSourceHandler
    {
        /// <summary>
        /// Loads the video from the specified source.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The path to the video file.</returns>
        public async Task<string> LoadVideoAsync(VideoSource source, CancellationToken cancellationToken = default(CancellationToken))
        {
            string path = null;
            var streamVideoSource = source as StreamVideoSource;

            if (streamVideoSource?.Stream != null)
            {
                using (var stream = await streamVideoSource.GetStreamAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (stream != null)
                    {
                        var tempFileName = GetFileName(stream, streamVideoSource.Format);
                        var tempDirectory = Path.Combine(Path.GetTempPath(), "MediaCache");
                        path = Path.Combine(tempDirectory, tempFileName);

                        if (!File.Exists(path))
                        {
                            if (!Directory.Exists(tempDirectory))
                                Directory.CreateDirectory(tempDirectory);

                            using (var tempFile = File.Create(path))
                            {
                                await stream.CopyToAsync(tempFile);
                            }
                        }
                    }
                }
            }

            return path;
        }

        /// <summary>
        /// Computes a file name based on a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
		private string GetFileName(Stream stream, string format)
        {
            var hasher = MD5.Create();
            var hashBytes = hasher.ComputeHash(stream);
            stream.Position = 0;
            var hash = Convert.ToBase64String(hashBytes)
                .Replace("=", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("/", string.Empty);
            return $"{hash}_temp.{format}";
        }
    }
}