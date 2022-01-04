using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
    public partial class StandardView : MvxContentPage<StandardViewModel>
    {
        public StandardView()
        {
            InitializeComponent();
        }
    }
}