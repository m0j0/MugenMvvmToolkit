#region Copyright

// ****************************************************************************
// <copyright file="ModalViewMediator.cs">
// Copyright (c) 2012-2015 Vyacheslav Volkov
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

using System.ComponentModel;
using JetBrains.Annotations;
using MugenMvvmToolkit.DataConstants;
using MugenMvvmToolkit.Interfaces;
using MugenMvvmToolkit.Interfaces.Callbacks;
using MugenMvvmToolkit.Interfaces.Models;
using MugenMvvmToolkit.Interfaces.Navigation;
using MugenMvvmToolkit.Interfaces.ViewModels;
using MugenMvvmToolkit.Interfaces.Views;
using MugenMvvmToolkit.Models;
using MugenMvvmToolkit.ViewModels;
using Xamarin.Forms;

namespace MugenMvvmToolkit.Infrastructure.Mediators
{
    public class ModalViewMediator : WindowViewMediatorBase<IModalView>
    {
        #region Fields

        private readonly EventHandler<Page, CancelEventArgs> _backButtonHandler;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModalViewMediator" /> class.
        /// </summary>
        public ModalViewMediator([NotNull] IViewModel viewModel, [NotNull] IThreadManager threadManager,
            [NotNull] IViewManager viewManager, [NotNull] IWrapperManager wrapperManager, [NotNull] IOperationCallbackManager operationCallbackManager)
            : base(viewModel, threadManager, viewManager, wrapperManager, operationCallbackManager)
        {
            _backButtonHandler = ReflectionExtensions
                .CreateWeakDelegate<ModalViewMediator, CancelEventArgs, EventHandler<Page, CancelEventArgs>>(this,
                    (service, o, arg3) => service.OnBackButtonPressed((Page)o, arg3),
                    (o, handler) => XamarinFormsExtensions.BackButtonPressed -= handler, handler => handler.Handle);
            UseAnimations = true;
        }

        #endregion

        #region Properties

        public bool UseAnimations { get; set; }

        #endregion

        #region Methods

        private void OnBackButtonPressed(Page page, CancelEventArgs arg3)
        {
            if (View == page)
                OnViewClosing(page, arg3);
        }

        #endregion

        #region Overrides of WindowViewMediatorBase<IModalView>

        /// <summary>
        ///     Shows the view in the specified mode.
        /// </summary>
        protected override void ShowView(IModalView view, bool isDialog, IDataContext context)
        {
            var page = (Page)ViewModel
                .GetIocContainer(true)
                .Get<INavigationService>()
                .CurrentContent;
            bool animated;
            if (context.TryGetData(NavigationConstants.UseAnimations, out animated))
                ViewModel.Settings.State.AddOrUpdate(NavigationConstants.UseAnimations, animated);
            else
                animated = UseAnimations;
            page.Navigation.PushModalAsync(view.GetUnderlyingView<Page>(), animated);
        }

        /// <summary>
        ///     Initializes the specified view.
        /// </summary>
        protected override void InitializeView(IModalView view, IDataContext context)
        {
            var page = view.GetUnderlyingView<Page>();
            page.Disappearing += OnViewClosed;
            XamarinFormsExtensions.BackButtonPressed += _backButtonHandler;
        }

        /// <summary>
        ///     Clears the event subscribtions from the specified view.
        /// </summary>
        /// <param name="view">The specified window-view to dispose.</param>
        protected override void CleanupView(IModalView view)
        {
            var page = view.GetUnderlyingView<Page>();
            page.Disappearing -= OnViewClosed;
            XamarinFormsExtensions.BackButtonPressed -= _backButtonHandler;
        }

        /// <summary>
        ///     Closes the view.
        /// </summary>
        protected override void CloseView(IModalView view)
        {
            var page = view.GetUnderlyingView<Page>();
            bool animated;
            if (ViewModel.Settings.State.TryGetData(NavigationConstants.UseAnimations, out animated))
                ViewModel.Settings.State.Remove(NavigationConstants.UseAnimations);
            else
                animated = UseAnimations;
            page.Navigation.PopModalAsync(animated);
        }

        #endregion
    }
}