using eliteKit.Interfaces;
using eliteKit.iOSCore.eliteVideo;
using Foundation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(PlatformFeatures))]
namespace eliteKit.iOSCore.eliteVideo
{
    /// <summary>
    /// Platform native methods needed in cross-platform code.
    /// </summary>

    internal class PlatformFeatures : IPlatformFeatures
    {
        #region Properties

        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        public string PackageName => NSBundle.MainBundle.BundleIdentifier;

        #endregion

        #region Methods

        /// <summary>
        /// Hashes the specified value using SHA-1.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>
        /// A base-64 encoded SHA-1 hash.
        /// </returns>
        public string HashSha1(string value)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        /// Exits the mobile application.
        /// </summary>
        public void Exit()
        {
            NSThread.MainThread.Cancel();
        }

        #endregion
    }
}