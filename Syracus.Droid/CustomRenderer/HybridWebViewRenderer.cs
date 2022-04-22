using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Mobitheque.Droid.CustomRenderer;
using Syracuse.Mobitheque.UI.CustomRenderer;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Mobitheque.Droid.CustomRenderer
{
    [Obsolete]
    public class HybridWebViewRenderer : WebViewRenderer
    {

        public HybridWebView HybridWebView
        {
            get { return Element as HybridWebView; }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.SetWebViewClient(new HybridWebViewClient(HybridWebView));
            }
        }

       
    }

    internal class HybridWebViewClient : WebViewClient
    {
        private readonly HybridWebView _hybridWebView;
        internal HybridWebViewClient(HybridWebView cookieWebView)
        {
            _hybridWebView = cookieWebView;
        }

        public override void OnPageStarted(global::Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
            CookieManager.Instance.AcceptCookie();
            CookieManager.Instance.AcceptThirdPartyCookies(view);
            _hybridWebView.OnNavigating(new CookieNavigationEventArgs
            {
                Url = url
            });
        }

        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
            var cookieHeader = CookieManager.Instance.GetCookie(url);
            if (cookieHeader != null)
            {
                Uri uri = new Uri(url);
                var domain = uri.Authority;
                var cookies = new CookieCollection();
                var cookiePairs = cookieHeader.Split(';');
                foreach (var cookiePair in cookiePairs)
                {
                    var cookiePieces = cookiePair.Split('=',2);
                    cookies.Add(new Cookie
                    {
                        Name = cookiePieces[0].Trim(),
                        Value = cookiePieces[1].Trim(),
                        Domain = domain.Trim(),
                    });
                }

                _hybridWebView.OnNavigated(new CookieNavigatedEventArgs
                {
                    Cookies = cookies,
                    Url = url
                });

            }
        }

    }

}