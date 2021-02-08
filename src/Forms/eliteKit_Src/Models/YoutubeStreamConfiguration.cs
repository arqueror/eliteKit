using Newtonsoft.Json.Linq;
using System;

namespace eliteKit.Models
{
    public class YouTubeStreamConfiguration
    {
        public string PlayerSourceUrl { get; }

        public string DashManifestUrl { get; }

        public string HlsManifestUrl { get; }

        public string MuxedStreamInfosUrlEncoded { get; }

        public string AdaptiveStreamInfosUrlEncoded { get; }

        public JToken MuxedStreamInfosJson { get; }

        public JToken AdaptiveStreamInfosJson { get; }

        public DateTimeOffset ValidUntil { get; }

        public YouTubeStreamConfiguration(string playerSourceUrl, string dashManifestUrl, string hlsManifestUrl,
            string muxedStreamInfosUrlEncoded, string adaptiveStreamInfosUrlEncoded, JToken muxedStreamInfosJson,
            JToken adaptiveStreamInfosJson, DateTimeOffset validUntil)
        {
            PlayerSourceUrl = playerSourceUrl;
            DashManifestUrl = dashManifestUrl;
            HlsManifestUrl = hlsManifestUrl;
            MuxedStreamInfosUrlEncoded = muxedStreamInfosUrlEncoded;
            AdaptiveStreamInfosUrlEncoded = adaptiveStreamInfosUrlEncoded;
            MuxedStreamInfosJson = muxedStreamInfosJson;
            AdaptiveStreamInfosJson = adaptiveStreamInfosJson;
            ValidUntil = validUntil;
        }
    }
}
