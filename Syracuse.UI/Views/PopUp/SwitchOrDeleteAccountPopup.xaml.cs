using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Database;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwitchOrDeleteAccountPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public enum EnumOutputType { Switch, Delete, Cancel }
        public EnumOutputType ReturnValue;

        private TaskCompletionSource<EnumOutputType> _taskCompletionSource;
        public Task<EnumOutputType> PopupClosedTask => _taskCompletionSource.Task;

        private bool isConnect { get; set; } = false;

        private CookiesDatabase Database;

        private CookiesSave User;

        private CookiesSave otherAccount;
        public CookiesSave OtherAccount
        {
            get { return otherAccount; }
            set
            {
                otherAccount = value;
                OnPropertyChanged(nameof(OtherAccount));
            }
        }

        public SwitchOrDeleteAccountPopup(CookiesSave otherAccount)
        {
            InitializeComponent();
            ReturnValue = EnumOutputType.Cancel;
            OtherAccount = otherAccount;
            BindingContext = this;
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            ReturnValue = EnumOutputType.Cancel;
            CloseAllPopup();
        }
        private void OnDeleteClick(object sender, EventArgs args)
        {
            ReturnValue = EnumOutputType.Delete;
            CloseAllPopup();
        }

        private void OnSwitchAccountClick(object sender, EventArgs args)
        {
            ReturnValue = EnumOutputType.Switch;
            CloseAllPopup();
        }


        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _taskCompletionSource = new TaskCompletionSource<EnumOutputType>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _taskCompletionSource.SetResult(ReturnValue);
        }


    }
}