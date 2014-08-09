﻿#region Copyright
// ****************************************************************************
// <copyright file="BindingContextManagerEx.cs">
// Copyright © Vyacheslav Volkov 2012-2014
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
using MugenMvvmToolkit.Binding.Interfaces;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Models;
#if NETFX_CORE || WINDOWSCOMMON
using Windows.UI.Xaml;
using EventType = System.Object;
#else
using System.Windows;
using EventType = System.Windows.DependencyPropertyChangedEventArgs;
#endif
using MugenMvvmToolkit.Binding.Core;
using MugenMvvmToolkit.Binding.DataConstants;
using MugenMvvmToolkit.Binding.Interfaces.Models;
using MugenMvvmToolkit.Binding.Models;
using MugenMvvmToolkit.Binding.Models.EventArg;
using MugenMvvmToolkit.Interfaces;

namespace MugenMvvmToolkit.Binding.Infrastructure
{
    public class BindingContextManagerEx : BindingContextManager
    {
        #region Nested types

        private sealed class BindingContextSource : IBindingContext
#if WINDOWS_PHONE || NETFX_CORE
, IHandler<ValueChangedEventArgs>
#endif
        {
            #region Fields

#if WINDOWS_PHONE || NETFX_CORE
            private readonly IObserver _observer;
#else
            private readonly WeakReference _sourceReference;
#endif
            #endregion

            #region Constructors

            public BindingContextSource(FrameworkElement element)
            {
#if WINDOWS_PHONE || NETFX_CORE
                _observer = BindingProvider.Instance
                    .ObserverProvider
                    .Observe(element, BindingPath.DataContext, true);
                _observer.Listener = this;
#else        
                element.DataContextChanged += RaiseDataContextChanged;
                _sourceReference = MvvmExtensions.GetWeakReference(element);
#endif
            }

            #endregion

            #region Implementation of IBindingContext

            /// <summary>
            ///     Gets the source object.
            /// </summary>
            public object Source
            {
                get
                {
#if WINDOWS_PHONE || NETFX_CORE
                    return _observer.Source;
#else
                    return _sourceReference.Target;
#endif
                }
            }

            /// <summary>
            ///     Gets the data context.
            /// </summary>
            public object DataContext
            {
                get
                {
                    var target = (FrameworkElement)Source;
                    if (target == null)
                        return null;
                    object context = target.DataContext;
                    if (context == null || context.GetType().FullName.Equals("MS.Internal.NamedObject"))
                        return null;
                    return context;
                }
                set
                {
                    var target = (FrameworkElement)Source;
                    if (target == null)
                        return;
                    if (ReferenceEquals(value, BindingConstants.UnsetValue))
                        value = DependencyProperty.UnsetValue;
                    target.DataContext = value;
                }
            }

            /// <summary>
            ///     Occurs when the DataContext property changed.
            /// </summary>
            public event EventHandler<IBindingContext, EventArgs> DataContextChanged;

            #endregion

            #region Methods

#if WINDOWS_PHONE || NETFX_CORE
            void IHandler<ValueChangedEventArgs>.Handle(object sender, ValueChangedEventArgs message)
            {
                var handler = DataContextChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
#else
            private void RaiseDataContextChanged(object sender, EventType args)
            {
                var handler = DataContextChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
#endif
            #endregion
        }

        #endregion

        #region Overrides of BindingContextManager

        /// <summary>
        ///     Creates an instance of <see cref="IBindingContext" /> for the specified item.
        /// </summary>
        /// <returns>An instnace of <see cref="IBindingContext" />.</returns>
        protected override IBindingContext CreateBindingContext(object item)
        {
            var member = item as FrameworkElement;
            if (member == null)
                return base.CreateBindingContext(item);
            return new BindingContextSource(member);
        }

        #endregion
    }
}