using System;
using System.Collections.Generic;
using System.Text;

namespace eliteKit.eliteEnums
{
    public enum eliteFillMode
    {
        /// <summary>
        /// The video stretches to fill the layer’s bounds.
        /// </summary>
        Resize,

        /// <summary>
        /// The video’s aspect ratio is preserved and fits the video within the layer’s bounds.
        /// </summary>
        ResizeAspect,

        /// <summary>
        /// The video’s aspect ratio is preserved and fills the layer’s bounds.
        /// </summary>
        ResizeAspectFill
    }
}
