using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxContentPagePresentation()]
     public partial class LoginView : MvxContentPage<LoginViewModel>
    {
        public ZxingScannerView scanner;
        public LoginView()
        {
            InitializeComponent();

            this.ScannButton.Pressed += async (sender, e) =>
            {
                if (await Permissions.CheckStatusAsync<Permissions.Camera>() != PermissionStatus.Granted)
                {
                    var tempo = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    await Permissions.RequestAsync<Permissions.Camera>();
                }
                if (await Permissions.CheckStatusAsync<Permissions.Camera>() == PermissionStatus.Granted)
                {
                    this.scanner = new ZxingScannerView();
                    scanner.OnScanResult += Handle_OnScanResult;
                    await this.Navigation.PushAsync(scanner);
                }
            };
            UserNameInput.Focused += Handle_Focus;
            UserNameInput.Unfocused += Handle_Unfocused;

            PasswordInput.Focused += Handle_Focus;
            PasswordInput.Unfocused += Handle_Unfocused;
            ScannButton.FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)) * 2;
            // CHECK VIEW
            // https://devblogs.microsoft.com/xamarin/validation-xamarin-forms-enterprise-apps/
        }

        public void Handle_Focus(object sender, FocusEventArgs args)
        {
            this.FormLayout.VerticalOptions = LayoutOptions.StartAndExpand;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as LoginViewModel).OnDisplayAlert += LoginView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private void LoginView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

        public void Handle_Unfocused(object sender, FocusEventArgs args)
        {
            this.FormLayout.VerticalOptions = LayoutOptions.Center;
        }

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.Navigation.PopAsync();
                this.UserNameInput.Unfocus();
                this.UserNameInput.Focus();
                this.UserNameInput.Text = result.Text;
            });
           
        }

        private async void OpenBrowser_OnClicked(object sender, EventArgs e)
        {
            string url = this.ViewModel.department.ForgetMdpUrl;
            if (url == null)
            {
                url = this.ViewModel.department.LibraryUrl + "resetpassword.aspx";
            }
            Uri uri;
            try
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
                else
                {
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                }
            }
            catch (Exception)
            {
                await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.ErrorOccurred), ApplicationResource.ButtonValidation);
            }



        }

    }
}
