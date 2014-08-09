﻿#region Copyright
// ****************************************************************************
// <copyright file="XmlTokens.cs">
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
using MugenMvvmToolkit.Binding.Models;

namespace MugenMvvmToolkit.Binding.Parse
{
    internal static class XmlTokens
    {
        public static readonly TokenType StartComment = new TokenType("StartComment", "<!--");

        public static readonly TokenType EndComment = new TokenType("EndComment", "-->");

        public static readonly TokenType CloseElement = new TokenType("CloseElement", "/>");

        public static readonly TokenType ComplexCloseElement = new TokenType("ComplexCloseElement", "</");
    }
}