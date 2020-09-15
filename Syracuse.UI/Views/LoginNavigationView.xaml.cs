using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.UI.Views
{
     [MvxContentPagePresentation()]
    public partial class LoginNavigationView : MvxNavigationPage<LoginViewModel>
    {
        public LoginNavigationView()
        {
            InitializeComponent();
            this.PushAsync(new LoginView());
    }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as LoginViewModel).OnDisplayAlert += LoginView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private void LoginView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}
