﻿using System;
using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using VanArsdel.Inventory.ViewModels;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.Views
{
    public sealed partial class OrderItemsView : Page
    {
        public OrderItemsView()
        {
            ViewModel = new OrderItemsViewModel(new DataProviderFactory());
            InitializeComponent();
        }

        public OrderItemsViewModel ViewModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.OrderItemList.PropertyChanged += OnViewModelPropertyChanged;
            await ViewModel.LoadAsync(e.Parameter as OrderItemsViewState);
            UpdateTitle();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            ViewModel.CancelEdit();
            ViewModel.SaveState();
            ViewModel.OrderItemList.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.OrderItemList.Title))
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            this.SetTitle($"OrderItems {ViewModel.OrderItemList.Title}".Trim());
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await ViewModel.RefreshAsync();
        }

        private async void OpenInNewView(object sender, RoutedEventArgs e)
        {
            await ViewManager.Current.CreateNewView(typeof(OrderItemsView), ViewModel.OrderItemList.GetCurrentState());
        }

        private async void OpenDetailsInNewView(object sender, RoutedEventArgs e)
        {
            ViewModel.OrderItemDetails.IsEditMode = false;
            await ViewManager.Current.CreateNewView(typeof(OrderItemView), new OrderItemViewState(ViewModel.OrderItemDetails.Item.OrderID) { OrderLine = ViewModel.OrderItemDetails.Item.OrderLine });
        }

        public int GetRowSpan(bool isMultipleSelection)
        {
            return isMultipleSelection ? 2 : 1;
        }
    }
}
