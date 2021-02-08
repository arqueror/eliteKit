using eliteKit.eliteEnums;

namespace eliteKit.Models
{
    public class eliteVideoItem
    {
        public int VideoOrder { get; set; }
        public eliteVideoProvider VideoProvider { get; set; } = eliteVideoProvider.Default;
        public string VideoSource { get; set; } = null;
    }
}
