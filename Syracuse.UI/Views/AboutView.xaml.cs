using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "A Propos")]
    public partial class AboutView : MvxContentPage<AboutViewModel>
    {
        public AboutView()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            (this.DataContext as AboutViewModel).OnDisplayAlert += AboutView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        private void AboutView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}
