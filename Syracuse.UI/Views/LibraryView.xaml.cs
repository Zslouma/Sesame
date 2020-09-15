using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Mes Bibliothéques")]
    public partial class LibraryView : MvxContentPage<LibraryViewModel>
    {
        public LibraryView()
        {
            InitializeComponent();
        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            if (carouselView.Position < (this.ViewModel.ItemsSource.Count - 1))
            {
                carouselView.Position += 1;
            }
            else if (carouselView.Position == this.ViewModel.ItemsSource.Count - 1)
            {
                carouselView.Position = 0;
            }
        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            if (carouselView.Position > 0)
            {
                carouselView.Position -= 1;
            }
            else if (carouselView.Position == 0)
            {
                carouselView.Position = this.ViewModel.ItemsSource.Count - 1;
            }
        }

        private void WebViewNavigating(object sender, WebNavigatingEventArgs e)
        {
            this.ViewModel.AbsoluteIndicatorVisibility = true;
            this.ViewModel.RaiseAllPropertiesChanged();
        }

        private void WebViewNavigated(object sender, WebNavigatedEventArgs e)
        {
            var item = sender as WebView;
            LibraryInformations bindingContext = item.Source.BindingContext as LibraryInformations;
            if (e.Result != WebNavigationResult.Success)
            {
                this.ViewModel.WebViewError(bindingContext, true);
            }
            else
            {
                this.ViewModel.WebViewError(bindingContext, false);
            }
            this.ViewModel.AbsoluteIndicatorVisibility = false;
            this.ViewModel.RaiseAllPropertiesChanged();
        }
    }
}