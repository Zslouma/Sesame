using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "La presse international")]
    public partial class PressInternationalView : MvxContentPage<PressInternationalViewModel>
    {
        public PressInternationalView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.resultsListInternationalPress.ItemTapped += ResultsList_ItemTapped;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.resultsListInternationalPress.ItemTapped -= ResultsList_ItemTapped;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as PressInternationalViewModel).OnDisplayAlert += PressInternationalViewModel_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private async void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Result;

            await this.ViewModel.GoToDetailView(item);
        }

        private void PressInternationalViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}