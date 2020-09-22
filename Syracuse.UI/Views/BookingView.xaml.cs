using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

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
            base.OnBindingContextChanged();
        }

        private void BookingView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}
