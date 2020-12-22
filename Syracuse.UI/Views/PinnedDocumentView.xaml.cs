using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;



namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Mes documents epinglés")]
    public partial class PinnedDocumentView : MvxContentPage<PinnedDocumentViewModel>
    {
        public PinnedDocumentView()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as PinnedDocumentViewModel).OnDisplayAlert += PinnedDocumentView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        private void PinnedDocumentView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}