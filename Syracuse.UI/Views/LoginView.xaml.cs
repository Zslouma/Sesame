using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;
using ZXing;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxContentPagePresentation()]
     public partial class LoginView : ContentPage
     {
        public ZxingScannerView scanner;
        public LoginView()
        {
            InitializeComponent();

            this.ScannButton.Pressed += async (sender, e) =>
            {
                this.scanner = new ZxingScannerView();
                scanner.OnScanResult += Handle_OnScanResult;
                await this.Navigation.PushAsync(scanner);
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
            
            //Nothing to do here.
        }

    }
}
