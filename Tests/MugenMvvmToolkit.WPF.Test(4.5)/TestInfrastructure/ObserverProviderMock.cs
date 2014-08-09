﻿using System;
using MugenMvvmToolkit.Binding.Interfaces;
using MugenMvvmToolkit.Binding.Interfaces.Models;

namespace MugenMvvmToolkit.Test.TestInfrastructure
{
    public class ObserverProviderMock : IObserverProvider
    {
        #region Properties

        public Func<object, IBindingPath, bool, IObserver> Observe { get; set; }

        public Func<object, IEventListener, IDisposable> ObserveParent { get; set; }

        #endregion

        #region Implementation of IObserverProvider

        /// <summary>
        ///     Attempts to track the value change using the binding path.
        /// </summary>
        IObserver IObserverProvider.Observe(object target, IBindingPath path, bool ignoreAttachedMembers)
        {
            return Observe(target, path, ignoreAttachedMembers);
        }

        /// <summary>
        ///     Attempts to track the change of parent.
        /// </summary>
        public IDisposable TryObserveParent(object target, IEventListener listener)
        {
            return ObserveParent(target, listener);
        }

        #endregion
    }
}