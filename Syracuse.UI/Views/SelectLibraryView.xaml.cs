using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(NoHistory = true)]
    public partial class SelectLibraryView : MvxContentPage<SelectLibraryViewModel>
    {
        Page page;
        Page networkErrorPage = new NetworkErrorView();
        bool isnetworkError = false;
        public NavigationPage MainPage = new NavigationPage();
        public SelectLibraryView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Connectivity_test();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as SelectLibraryViewModel).OnDisplayAlert += SelectLibrary_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        public async void InvokeCompleted(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text) && !this.isnetworkError)
            {
                this.ViewModel.CanSubmit = true;
            }
            else
            {
                this.ViewModel.CanSubmit = false;
            }
        }
        private async void btnScan_Clicked(object sender, EventArgs e)
        {
            this.ViewModel.IsLoading = true;
            this.ViewModel.ValidateHandler("https://syracusepp.archimed.fr/mobidoc/etablissements/CNP-SESAME.json?_s=");

            this.ViewModel.IsLoading = false;
        }

        public async Task Connectivity_test()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                this.SelectLibrary_OnDisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
                this.isnetworkError = true;
            }
            else
            {
                if (this.isnetworkError && MainPage is NavigationPage)
                {
                    this.isnetworkError = false;
                }
            }
            if (!string.IsNullOrEmpty(txtBarcode.Text) && !this.isnetworkError)
            {
                this.ViewModel.CanSubmit = true;
            }
            else
            {
                this.ViewModel.CanSubmit = false;
            }
        }
        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test().Wait();
        }

        private void SelectLibrary_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}