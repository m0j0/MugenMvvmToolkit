﻿#region Copyright

// ****************************************************************************
// <copyright file="PlatformType.cs">
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

namespace MugenMvvmToolkit.Models
{
    public class PlatformType : StringConstantBase<PlatformType>
    {
        #region Fields

        public static readonly PlatformType Android;

        public static readonly PlatformType iOS;

        public static readonly PlatformType WinForms;

        public static readonly PlatformType UWP;

        public static readonly PlatformType WPF;

        public static readonly PlatformType XamarinFormsAndroid;

        public static readonly PlatformType XamarinFormsiOS;

        public static readonly PlatformType XamarinFormsWinPhone;

        public static readonly PlatformType XamarinFormsWinRT;

        public static readonly PlatformType XamarinFormsUWP;

        public static readonly PlatformType Unknown;

        public static readonly PlatformType UnitTest;

        #endregion

        #region Constructors

        static PlatformType()
        {
            Android = new PlatformType(nameof(Android));
            iOS = new PlatformType(nameof(iOS));
            WinForms = new PlatformType(nameof(WinForms));
            UWP = new PlatformType(nameof(UWP));
            WPF = new PlatformType(nameof(WPF));
            XamarinFormsAndroid = new PlatformType(nameof(XamarinFormsAndroid));
            XamarinFormsiOS = new PlatformType(nameof(XamarinFormsiOS));
            XamarinFormsWinPhone = new PlatformType(nameof(XamarinFormsWinPhone));
            XamarinFormsWinRT = new PlatformType(nameof(XamarinFormsWinRT));
            XamarinFormsUWP = new PlatformType(nameof(XamarinFormsUWP));
            Unknown = new PlatformType("Unknown");
            UnitTest = new PlatformType("UnitTest");
        }

        public PlatformType(string id)
            : base(id)
        {
        }

        #endregion
    }
}
