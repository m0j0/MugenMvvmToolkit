﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MugenMvvmToolkit.Binding;
using MugenMvvmToolkit.Binding.Interfaces.Models;

#if WPF
namespace MugenMvvmToolkit.WPF.Binding
#elif WINFORMS
namespace MugenMvvmToolkit.WinForms.Binding
#elif XAMARIN_FORMS && ANDROID
namespace MugenMvvmToolkit.Xamarin.Forms.Android.Binding
#elif XAMARIN_FORMS && TOUCH
namespace MugenMvvmToolkit.Xamarin.Forms.iOS.Binding
#elif XAMARIN_FORMS && WINDOWS_UWP
namespace MugenMvvmToolkit.Xamarin.Forms.UWP.Binding
#elif XAMARIN_FORMS && WINDOWS_PHONE
namespace MugenMvvmToolkit.Xamarin.Forms.WinPhone.Binding
#elif XAMARIN_FORMS && NETFX_CORE
namespace MugenMvvmToolkit.Xamarin.Forms.WinRT.Binding
#elif TOUCH
namespace MugenMvvmToolkit.iOS.Binding
#elif ANDROID
namespace MugenMvvmToolkit.Android.Binding
#elif WINDOWS_UWP
namespace MugenMvvmToolkit.UWP.Binding
#endif
{
    public static class BindingConverterExtensions
    {
#if !WINDOWS_UWP && !NETFX_CORE
        #region Nested types

        private sealed class MultiTypeConverter : TypeConverter
        {
        #region Fields

            private readonly TypeConverter _first;
            private readonly TypeConverter _second;

        #endregion

        #region Constructors

            public MultiTypeConverter(TypeConverter first, TypeConverter second)
            {
                _first = first;
                _second = second;
            }

        #endregion

        #region Overrides of TypeConverter

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return _first.CanConvertFrom(context, sourceType) || _second.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                var type = value.GetType();
                if (_first.CanConvertFrom(context, type))
                    return _first.ConvertFrom(context, culture, value);
                if (_second.CanConvertFrom(context, type))
                    return _second.ConvertFrom(context, culture, value);
                return base.ConvertFrom(context, culture, value);
            }

        #endregion
        }

        #endregion

        #region Fields

        private static readonly Dictionary<object, TypeConverter> MemberToTypeConverter;

        #endregion

        #region Constructors

        static BindingConverterExtensions()
        {
            MemberToTypeConverter = new Dictionary<object, TypeConverter>();
        }

        #endregion
#endif

        #region Methods

        public static object Convert(IBindingMemberInfo member, Type type, object value)
        {
            if (value == null)
                return type.GetDefaultValue();
            if (type.IsInstanceOfType(value))
                return value;

#if !WINDOWS_UWP && !NETFX_CORE
            var converter = GetTypeConverter(type, member.Member);
            if (converter != null && converter.CanConvertFrom(value.GetType()))
                return converter.ConvertFrom(value);
#endif

#if WINDOWS_UWP || NETFX_CORE
            if (BindingExtensions.IsConvertible(value))
#else
            if (value is IConvertible)
#endif
                return System.Convert.ChangeType(value, type.GetNonNullableType(), BindingServiceProvider.BindingCultureInfo());

#if WINDOWS_UWP || NETFX_CORE
            if (type.GetTypeInfo().IsEnum)
#else
            if (type.IsEnum)
#endif
            {
#if WINDOWS_UWP || NETFX_CORE
                var s = value as string;
                if (s != null)
                    return Enum.Parse(type, s, false);
#endif
                return Enum.ToObject(type, value);
            }

            if (type == typeof(string))
                return value.ToString();
            return value;
        }

#if !WINDOWS_UWP && !NETFX_CORE
        private static TypeConverter GetTypeConverter(Type type, MemberInfo member)
        {
            object key = member ?? type;
            lock (MemberToTypeConverter)
            {
                TypeConverter value;
                if (!MemberToTypeConverter.TryGetValue(key, out value))
                {
                    var memberValue = GetConverter(member);
#if WINDOWS_PHONE
                    value = null;
#else
                    value = TypeDescriptor.GetConverter(type);
#endif
                    if (value != null && memberValue != null)
                        value = new MultiTypeConverter(memberValue, value);
                    else if (value == null)
                        value = memberValue;
                    MemberToTypeConverter[key] = value;
                }
                return value;
            }
        }

        private static TypeConverter GetConverter(MemberInfo member)
        {
            var attribute = member?.GetCustomAttributes(typeof(TypeConverterAttribute), true)
                .OfType<TypeConverterAttribute>()
                .FirstOrDefault();
            if (attribute == null)
                return null;
            var constructor = Type.GetType(attribute.ConverterTypeName, false)?.GetConstructor(Empty.Array<Type>());
            return constructor?.Invoke(Empty.Array<object>()) as TypeConverter;
        }
#endif
        private static Type GetNonNullableType(this Type type)
        {
            return IsNullableType(type) ? type.GetGenericArguments()[0] : type;
        }

        private static bool IsNullableType(this Type type)
        {
#if WINDOWS_UWP || NETFX_CORE
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
#else
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
#endif
        }

        #endregion
    }
}