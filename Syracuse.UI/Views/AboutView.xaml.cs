using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "A Propos")]
    public partial class AboutView : MvxContentPage<AboutViewModel>
    {
        public AboutView()
        {
            InitializeComponent();
        }
    }
}
