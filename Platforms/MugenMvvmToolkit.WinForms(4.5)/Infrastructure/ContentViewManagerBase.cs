﻿#region Copyright
// ****************************************************************************
// <copyright file="ContentViewManagerBase.cs">
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
using MugenMvvmToolkit.Interfaces;

namespace MugenMvvmToolkit.Infrastructure
{
    /// <summary>
    ///     Represents the base class that allows to set content in the specified view.
    /// </summary>
    public abstract class ContentViewManagerBase<TView, TContent> : IContentViewManager
    {
        #region Implementation of IContentViewManager

        void IContentViewManager.SetContent(object view, object content)
        {
            SetContent((TView) view, (TContent) content);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the specified content.
        /// </summary>
        protected abstract void SetContent(TView view, TContent content);

        #endregion
    }
}