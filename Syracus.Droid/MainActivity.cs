using Android.App;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Platforms.Android.Core;
using Android.Content.PM;
using Android.OS;

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
            Xamarin.Forms.Forms.SetFlags(new string[] { "IndicatorView_Experimental", "SwipeView_Experimental" });
            base.OnCreate(bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            XF.Material.Droid.Material.Init(this, bundle);
            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#6574CF"));
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);
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
