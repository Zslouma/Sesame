using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Download")]
    public partial class DownloadView : MvxContentPage<DownloadViewModel>
    {
        public DownloadView()
        {
            InitializeComponent();
        }
    }
}