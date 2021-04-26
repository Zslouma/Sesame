using System;
using System.Diagnostics;
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
        private SwitchOrDeleteAccountPopup _changeOrDeleteAccountPopup;
        public OtherAccountView()
        {
            InitializeComponent();
            this.otherAccountList.ItemTapped += OtherAccountList_ItemTapped;
        }

        private async Task AskActionPopUp(CookiesSave otherAccount)
        {
            var database = Core.App.Database;
            var user = await database.GetActiveUser();
            if (user != null)
            {
                _changeOrDeleteAccountPopup = new SwitchOrDeleteAccountPopup(otherAccount);
                await PopupNavigation.Instance.PushAsync(_changeOrDeleteAccountPopup);
                var ret = await _changeOrDeleteAccountPopup.PopupClosedTask;
                switch (ret)
                {
                    case SwitchOrDeleteAccountPopup.EnumOutputType.Cancel:
                        break;
                    case SwitchOrDeleteAccountPopup.EnumOutputType.Switch:
                        await this.ViewModel.ChangeAccount(otherAccount);
                        break;
                    case SwitchOrDeleteAccountPopup.EnumOutputType.Delete:
                        bool answer = await DisplayAlert(ApplicationResource.Warning, ApplicationResource.ForgetAccount, ApplicationResource.Yes, ApplicationResource.No);
                        if (answer)
                        {
                            await this.ViewModel.ExecuteRemoveItemCommand(otherAccount);
                        }
                        break;
                }
            }
        }

        private async void OtherAccountList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            CookiesSave otherAccount = e.Item as CookiesSave;
            await AskActionPopUp(otherAccount);
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
