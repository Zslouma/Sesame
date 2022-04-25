using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms.Xaml;
using Syracuse.Mobitheque.Core.Models;
using System;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Syracuse.Mobitheque.Core;


namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchDetailsView : MvxContentPage<SearchDetailsViewModel>
    {

        public SearchDetailsView()
        {
            InitializeComponent();
        }
        private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            var model = e.CurrentItem as Result;
        }

        private void carouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            var CurrentPosition = e.CurrentPosition;
            this.ViewModel.RaiseAllPropertiesChanged();
            if (e.CurrentPosition > e.PreviousPosition)
            {
                if (e.CurrentPosition+1 >= this.ViewModel.EndDataPosition && e.CurrentPosition + 1 < this.ViewModel.ItemsSource.Count )
                {
                    this.ViewModel.EndDataPosition = this.ViewModel.EndDataPosition + 10 < this.ViewModel.ItemsSource.Count ? this.ViewModel.EndDataPosition + 10 : this.ViewModel.ItemsSource.Count - 1;
                    this.ViewModel.FormateToCarrousel(e.CurrentPosition, this.ViewModel.EndDataPosition);
                }
            }
            else
            {
                if (e.CurrentPosition <=  this.ViewModel.StartDataPosition && e.CurrentPosition > 0 )
                {
                    this.ViewModel.StartDataPosition = this.ViewModel.StartDataPosition - 10 >= 0 ? this.ViewModel.StartDataPosition - 10 : 0;
                    this.ViewModel.FormateToCarrousel( this.ViewModel.StartDataPosition, e.CurrentPosition);
                }
                if (e.CurrentPosition < e.PreviousPosition-1 )
                {
                    this.carouselView.Position = e.PreviousPosition;
                    CurrentPosition = e.PreviousPosition;
                }
            }
            this.ViewModel.Position = CurrentPosition;
            this.ViewModel.IsPositionVisible = true;
        }
        private async void OnCarouselViewRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (int.Parse(this.ViewModel.NbrResults) > this.ViewModel.ItemsSource.Count)
            {
            
                if (!this.ViewModel.InLoadMore)
                {
                    await this.ViewModel.LoadMore();
                }
            }
            else
            {
                carouselView.RemainingItemsThresholdReached -= OnCarouselViewRemainingItemsThresholdReached;
            }

        }
        private async void OpenBrowser_OnClicked(object sender, EventArgs e)
        {
            string url = ((Button)sender).CommandParameter as string;
            Uri uri;
            try
            {
                if (Uri.TryCreate(url, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    uri = await this.ViewModel.GetUrlTransfert(uri);
                    //await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                    this.ViewModel.OpenWebBrowser(uri.ToString());


                }
                else
                {
                    uri = await this.ViewModel.RelativeUriToAbsolute(url);
                    uri = await this.ViewModel.GetUrlTransfert(uri);
                    this.ViewModel.OpenWebBrowser(uri.ToString());
                    //await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

                }
            }
            catch (Exception)
            {
                await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.ErrorOccurred), ApplicationResource.ButtonValidation);
            }
            
        }

        private async void HoldingButton_Clicked(object sender, EventArgs e)
        {
            Holdings data = ((Button)sender).BindingContext as Holdings;
            bool answer = await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.HoldingChoice, data.Site), ApplicationResource.Book, ApplicationResource.Cancel);
            if (answer)
            {
                await this.ViewModel.Holding(data.Holdingid, data.RecordId, data.BaseName);
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
            this.ViewModel.AbsoluteIndicatorVisibility = false;
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as SearchDetailsViewModel).OnDisplayAlert += SearchDetailsView_OnDisplayAlert;
            (this.DataContext as SearchDetailsViewModel).OnDisplayAlertMult += SearchDetailsView_OnDisplayAlertMult;
            base.OnBindingContextChanged();
        }

        private void SearchDetailsView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
        private Task<bool> SearchDetailsView_OnDisplayAlertMult(string title, string message, string buttonYes, string buttonNo)
        {
            var res = this.DisplayAlert(title, message, buttonYes, buttonNo);
            return res;
        }


    }
}