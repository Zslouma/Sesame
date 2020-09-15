using Foundation;
using MvvmCross.Forms.Platforms.Ios.Core;
using Syracuse.UI;
using UIKit;

namespace Syracuse.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate<MvxFormsIosSetup<Mobitheque.Core.App, UI.App>, Mobitheque.Core.App, UI.App>
    {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags(new string[] { "IndicatorView_Experimental", "SwipeView_Experimental" });
            UINavigationBar.Appearance.ShadowImage = new UIImage();
            Xamarin.Forms.Forms.Init();
            XF.Material.iOS.Material.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            this.LoadApplication(new App());
            return base.FinishedLaunching(app, options);
            /*Xamarin.Forms.Forms.Init();
            XF.Material.iOS.Material.Init();

            LoadApplication(new App());
            base.FinishedLaunching(application);*/
        }

    }
}
