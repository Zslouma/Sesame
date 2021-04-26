using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core.Services.Database;
using System;
using System.ComponentModel;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TutorialPopupAddAccount : Rg.Plugins.Popup.Pages.PopupPage
    {

        private bool isConnect { get; set; } = false;

        private CookiesDatabase Database;

        private Syracuse.Mobitheque.Core.Models.CookiesSave User;

        public TutorialPopupAddAccount(CookiesDatabase database = null, Syracuse.Mobitheque.Core.Models.CookiesSave user = null)
        {
            InitializeComponent();
            if (database != null && user != null)
            {
                this.Database = database;
                this.User = user;
                this.DontShow.IsVisible = true;
            }
            else
            {
                this.DontShow.IsVisible = false;
            }

        }
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
        async void OnNotShowAgain(object sender, EventArgs args)
        {
            this.User.IsTutorialAddAcount = false;
            this.Database.SaveItemAsync(this.User).Wait();
            await PopupNavigation.Instance.PopAllAsync();
        }

        async void OnClose(object sender, EventArgs args)
        {
            this.CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}