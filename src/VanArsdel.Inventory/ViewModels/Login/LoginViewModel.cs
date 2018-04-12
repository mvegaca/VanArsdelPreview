﻿using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel(IContext context, INavigationService navigationService) : base(context)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }

        public LoginViewState ViewState { get; private set; }

        public Task LoadAsync(LoginViewState viewState)
        {
            ViewState = viewState;
            return Task.CompletedTask;
        }
    }
}
