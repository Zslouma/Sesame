using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Accueil")]
    public partial class HomeView : MvxContentPage<HomeViewModel>
    {

        public HomeView()
        {
            InitializeComponent();

            this.resultsList.ItemTapped += ResultsList_ItemTapped;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as HomeViewModel).OnDisplayAlert += HomeViewModel_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private async void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Result;

            await this.ViewModel.GoToDetailView(item);
        }

        private void HomeViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}
