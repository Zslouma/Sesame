using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
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
        }

        private void SelectLibrary_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);

        private void PickerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEnabled")
            {
                LibraryPicker.TitleColor = LibraryPicker.IsEnabled ? (Color)Application.Current.Resources["PurpleTextColor"] : (Color) Application.Current.Resources["PurpleTextColorTransparente"];
            }
        }

    }
}