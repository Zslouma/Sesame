using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Mes emprunts")]
    public partial class LoansView : MvxContentPage<LoansViewModel>
    {
        public LoansView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as LoansViewModel).OnDisplayAlert += LoansViewModel_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        private void LoansViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}
