using eliteKit.Diagnostics;
using eliteKit.Interfaces;
using UIKit;
using Xamarin.Forms;

namespace eliteKit.iOSCore.eliteVideo
{
    /// <summary>
    /// Initializes the VideoPlayer component for use with Xamarin Forms.
    /// Make sure to call <c>FormsVideoPlayer.Init();</c> after the <c>Forms.Init()</c> call in each project of your solution.
    /// </summary>
    internal sealed class FormsVideoPlayer
    {
        #region Properties

        /// <summary>
        /// Indicates whether the VideoPlayer component is initialized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInitialized { get; internal set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the VideoPlayer component for use with Xamarin Forms.
        /// This method should be invoked after the Forms.Init() call during application startup.
        /// </summary>
        public static void Init()
        {
            if (!IsInitialized)
            {
                Log.Info($"Initializing Xamarin Forms Video Player on {UIDevice.CurrentDevice.Model} v{UIDevice.CurrentDevice.SystemVersion}");
                // DependencyService.Register<IPlatformFeatures, PlatformFeatures>();
                IsInitialized = true;
            }
        }

        #endregion
    }
}