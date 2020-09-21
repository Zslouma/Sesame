using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;
using ZXing;

namespace Syracuse.UI.Views
{
    [MvxContentPagePresentation()]
     public partial class LoginView : ContentPage
     {
        public LoginView()
        {
            InitializeComponent();

            //this.ScannButton.Pressed += async (sender, e) =>
            //{
            //    var scanner = new ZxingScannerView();
            //    scanner.OnScanResult += Handle_OnScanResult;
            //    await this.Navigation.PushAsync(scanner);
            //};
            UserNameInput.Focused += Handle_Focus;
            UserNameInput.Unfocused += Handle_Unfocused;

            PasswordInput.Focused += Handle_Focus;
            PasswordInput.Unfocused += Handle_Unfocused;
            // CHECK VIEW
            // https://devblogs.microsoft.com/xamarin/validation-xamarin-forms-enterprise-apps/
        }

        public void Handle_Focus(object sender, FocusEventArgs args)
        {
            this.FormLayout.VerticalOptions = LayoutOptions.StartAndExpand;
        }

        public void Handle_Unfocused(object sender, FocusEventArgs args)
        {
            this.FormLayout.VerticalOptions = LayoutOptions.Center;
        }

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Scanned result", result.Text, "OK");
                this.UserNameInput.Text = result.Text;
                await Navigation.PopAsync();
            });
            //Nothing to do here.
        }

    }
}
