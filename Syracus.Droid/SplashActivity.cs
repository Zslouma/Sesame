using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

namespace Syracuse.Mobitheque.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, Icon = "@drawable/logo", NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed()
        {
            // Method intentionally left empty.
        }
    }
}