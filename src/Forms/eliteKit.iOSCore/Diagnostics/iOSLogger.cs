using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eliteKit.Interfaces;
using eliteKit.iOSCore.Diagnostics;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(iOSLogger))]
namespace eliteKit.iOSCore.Diagnostics
{
    /// <summary>
    /// Platform specific logging implementation.
    /// </summary>
    internal class iOSLogger : ILogger
    {
        #region Fields

        /// <summary>
        /// The prefix used to denote the log entry subject.
        /// </summary>
        private string _logEntryPrefix = "VideoPlayer";

        #endregion

        #region Methods

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Console.WriteLine($"{_logEntryPrefix} [INFO]: {message}");
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            Console.WriteLine($"{_logEntryPrefix} [WARN]: {message}");
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            Console.WriteLine($"{_logEntryPrefix} [ERROR]: {message}");
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            Console.WriteLine($"{_logEntryPrefix} [ERROR]: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public void Error(Exception exception, string message)
        {
            Console.WriteLine($"{_logEntryPrefix} [ERROR]: {message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }

        #endregion
    }
}