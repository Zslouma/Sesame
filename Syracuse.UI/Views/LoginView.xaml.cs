using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxContentPagePresentation()]
     public partial class LoginView : MvxContentPage<LoginViewModel>
    {
        Page page;
        Page networkErrorPage = new NetworkErrorView();
        bool isnetworkError = false;
        public NavigationPage MainPage = new NavigationPage();

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
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListSSO.Children.Clear();
            foreach (var item in this.ViewModel.ListSSO)
            {
                Button button = new Button();
                button.Text = item.Label;
                button.CornerRadius = 15;
                button.BackgroundColor = Color.FromHex("#FFFFFF");
                button.TextColor = Color.FromHex("#6574CF");
                button.CommandParameter = item.Value;
                button.Clicked += OpenBrowserProvider_OnClicked;
                ListSSO.Children.Add(button);
            }
            ListSSO.VerticalOptions = LayoutOptions.FillAndExpand;
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


        protected override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Connectivity_test();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
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

        private async void OpenBrowserProvider_OnClicked(object sender, EventArgs e)
        {
            try
            {
                string provider = ((Button)sender).CommandParameter as string;
                string url = this.ViewModel.department.LibraryUrl + "/?forcelogon=" + provider;
                Uri uri;
                this.ViewModel.OpenWebBrowser(url);
            }
            catch (Exception)
            {
                await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.ErrorOccurred), ApplicationResource.ButtonValidation);
            }

        }
        public async Task Connectivity_test()
        {
            Console.WriteLine("Connectivity_test for App.xaml");
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                this.LoginView_OnDisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
                this.isnetworkError = true;
            }
            else
            {
                if (this.isnetworkError && MainPage is NavigationPage)
                {
                    this.isnetworkError = false;
                }
            }
        }
        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test().Wait();
        }

    }
}
