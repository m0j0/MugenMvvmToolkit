﻿#region Copyright

// ****************************************************************************
// <copyright file="INavigationOperation.cs">
// Copyright (c) 2012-2016 Vyacheslav Volkov
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

using System.Threading.Tasks;

namespace MugenMvvmToolkit.Interfaces.Callbacks
{
    public interface INavigationOperation : IAsyncOperation<bool>
    {
        Task NavigationCompletedTask { get; }
    }
}
