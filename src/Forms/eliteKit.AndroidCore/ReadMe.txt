1.- Add You AccessWifiState, Internet, MediaContentControl and WakeLock permissions in order to use eliteVideo
2.- config androidManifest android:networkSecurityConfig="@xml/network_security_config". Android P requires to handle HTTP
3.- If you want to hide status bar. Put this code on MainActivity

		if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
		{
			// Kill status bar underlay added by FormsAppCompatActivity
			// Must be done before calling FormsAppCompatActivity.OnCreate()
			var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			if (statusBarHeightInfo == null)
			{
				statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("_statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			}
			statusBarHeightInfo?.SetValue(this, 0);
		}

		and before the base.OnCreate of the activity, then have

			this.Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.TurnScreenOn);