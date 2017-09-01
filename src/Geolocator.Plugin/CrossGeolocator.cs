﻿using Plugin.Geolocator.Abstractions;
using System;
#if __ANDROID__
using Android.Gms.Common;
using Android.App;
#endif

namespace Plugin.Geolocator
{
    /// <summary>
    /// Cross platform Geolocator implemenations
    /// </summary>
    public class CrossGeolocator
    {
        static Lazy<IGeolocator> implementation = new Lazy<IGeolocator>(() => CreateGeolocator(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value != null;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static IGeolocator Current
        {
            get
            {
                var ret = implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

#if __ANDROID__
		public static bool UseFusedLocationProvider { get; set; } = true;
#endif

		static IGeolocator CreateGeolocator()
        {
#if NETSTANDARD1_0
            return null;
#elif __ANDROID__
            if (UseFusedLocationProvider && GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Application.Context) == ConnectionResult.Success)
                return new FusedGeolocatorImplementation();

		    return new GeolocatorImplementation();
#else
		    return new GeolocatorImplementation();
#endif
		}

		internal static Exception NotImplementedInReferenceAssembly() =>
			new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        
    }
}
