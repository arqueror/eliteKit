using System.Collections.Generic;
using System.Runtime.Serialization;

namespace eliteKit.MarkupExtensions
{
    [DataContract]
    internal class VimeoVideo
    {
        [DataMember]
        public string cdn_url { get; set; }
        [DataMember]
        public int? view { get; set; }
        [DataMember]
        public Request request { get; set; }
        [DataMember]
        public string player_url { get; set; }
        [DataMember]
        public Video video { get; set; }
        [DataMember]
        public Build2 build { get; set; }
        [DataMember]
        public Embed embed { get; set; }
        [DataMember]
        public string vimeo_url { get; set; }
        [DataMember]
        public User user { get; set; }
    }
    [DataContract]
    public class Hls
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string origin { get; set; }
        [DataMember]
        public string cdn { get; set; }
    }
    [DataContract]
    public class Progressive
    {
        [DataMember]
        public int? profile { get; set; }
        [DataMember]
        public int? width { get; set; }
        [DataMember]
        public string mime { get; set; }
        [DataMember]
        public int? fps { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string cdn { get; set; }
        [DataMember]
        public string quality { get; set; }
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public string origin { get; set; }
        [DataMember]
        public int? height { get; set; }
    }
    [DataContract]
    public class Files
    {
        [DataMember]
        public Hls hls { get; set; }
        [DataMember]
        public List<Progressive> progressive { get; set; }
    }
    [DataContract]
    public class Cookie
    {
        [DataMember]
        public int? scaling { get; set; }
        [DataMember]
        public double volume { get; set; }
        [DataMember]
        public object quality { get; set; }
        [DataMember]
        public int? hd { get; set; }
        [DataMember]
        public object captions { get; set; }
    }

    [DataContract]
    public class Flags
    {
        [DataMember]
        public int? dnt { get; set; }
        [DataMember]
        public string preload_video { get; set; }
        [DataMember]
        public int? plays { get; set; }
        [DataMember]
        public int? webp { get; set; }
        [DataMember]
        public int? conviva { get; set; }
        [DataMember]
        public int? flash_hls { get; set; }
        [DataMember]
        public int? login { get; set; }
        [DataMember]
        public int? partials { get; set; }
        [DataMember]
        public int? blurr { get; set; }
    }
    [DataContract]
    public class Build
    {
        [DataMember]
        public string player { get; set; }
        [DataMember]
        public string js { get; set; }
    }
    [DataContract]
    public class Urls
    {
        [DataMember]
        public string zeroclip_swf { get; set; }
        [DataMember]
        public string js { get; set; }
        [DataMember]
        public string proxy { get; set; }
        [DataMember]
        public string flideo { get; set; }
        [DataMember]
        public string moog { get; set; }
        [DataMember]
        public string comscore_js { get; set; }
        [DataMember]
        public string blurr { get; set; }
        [DataMember]
        public string vuid_js { get; set; }
        [DataMember]
        public string moog_js { get; set; }
        [DataMember]
        public string zeroclip_js { get; set; }
        [DataMember]
        public string css { get; set; }
    }
    [DataContract]
    public class Request
    {
        [DataMember]
        public Files files { get; set; }
        [DataMember]
        public string ga_account { get; set; }
        [DataMember]
        public int? expires { get; set; }
        [DataMember]
        public int? timestamp { get; set; }
        [DataMember]
        public string signature { get; set; }
        [DataMember]
        public string currency { get; set; }
        [DataMember]
        public string session { get; set; }
        [DataMember]
        public Cookie cookie { get; set; }
        [DataMember]
        public string cookie_domain { get; set; }
        [DataMember]
        public object referrer { get; set; }
        [DataMember]
        public string comscore_id { get; set; }
        [DataMember]
        public Flags flags { get; set; }
        [DataMember]
        public Build build { get; set; }
        [DataMember]
        public Urls urls { get; set; }
        [DataMember]
        public string country { get; set; }
    }
    [DataContract]
    public class Rating
    {
        [DataMember]
        public int? id { get; set; }
    }
    [DataContract]
    public class Owner
    {
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string img { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string img_2x { get; set; }
        [DataMember]
        public int? id { get; set; }
    }
    [DataContract]
    public class Thumbs
    {
        [DataMember]
        public string __invalid_name__1280 { get; set; }
        [DataMember]
        public string __invalid_name__960 { get; set; }
        [DataMember]
        public string __invalid_name__640 { get; set; }
        [DataMember]
        public string @base { get; set; }
    }
    [DataContract]
    public class Video
    {
        [DataMember]
        public Rating rating { get; set; }
        [DataMember]
        public int? allow_hd { get; set; }
        [DataMember]
        public int? height { get; set; }
        [DataMember]
        public Owner owner { get; set; }
        [DataMember]
        public Thumbs thumbs { get; set; }
        [DataMember]
        public int? duration { get; set; }
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public int? hd { get; set; }
        [DataMember]
        public string embed_code { get; set; }
        [DataMember]
        public int? default_to_hd { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string privacy { get; set; }
        [DataMember]
        public string share_url { get; set; }
        [DataMember]
        public int? width { get; set; }
        [DataMember]
        public string embed_permission { get; set; }
        [DataMember]
        public double fps { get; set; }
    }
    [DataContract]
    public class Build2
    {
        [DataMember]
        public string player { get; set; }
        [DataMember]
        public string rpc { get; set; }
    }
    [DataContract]
    public class Settings
    {
        [DataMember]
        public int? fullscreen { get; set; }
        [DataMember]
        public int? byline { get; set; }
        [DataMember]
        public int? like { get; set; }
        [DataMember]
        public int? playbar { get; set; }
        [DataMember]
        public int? title { get; set; }
        [DataMember]
        public int? color { get; set; }
        [DataMember]
        public int? branding { get; set; }
        [DataMember]
        public int? share { get; set; }
        [DataMember]
        public int? scaling { get; set; }
        [DataMember]
        public int? logo { get; set; }
        [DataMember]
        public int? collections { get; set; }
        [DataMember]
        public int? info_on_pause { get; set; }
        [DataMember]
        public int? watch_later { get; set; }
        [DataMember]
        public int? portrait { get; set; }
        [DataMember]
        public int? embed { get; set; }
        [DataMember]
        public int? volume { get; set; }
    }
    [DataContract]
    public class Embed
    {
        [DataMember]
        public string player_id { get; set; }
        [DataMember]
        public string outro { get; set; }
        [DataMember]
        public int? api { get; set; }
        [DataMember]
        public string context { get; set; }
        [DataMember]
        public int? time { get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]
        public Settings settings { get; set; }
        [DataMember]
        public int? on_site { get; set; }
        [DataMember]
        public int? loop { get; set; }
        [DataMember]
        public int? autoplay { get; set; }
    }
    [DataContract]
    public class User
    {
        [DataMember]
        public int? liked { get; set; }
        [DataMember]
        public string account_type { get; set; }
        [DataMember]
        public int? progress { get; set; }
        [DataMember]
        public int? owner { get; set; }
        [DataMember]
        public int? watch_later { get; set; }
        [DataMember]
        public int? logged_in { get; set; }
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public int? mod { get; set; }
    }
}
