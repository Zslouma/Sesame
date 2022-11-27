using Android.App;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Platforms.Android.Core;
using Android.Content.PM;
using Android.OS;
using System.Net;
using Android.Util;

namespace Syracuse.Mobitheque.Droid
{
    [Activity(Label = "Mobitheque.Droid",
              Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              ScreenOrientation = ScreenOrientation.Portrait,
              LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MvxFormsAppCompatActivity<MvxFormsAndroidSetup<Mobitheque.Core.App, UI.App>, Mobitheque.Core.App, UI.App>
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
            Xamarin.Forms.Forms.SetFlags(new string[] { "IndicatorView_Experimental", "SwipeView_Experimental" });
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            XF.Material.Droid.Material.Init(this, bundle);
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#6574CF"));
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: false);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            }
            catch (System.Exception e)
            {
                Log.Error("Mobidoc", "OnCreate MainActivity Error: "+e.Message);
                throw;
            }
        }
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }
        // ... And this
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
