﻿#region Copyright

// ****************************************************************************
// <copyright file="PlatformBootstrapperService.cs">
// Copyright (c) 2012-2017 Vyacheslav Volkov
// </copyright>
// ****************************************************************************
// <author>Vyacheslav Volkov</author>
// <email>vvs0205@outlook.com</email>
// <project>MugenMvvmToolkit</project>
// <web>https://github.com/MugenMvvmToolkit/MugenMvvmToolkit</web>
// <license>
// See license.txt in this solution or http://opensource.org/licenses/MS-PL
// </license>
// ****************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.Xamarin.Forms.Binding;
using MugenMvvmToolkit.Xamarin.Forms.Infrastructure;

#if WINDOWS_PHONE
namespace MugenMvvmToolkit.Xamarin.Forms.WinPhone
#elif TOUCH
namespace MugenMvvmToolkit.Xamarin.Forms.iOS
#elif ANDROID
using Android.Content;
namespace MugenMvvmToolkit.Xamarin.Forms.Android
#elif WINDOWS_UWP
using MugenMvvmToolkit.Models.Messages;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

namespace MugenMvvmToolkit.Xamarin.Forms.UWP
#elif NETFX_CORE
using System.IO;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;

namespace MugenMvvmToolkit.Xamarin.Forms.WinRT
#endif
{
    public class PlatformBootstrapperService : XamarinFormsBootstrapperBase.IPlatformService
    {
        #region Constructors
#if ANDROID
        public PlatformBootstrapperService(Func<Context> getCurrentContext)
        {
            PlatformExtensions.GetCurrentContext = getCurrentContext;
        }
#endif
        #endregion

        #region Methods

#if WINDOWS_UWP
        private void OnLeavingBackground(object sender, LeavingBackgroundEventArgs leavingBackgroundEventArgs)
        {
            if (ServiceProvider.IsInitialized)
                ServiceProvider.EventAggregator.Publish(this, new ForegroundNavigationMessage());
        }

        private void OnEnteredBackground(object sender, EnteredBackgroundEventArgs enteredBackgroundEventArgs)
        {
            if (ServiceProvider.IsInitialized)
                ServiceProvider.EventAggregator.Publish(this, new BackgroundNavigationMessage());
        }
#endif
        #endregion

        #region Implementation of IPlatformService

        public PlatformInfo GetPlatformInfo()
        {
#if WINDOWS_PHONE
            return new PlatformInfo(PlatformType.XamarinFormsWinPhone, Environment.OSVersion.Version.ToString());
#elif TOUCH            
            return new PlatformInfo(PlatformType.XamarinFormsiOS, UIKit.UIDevice.CurrentDevice.SystemVersion);
#elif ANDROID            
            return new PlatformInfo(PlatformType.XamarinFormsAndroid, global::Android.OS.Build.VERSION.Release);
#elif WINDOWS_UWP
            // get the system version number
            var deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            var version = ulong.Parse(deviceFamilyVersion);
            var majorVersion = (version & 0xFFFF000000000000L) >> 48;
            var minorVersion = (version & 0x0000FFFF00000000L) >> 32;
            var buildVersion = (version & 0x00000000FFFF0000L) >> 16;
            var revisionVersion = (version & 0x000000000000FFFFL);
            var isPhone = new EasClientDeviceInformation().OperatingSystem.SafeContains("WindowsPhone", StringComparison.OrdinalIgnoreCase);
            return new PlatformInfo(isPhone ? PlatformType.XamarinFormsUWPPhone : PlatformType.XamarinFormsUWP, new Version((int)majorVersion, (int)minorVersion, (int)buildVersion, (int)revisionVersion).ToString());
#elif NETFX_CORE            
            var isPhone = new EasClientDeviceInformation().OperatingSystem.SafeContains("WindowsPhone", StringComparison.OrdinalIgnoreCase);
            var isWinRT10 = typeof(DependencyObject).GetMethodEx("RegisterPropertyChangedCallback", MemberFlags.Instance | MemberFlags.Public) != null;
            var version = isWinRT10 ? new Version(10, 0) : new Version(8, 1);
            return new PlatformInfo(isPhone ? PlatformType.XamarinFormsWinRTPhone : PlatformType.XamarinFormsWinRT, version.ToString());
#endif
        }

        public ICollection<Assembly> GetAssemblies()
        {
#if WINDOWS_PHONE
            var assemblies = new HashSet<Assembly>();
            foreach (var part in System.Windows.Deployment.Current.Parts)
            {
                string assemblyName = part.Source.Replace(".dll", string.Empty);
                if (assemblyName.Contains("/"))
                    continue;
                try
                {
                    assemblies.Add(Assembly.Load(assemblyName));
                }
                catch (Exception e)
                {
                    Tracer.Error(e.Flatten(true));
                }
            }
            return assemblies;
#elif WINDOWS_UWP || NETFX_CORE
            return new[] { typeof(PlatformBootstrapperService).GetAssembly() };
#else
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        }

        public void Initialize()
        {
#if WINDOWS_UWP
            if (ApiInformation.IsEventPresent("Windows.UI.Xaml.Application", "EnteredBackground"))
                Application.Current.EnteredBackground += OnEnteredBackground;
            if (ApiInformation.IsEventPresent("Windows.UI.Xaml.Application", "LeavingBackground"))
                Application.Current.LeavingBackground += OnLeavingBackground;
#endif
        }

        public Func<MemberInfo, Type, object, object> ValueConverter => BindingConverterExtensions.Convert;

        #endregion
    }
}
