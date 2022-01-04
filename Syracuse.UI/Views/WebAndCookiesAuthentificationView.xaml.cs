using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Syracuse.Mobitheque.UI.CustomRenderer;
using System;
using System.ComponentModel;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebAndCookiesAuthentificationView : MvxContentPage<WebAndCookiesAuthentificationViewModel>
    {
        bool CanRefresh { get; set; } = true;
        public WebAndCookiesAuthentificationView()
        {
            InitializeComponent();
        }

        private async void WebViewNavigated(object sender, CookieNavigatedEventArgs args)
        {
            Console.WriteLine("WebChanged Cookies :" + args.Cookies.Count.ToString());
            Console.WriteLine("WebChanged Source :" + args.Url);
            if (args.Cookies.Count > 0 && args.Url.Contains(this.ViewModel.Departement.DomainUrl))
            {
                Console.WriteLine("WebChanged Scucess");
                await this.ViewModel.AuthenticationAndRedirect(args.Cookies);
            }
        }
    }
}