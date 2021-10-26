using Foundation;
using Mobitheque.IOS.CustomRenderer;
using Syracuse.Mobitheque.UI.CustomRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Mobitheque.IOS.CustomRenderer
{
    public class HybridWebViewRenderer : WkWebViewRenderer
    {
        WKWebView webView;
        public HybridWebView HybridWebView
        {
            get { return Element as HybridWebView; }
        }
        public HybridWebViewRenderer()
        {
        }
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                NavigationDelegate = new CustomNavigationDelegate(HybridWebView);
            }
        }



    }
    public class CustomNavigationDelegate : WKNavigationDelegate
    {
        private readonly HybridWebView _hybridWebView;
        internal CustomNavigationDelegate(HybridWebView cookieWebView)
        {
            _hybridWebView = cookieWebView;
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookies((cookies) =>
            {
                var cookiesCollection = new CookieCollection();
                foreach (var cookie in cookies)
                {
                    cookiesCollection.Add(new Cookie
                    {
                        Name = cookie.Name,
                        Value = cookie.Value,
                        Domain = cookie.Domain,
                    });
                }
                _hybridWebView.OnNavigated(new CookieNavigatedEventArgs
                {
                    Cookies = cookiesCollection,
                    Url = webView.Url.ToString()
                }) ;
            });
        }
    }
}