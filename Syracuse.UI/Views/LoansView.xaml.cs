using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms.BehaviorsPack;

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
            (this.DataContext as LoansViewModel).OnDisplayAlertMult += LoansViewModel_OnDisplayAlertMult;
            base.OnBindingContextChanged();
        }

        private void LoansViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
        private Task<bool> LoansViewModel_OnDisplayAlertMult(string title, string message, string buttonYes, string buttonNo) {     
            var res = this.DisplayAlert(title, message, buttonYes, buttonNo);
            return res;
        }



    }
}
