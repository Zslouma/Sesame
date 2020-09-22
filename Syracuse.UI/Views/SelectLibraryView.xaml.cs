using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(NoHistory = true)]
    public partial class SelectLibraryView : MvxContentPage<SelectLibraryViewModel>
    {

        public SelectLibraryView()
        {
            InitializeComponent();
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
        }

        private void SelectLibrary_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

    }
}