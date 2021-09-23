using Android.Content;
using Mobitheque.Droid.CustomRenderer;
using Syracuse.Mobitheque.UI.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Mobitheque.Droid.CustomRenderer
{
    public class HybridWebViewRenderer : WebViewRenderer
    {
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
        }


    }
}