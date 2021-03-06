﻿#region Copyright

// ****************************************************************************
// <copyright file="TestExtensions.cs">
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
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models;
using Should.Core.Exceptions;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
}

namespace Microsoft.VisualStudio.TestPlatform.UnitTestFramework
{
}

namespace MugenMvvmToolkit.Silverlight.Binding.Converters
{
}

namespace MugenMvvmToolkit.Silverlight.Binding.Modules
{
}

namespace MugenMvvmToolkit.Silverlight.Infrastructure
{
}

namespace MugenMvvmToolkit.Silverlight.Infrastructure.Mediators
{
}

namespace MugenMvvmToolkit.Silverlight
{
}

namespace MugenMvvmToolkit.Silverlight.Interfaces.Views
{
}

namespace MugenMvvmToolkit.Silverlight.Infrastructure.Navigation
{
}

namespace MugenMvvmToolkit.Silverlight.Interfaces.Navigation
{
}

namespace MugenMvvmToolkit.Silverlight.Models.EventArg
{
}

namespace MugenMvvmToolkit.WPF.Infrastructure.Mediators
{
}

namespace MugenMvvmToolkit.WPF.Binding.Converters
{
}

namespace MugenMvvmToolkit.WPF.Binding.Modules
{
}

namespace MugenMvvmToolkit.WPF.Infrastructure
{
}

namespace MugenMvvmToolkit.WPF
{
}

namespace MugenMvvmToolkit.WPF.Interfaces.Views
{
}

namespace MugenMvvmToolkit.WPF.Infrastructure.Navigation
{
}

namespace MugenMvvmToolkit.WPF.Interfaces.Navigation
{
}

namespace MugenMvvmToolkit.WPF.Models.EventArg
{
}

namespace MugenMvvmToolkit.UWP.Infrastructure.Mediators
{
}

namespace MugenMvvmToolkit.UWP.Binding.Converters
{
}

namespace MugenMvvmToolkit.UWP.Binding.Modules
{
}

namespace MugenMvvmToolkit.UWP.Infrastructure
{
}

namespace MugenMvvmToolkit.UWP
{
}

namespace MugenMvvmToolkit.UWP.Interfaces.Views
{
}

namespace MugenMvvmToolkit.UWP.Infrastructure.Navigation
{
}

namespace MugenMvvmToolkit.UWP.Interfaces.Navigation
{
}

namespace MugenMvvmToolkit.UWP.Models.EventArg
{
}

namespace MugenMvvmToolkit.UWP.Infrastructure.Callbacks
{
}

namespace MugenMvvmToolkit.WPF.Infrastructure.Callbacks
{
}

namespace MugenMvvmToolkit.Silverlight.Infrastructure.Callbacks
{
}

namespace MugenMvvmToolkit
{
    public static class TestExtensions
    {
        public static readonly IList<string> TestStrings = new[] { "test1", "test2", "test3", "test4" };

        public static void ShouldThrow(this Action action)
        {
            bool isThrow = false;
            try
            {
                action();
            }
            catch (Exception)
            {
                isThrow = true;
            }
            if (!isThrow)
                throw new AssertException();
        }

        public static IList<object> GetObservers(this IEventAggregator aggregator)
        {
            return aggregator.GetSubscribers().Select(subscriber => subscriber.Target).ToList();
        }

        public static T GetDataTest<T>(this IDataContext context, DataConstant<T> constant)
        {
            T data;
            if (context.TryGetData(constant, out data))
                return data;
            return default(T);
        }
    }
}
