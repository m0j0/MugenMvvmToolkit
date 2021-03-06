﻿#region Copyright

// ****************************************************************************
// <copyright file="XamarinFormsDesignBootstrapperBase.cs">
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
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Interfaces.ViewModels;
using MugenMvvmToolkit.Xamarin.Forms.Interfaces.Navigation;
using Xamarin.Forms;

namespace MugenMvvmToolkit.Xamarin.Forms.Infrastructure
{
    public abstract class XamarinFormsDesignBootstrapperBase : XamarinFormsBootstrapperBase
    {
        #region Constructors

        protected XamarinFormsDesignBootstrapperBase(IPlatformService platformService = null, bool isDesignMode = true) : base(platformService, isDesignMode)
        {
        }

        #endregion

        #region Methods

        public static void EnsureInitialized(Func<XamarinFormsDesignBootstrapperBase> factory)
        {
            Should.NotBeNull(factory, nameof(factory));
            if (Current == null)
                factory().Initialize();
        }

        public sealed override void Start()
        {
        }

        protected sealed override void InitializeRootPage(IViewModel viewModel, IDataContext context)
        {
            base.InitializeRootPage(viewModel, context);
        }

        protected sealed override NavigationPage CreateNavigationPage(Page mainPage)
        {
            return base.CreateNavigationPage(mainPage);
        }

        protected sealed override INavigationService CreateNavigationService()
        {
            return base.CreateNavigationService();
        }

        protected sealed override void OnStart()
        {
            base.OnStart();
        }

        #endregion
    }
}