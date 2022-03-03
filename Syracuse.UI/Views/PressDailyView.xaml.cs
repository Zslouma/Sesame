﻿using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "La presse journalière")]
    public partial class PressDailyView : MvxContentPage<PressDailyViewModel>
    {
        public PressDailyView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as PressDailyViewModel).OnDisplayAlert += PressDailyViewModel_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private async void ResultsList_ItemTapped(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var item = e.CurrentSelection[0] as Result;
                await this.ViewModel.GoToDetailView(item);
            }
            else
            {
                await this.DisplayAlert("Erreur", "Une erreur est survenue", "Ok");
            }
        }
        public async void HandleSwitchToggledByUser(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                this.ViewModel.DownloadAllDocument(this.ViewModel.Results);
            }
        }

        private void PressDailyViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}