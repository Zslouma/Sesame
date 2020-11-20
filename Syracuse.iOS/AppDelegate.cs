using System.Linq;
using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using Syracuse.Mobitheque.UI;
using Syracuse.Mobitheque.UI.Views;
using UIKit;

namespace Syracuse.Mobitheque.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<MvxFormsIosSetup<Mobitheque.Core.App, UI.App>, Mobitheque.Core.App, UI.App>
    {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags(new string[] { "IndicatorView_Experimental", "SwipeView_Experimental" });
            Xamarin.Forms.Forms.Init();
            XF.Material.iOS.Material.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            this.LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

    }
}
