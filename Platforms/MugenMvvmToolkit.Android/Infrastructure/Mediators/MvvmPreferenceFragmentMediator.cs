#region Copyright

// ****************************************************************************
// <copyright file="MvvmPreferenceFragmentMediator.cs">
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
using JetBrains.Annotations;

#if APPCOMPAT
using Android.Support.V4.App;
using Android.Support.V7.Preferences;
using MugenMvvmToolkit.Android.AppCompat.Infrastructure.Mediators;
using PreferenceFragment = Android.Support.V7.Preferences.PreferenceFragmentCompat;

namespace MugenMvvmToolkit.Android.PreferenceCompat.Infrastructure.Mediators
#else
using Android.App;
using Android.Preferences;
using Android.OS;
using MugenMvvmToolkit.Binding;

namespace MugenMvvmToolkit.Android.Infrastructure.Mediators
#endif
{
    public class MvvmPreferenceFragmentMediator : MvvmFragmentMediator
    {
        #region Fields

        private PreferenceChangeListener _preferenceChangeListener;
#if !APPCOMPAT
        private bool _isPreferenceContext;
#endif
        #endregion

        #region Constructors

        public MvvmPreferenceFragmentMediator([NotNull] Fragment target) : base(target)
        {
        }

        #endregion

        #region Properties

        protected PreferenceManager PreferenceManager => (Target as PreferenceFragment)?.PreferenceManager;

        #endregion

        #region Methods

        public override void OnPause(Action baseOnPause)
        {
            var manager = PreferenceManager;
            if (manager != null)
                if (_preferenceChangeListener != null)
                {
                    manager.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(_preferenceChangeListener);
                    _preferenceChangeListener.State = false;
                }
            base.OnPause(baseOnPause);
        }

#if !APPCOMPAT
        public override void OnSaveInstanceState(Bundle outState, Action<Bundle> baseOnSaveInstanceState)
        {
            if (_isPreferenceContext)
                baseOnSaveInstanceState(outState);
            else
                base.OnSaveInstanceState(outState, baseOnSaveInstanceState);
        }

        public override void OnCreate(Bundle savedInstanceState, Action<Bundle> baseOnCreate)
        {
            base.OnCreate(savedInstanceState, baseOnCreate);
            if (DataContext == null)
            {
                if ((savedInstanceState == null || !savedInstanceState.ContainsKey(IgnoreStateKey)) && Target is PreferenceFragment)
                {
                    var activity = Target.Activity as PreferenceActivity;
                    if (activity != null)
                    {
                        _isPreferenceContext = true;
                        Target.BindFromExpression(nameof(DataContext), activity, nameof(DataContext));
                    }
                }
            }
        }
#endif

        public override void OnDestroy(Action baseOnDestroy)
        {
            if (_preferenceChangeListener != null)
            {
                _preferenceChangeListener.Dispose();
                _preferenceChangeListener = null;
            }
            base.OnDestroy(baseOnDestroy);
        }

        public override void OnResume(Action baseOnResume)
        {
            base.OnResume(baseOnResume);
            PreferenceManager.InitializePreferenceListener(ref _preferenceChangeListener);
        }

        public override void AddPreferencesFromResource(Action<int> baseAddPreferencesFromResource, int preferencesResId)
        {
            var fragment = Target as PreferenceFragment;
            if (fragment == null)
            {
                Tracer.Error("The AddPreferencesFromResource method supported only for PreferenceFragment");
                return;
            }
            baseAddPreferencesFromResource(preferencesResId);
            InitializePreferences(fragment.PreferenceScreen, preferencesResId);
        }

        protected virtual void InitializePreferences(PreferenceScreen preferenceScreen, int preferencesResId)
        {
            PreferenceExtensions.InitializePreferences(preferenceScreen, preferencesResId, Target);
            PreferenceManager.InitializePreferenceListener(ref _preferenceChangeListener);
        }

        #endregion
    }
}