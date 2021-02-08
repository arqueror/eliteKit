﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace eliteKit.AndroidCore.eliteVideo
{
    internal class VideoViewSavedState : global::Android.Views.View.BaseSavedState
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current position.
        /// </summary>
        /// <value>
        /// The current position.
        /// </value>
        public int CurrentPosition { get; set; }

        #endregion

        #region Constructors

        public VideoViewSavedState(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public VideoViewSavedState(Parcel source) : base(source)
        {
            CurrentPosition = source.ReadInt();
        }

        public VideoViewSavedState(IParcelable superState) : base(superState)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Writes data to the parcel.
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="flags"></param>
        public override void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            base.WriteToParcel(dest, flags);
            dest.WriteInt(CurrentPosition);
        }

        #endregion
    }
}