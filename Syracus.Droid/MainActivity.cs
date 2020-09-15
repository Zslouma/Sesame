using Android.App;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Platforms.Android.Core;
using Android.Content.PM;
using Android.OS;

namespace Syracuse.Droid
{
    [Activity(Label = "Syracus.Droid",
              Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
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
        // ... And this
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
