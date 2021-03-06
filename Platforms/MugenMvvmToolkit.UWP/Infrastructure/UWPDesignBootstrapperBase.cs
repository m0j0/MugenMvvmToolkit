﻿#region Copyright

// ****************************************************************************
// <copyright file="UwpDesignBootstrapperBase.cs">
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

using Windows.UI.Xaml.Controls;
using MugenMvvmToolkit.UWP.Interfaces.Navigation;

namespace MugenMvvmToolkit.UWP.Infrastructure
{
    public abstract class UwpDesignBootstrapperBase : UwpBootstrapperBase
    {
        #region Constructors

        protected UwpDesignBootstrapperBase() : base(ServiceProvider.IsDesignMode)
        {
        }

        #endregion

        #region Methods

        public sealed override void Start()
        {
            base.Start();
        }

        protected sealed override INavigationService CreateNavigationService(Frame frame)
        {
            return base.CreateNavigationService(frame);
        }

        #endregion
    }
}