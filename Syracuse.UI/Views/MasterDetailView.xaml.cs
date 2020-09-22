using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Root, WrapInNavigationPage = false, Title = "Syracuse App")]
    public partial class MasterDetailView : MvxMasterDetailPage<MasterDetailViewModel>
    {
        public MasterDetailView()
        {
            InitializeComponent();
        }
    }
}