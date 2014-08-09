﻿#region Copyright
// ****************************************************************************
// <copyright file="IViewMappingItem.cs">
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
using JetBrains.Annotations;

namespace MugenMvvmToolkit.Interfaces.Models
{
    /// <summary>
    ///     Represents the interface which contains information about binding a view to a view model.
    /// </summary>
    public interface IViewMappingItem
    {
        /// <summary>
        ///     Gets the name of mapping.
        /// </summary>
        [CanBeNull]
        string Name { get; }

        /// <summary>
        ///     Gets the type of view.
        /// </summary>
        [NotNull]
        Type ViewType { get; }

        /// <summary>
        ///     Gets or sets the type of view model.
        /// </summary>
        [NotNull]
        Type ViewModelType { get; }

        /// <summary>
        ///     Gets the uri, if any.
        /// </summary>
        [NotNull]
        Uri Uri { get; }
    }
}