using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Mes documents epinglés")]
    public partial class PinnedDocumentView : MvxContentPage<PinnedDocumentViewModel>
    {
        public PinnedDocumentView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.resultsListPinnedDocument.ItemTapped += ResultsList_ItemTapped;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.resultsListPinnedDocument.ItemTapped -= ResultsList_ItemTapped;
        }

        private async void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Result;

            await this.ViewModel.GoToDetailView(item);
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as PinnedDocumentViewModel).OnDisplayAlert += PinnedDocumentView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private void OnDownloadClick(object sender, EventArgs args)
        {

        }

        private void PinnedDocumentView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}