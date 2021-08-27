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
            LibraryPicker.PropertyChanged += PickerPropertyChanged;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as SelectLibraryViewModel).OnDisplayAlert += SelectLibrary_OnDisplayAlert;
            base.OnBindingContextChanged();
        }


        protected override void OnAppearing()
        {
            this.submitButton.IsEnabled = true;
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Connectivity_test();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        private void SelectLibrary_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

        private void PickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                LibraryPicker.TitleColor = LibraryPicker.IsEnabled ? (Color)Application.Current.Resources["PurpleTextColor"] : (Color) Application.Current.Resources["PurpleTextColorTransparente"];
            }
        }

        public async Task Connectivity_test()
        {
            Console.WriteLine("Connectivity_test for App.xaml");
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                this.SelectLibrary_OnDisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
                this.isnetworkError = true;
            }
            else
            {
                if (this.isnetworkError && MainPage is NavigationPage)
                {
                    this.ViewModel.ManageDepartments();
                    this.isnetworkError = false;
                }
            }
        }
        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test().Wait();
        }

    }
}