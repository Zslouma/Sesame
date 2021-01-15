using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core.Services.Database;
using System;
using System.ComponentModel;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TutorialPopup : Rg.Plugins.Popup.Pages.PopupPage
    {

        private bool isConnect { get; set; } = false;

        private CookiesDatabase Database;

        private Syracuse.Mobitheque.Core.Models.CookiesSave User;

        public TutorialPopup(CookiesDatabase database = null, Syracuse.Mobitheque.Core.Models.CookiesSave user = null)
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
        async void OnRepeatClick(object sender, EventArgs args)
        {
            this.Tutorial.IsAnimationPlaying = false;
            this.Tutorial.IsAnimationPlaying = true;
        }
        async void OnNotShowAgain(object sender, EventArgs args)
        {
            this.User.IsTutorial = false;
            this.Database.SaveItemAsync(this.User).Wait();
            await PopupNavigation.Instance.PopAllAsync();
        }


        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }




    }
}