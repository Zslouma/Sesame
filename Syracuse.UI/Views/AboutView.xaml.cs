using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using System;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "A Propos")]
    public partial class AboutView : MvxContentPage<AboutViewModel>
    {

        public AboutView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            this.ViewModel.VersionLabel = String.Format(AppResource.AboutVersion, Xamarin.Essentials.AppInfo.VersionString);
            this.ViewModel.CopyrightLabel = String.Format(AppResource.AboutCopyright, DateTime.Now.Year.ToString());
            this.ViewModel.RaiseAllPropertiesChanged();
            base.OnAppearing();

        }
    }
}
