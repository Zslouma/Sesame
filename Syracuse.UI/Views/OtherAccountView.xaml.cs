using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "")]
    public partial class OtherAccountView : MvxContentPage<OtherAccountViewModel>
    {
        public OtherAccountView()
        {
            InitializeComponent();
            this.otherAccountList.ItemTapped += OtherAccountList_ItemTapped;
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
    }
}
