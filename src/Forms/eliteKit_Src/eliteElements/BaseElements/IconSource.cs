using System;
using System.IO;
using eliteKit.eliteEnums;

namespace eliteKit.eliteElements.BaseElements
{
    public class IconSource
    {
        public Stream Stream { get; set; }
        public string GlyphValue { get; set; }
        public string FileName { get; set; }

        public IconType IconType =>
            !string.IsNullOrEmpty(GlyphValue) ? IconType.Fa :
            Stream == null ? IconType.None :
            FileName.EndsWith(".svg") ? IconType.Svg :
            IconType.Bitmap;
    }
}
