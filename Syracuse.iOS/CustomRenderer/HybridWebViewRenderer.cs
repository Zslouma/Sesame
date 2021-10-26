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
        public HybridWebViewRenderer()
        {
        }
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        public class CustomNavigationDelegate : WKNavigationDelegate
        {
            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                webView.Configuration.WebsiteDataStore.HttpCookieStore.GetAllCookies((cookies) =>
                {

                });
            }
        }

    }
    internal class MyWeakNavigationDelegate : WKNavigationDelegate, INSUrlConnectionDataDelegate
    {
        private HybridWebViewRenderer customWebViewRenderer;

        public MyWeakNavigationDelegate(HybridWebViewRenderer customWebViewRenderer)
        {
            this.customWebViewRenderer = customWebViewRenderer;
        }

    }
}