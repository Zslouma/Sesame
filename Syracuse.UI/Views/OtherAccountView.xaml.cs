using System;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "")]
    public partial class OtherAccountView : MvxContentPage<OtherAccountViewModel>
    {

        private TutorialPopupAddAccount _tutorialPopup;
        public OtherAccountView()
        {
            InitializeComponent();
            this.otherAccountList.ItemTapped += OtherAccountList_ItemTapped;
        }

        private async Task DisplayPopUp()
        {
            var database = Syracuse.Mobitheque.Core.App.Database;
            var user = await database.GetActiveUser();
            if (user != null)
            {
                Console.WriteLine("DisplayPopUp : " + user.IsTutorialAddAcount.ToString());
                if (user.IsTutorialAddAcount)
                {
                    _tutorialPopup = new TutorialPopupAddAccount(database, user);
                    await PopupNavigation.Instance.PushAsync(_tutorialPopup);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.DisplayPopUp();
        }

        private async void OtherAccountList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CookiesSave otherAccount = e.Item as CookiesSave;


            await this.ViewModel.ChangeAccount(otherAccount);
        }

        private async void AddAccount_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(ApplicationResource.Warning, ApplicationResource.DisconectAuthorization, ApplicationResource.Yes, ApplicationResource.No);
            if (answer)
            {
               await this.ViewModel.Disconnect(true);
            }
        }
        protected override void OnBindingContextChanged()
        {
            (this.DataContext as OtherAccountViewModel).OnDisplayAlert += OtherAccountView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        private void OtherAccountView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}
