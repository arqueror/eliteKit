using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using eliteKit.eliteCore;
using eliteKit.eliteEnums;
using eliteKit.Infrastructure.eliteVideo;
using eliteKit.Models;
using Xamarin.Forms;

namespace eliteKit.MarkupExtensions
{
    /// <summary>
    /// Converts a Vimeo video ID into a direct URL that is playable by the Xamarin Forms VideoPlayer.
    /// </summary>
    [ContentProperty("VideoId")]
    internal class VimeoVideoIdExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the video identifier.
        /// </summary>
        /// <value>
        /// The video identifier.
        /// </value>
        public string VideoId { get; set; }

        /// <summary>
        /// Gets or sets the video quality.
        /// </summary>
        /// <value>
        /// The video preferred quality.
        /// </value>
        public eliteVideoQuality VideoQuality { get; set; } = eliteVideoQuality.Low;

        #endregion

        #region IMarkupExtension

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public object ProvideValue(ref List<eliteVideoQuality> availableQualities, ref eliteVideoQuality currentVideoQuality)
        {
            try
            {
                Debug.WriteLine($"Acquiring Vimeo stream source URL from VideoId='{VideoId}'...");
                var videoInfoUrl = $"https://player.vimeo.com/video/{VideoId}/config";

                using (var client = new HttpClient())
                {
                    var videoPageContent = client.GetStringAsync(videoInfoUrl).Result;
                    var videoPageBytes = Encoding.UTF8.GetBytes(videoPageContent);

                    using (var stream = new MemoryStream(videoPageBytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(VimeoVideo));
                        var metaData = (VimeoVideo)serializer.ReadObject(stream);
                        var files = metaData.request.files.progressive;

                        var videoUrl = "";
                        var videos = files.OrderBy(s => s.width).Select(s => new VideoQualityModel { Quality = s.quality, Url = s.url }).ToList();


                        var countFlag = 0;
                        foreach (var videoQualityModel in videos)
                        {
                            if (videoQualityModel.Quality.Contains(((int)eliteVideoQuality.Low).ToString()))
                            {
                                availableQualities.Add(eliteVideoQuality.Low);
                            }
                            else if (videoQualityModel.Quality.Contains(((int)eliteVideoQuality.Normal).ToString()))
                            {
                                availableQualities.Add(eliteVideoQuality.Normal);
                            }
                            else if (videoQualityModel.Quality.Contains(((int)eliteVideoQuality.Medium).ToString()))
                            {
                                availableQualities.Add(eliteVideoQuality.Medium);
                            }
                            else if (videoQualityModel.Quality.Contains(((int)eliteVideoQuality.High).ToString()))
                            {
                                availableQualities.Add(eliteVideoQuality.High);
                            }
                        }

                        // Exact match
                        if (videos.Any())
                        {
                            //try to find preferred quality
                            var prefQuality = videos.FirstOrDefault(v => v.Quality.Contains(VideoQuality.ToString()));
                            if (prefQuality != null)
                            {
                                prefQuality.Quality = Regex.Replace(prefQuality.Quality, "[^0-9]", "");
                                videoUrl = prefQuality.Url;
                                currentVideoQuality = (eliteVideoQuality)Enum.Parse(typeof(eliteVideoQuality), prefQuality.Quality);
                                return VideoSource.FromUri(videoUrl);
                            }

                            //Not found.. Pick first quality in list and expose to caller available qualities
                            videoUrl = videos.First().Url;
                            var finalQuality = Regex.Replace(videos.First().Quality, "[^0-9]", "");
                            currentVideoQuality = (eliteVideoQuality)Enum.Parse(typeof(eliteVideoQuality), finalQuality);
                            Debug.WriteLine($"Stream URL: {videoUrl}");

                            //Make sure we can access URL.
                            if (!string.IsNullOrEmpty(videoUrl))
                            {
                                try
                                {
                                    var request = new HttpRequestMessage(HttpMethod.Head, videoUrl);
                                    var result = coreSettings.HttpClientSingleton.SendAsync(request).Result;
                                    if (!result.IsSuccessStatusCode)
                                    {
                                        return null;
                                    }

                                }
                                catch
                                {
                                    return null;
                                }

                            }

                            return VideoSource.FromUri(videoUrl);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured while attempting to convert Vimeo video ID into a remote stream path.");
                Debug.WriteLine(ex);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert the specified video ID into a streamable Vimeo URL.
        /// </summary>
        /// <param name="videoId">Video identifier.</param>
        /// <returns></returns>
        public static VideoSource Convert(string videoId, eliteVideoQuality quality, ref List<eliteVideoQuality> availableQualities, ref eliteVideoQuality currentVideoQuality)
        {
            var markupExtension = new VimeoVideoIdExtension { VideoId = videoId, VideoQuality = quality };
            return (VideoSource)markupExtension.ProvideValue(ref availableQualities, ref currentVideoQuality);
        }

        #endregion
    }
}
