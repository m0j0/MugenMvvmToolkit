﻿#region Copyright

// ****************************************************************************
// <copyright file="IMvvmApplication.cs">
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
using JetBrains.Annotations;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models;

namespace MugenMvvmToolkit.Interfaces
{
    public interface IMvvmApplication
    {
        bool IsInitialized { get; }

        [NotNull]
        PlatformInfo PlatformInfo { get; }

        LoadMode Mode { get; }

        IIocContainer IocContainer { get; }

        [NotNull]
        IDataContext Context { get; }

        void Initialize(PlatformInfo platformInfo, IIocContainer iocContainer, IList<Assembly> assemblies, IDataContext context);

        void Start();

        [NotNull]
        Type GetStartViewModelType();
    }
}
