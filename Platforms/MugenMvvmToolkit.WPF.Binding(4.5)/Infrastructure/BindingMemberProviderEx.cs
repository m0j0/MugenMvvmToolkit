﻿#region Copyright

// ****************************************************************************
// <copyright file="BindingMemberProviderEx.cs">
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

using System;
using JetBrains.Annotations;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Binding.Infrastructure;
using MugenMvvmToolkit.Binding.Interfaces.Models;
#if WPF
using System.ComponentModel;
using System.Windows;
using MugenMvvmToolkit.WPF.Binding.Models;

namespace MugenMvvmToolkit.WPF.Binding.Infrastructure
#elif WINDOWS_UWP
using System.Reflection;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.UWP.Binding.Models;
using Windows.UI.Xaml;

namespace MugenMvvmToolkit.UWP.Binding.Infrastructure
#endif
{
    public class BindingMemberProviderEx : BindingMemberProvider
    {
        #region Overrides of BindingMemberProvider

        protected override IBindingMemberInfo GetExplicitBindingMember(Type sourceType, string path)
        {
            if (typeof(DependencyObject).IsAssignableFrom(sourceType))
            {
                var property = GetDependencyProperty(sourceType, path);
                if (property != null)
                {
                    IBindingMemberInfo updateEvent = BindingServiceProvider.UpdateEventFinder(sourceType, path);
#if WPF
                    return new DependencyPropertyBindingMember(property, path, property.PropertyType, property.ReadOnly, sourceType.GetProperty(path), updateEvent);
#else

                    var member = sourceType.GetPropertyEx(path);
                    Type type = typeof(object);
                    bool readOnly = false;
                    if (member != null)
                    {
                        type = member.PropertyType;
                        readOnly = !member.CanWrite;
                    }
                    return new DependencyPropertyBindingMember(property, path, type, readOnly, member, updateEvent);
#endif
                }
            }
            return base.GetExplicitBindingMember(sourceType, path);
        }

        #endregion

        #region Methods

        private static DependencyProperty GetDependencyProperty(Type type, string name)
        {
#if WPF            
            return DependencyPropertyDescriptor.FromName(name, type, type)?.DependencyProperty;
#else

            FieldInfo fieldInfo = type.GetFieldEx(name + "Property", MemberFlags.Public | MemberFlags.Static) ??
                                  type.GetFieldEx(name, MemberFlags.Public | MemberFlags.Static);
            var property = fieldInfo == null
                ? null
                : fieldInfo.GetValue(null) as DependencyProperty;
            if (property == null)
            {
                var prop = type.GetPropertyEx(name + "Property", MemberFlags.Public | MemberFlags.Static) ??
                                  type.GetPropertyEx(name, MemberFlags.Public | MemberFlags.Static);
                property = prop == null || !prop.CanRead
                    ? null
                    : prop.GetValue(null, null) as DependencyProperty;
            }
            return property;
#endif
        }

        #endregion
    }
}
