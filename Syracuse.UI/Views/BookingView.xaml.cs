using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Mes réservations")]
    public partial class BookingView : MvxContentPage<BookingViewModel>
    {
        public BookingView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as BookingViewModel).OnDisplayAlert += BookingView_OnDisplayAlert;
            (this.DataContext as BookingViewModel).OnDisplayAlertMult += BookingView_OnDisplayAlertMult;
            base.OnBindingContextChanged();
        }

        private void BookingView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
        private Task<bool> BookingView_OnDisplayAlertMult(string title, string message, string buttonYes, string buttonNo)
        {
            var res = this.DisplayAlert(title, message, buttonYes, buttonNo);
            return res;
        }
    }
}
