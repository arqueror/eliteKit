using System;
using Android.Content;
using eliteKit.AndroidCore.eliteVideo;

namespace eliteKit.AndroidCore
{
    public class Abstractions
    {
        public static void Init(Context activity)
        {
            //CrossMediaManager.Current.Init(activity);
            FormsVideoPlayer.Init();
        }
    }
}
