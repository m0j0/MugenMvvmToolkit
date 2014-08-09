﻿#region Copyright
// ****************************************************************************
// <copyright file="IEntityStateManager.cs">
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
using JetBrains.Annotations;
using MugenMvvmToolkit.Interfaces.Models;

namespace MugenMvvmToolkit.Interfaces
{
    /// <summary>
    ///     Represents the interface that provides methods to manage entity state.
    /// </summary>
    public interface IEntityStateManager
    {
        /// <summary>
        ///     Creates an instance of <see cref="IEntitySnapshot" />
        /// </summary>
        /// <param name="entity">The specified entity to create snapshot.</param>
        /// <returns>An instance of <see cref="IEntitySnapshot" /></returns>
        IEntitySnapshot CreateSnapshot([NotNull] object entity);
    }
}