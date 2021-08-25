using Syracuse.Mobitheque.UI.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI
{
    public partial class App : Application
    {
        Page page;
        Page networkErrorPage = new NetworkErrorView();
        bool isnetworkError = false;
        public App()
        {
            InitializeComponent();
            this.MainPage = new NavigationPage();
            XF.Material.Forms.Material.Init(this);

        }
        protected override void OnStart()
        {
            //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            //Connectivity_test().Wait();
        }

        protected override void OnSleep()
        {
            //Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        protected override void OnResume()
        {
            //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }
       async Task Connectivity_test()
        {
            Console.WriteLine("Connectivity_test for App.xaml");
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                if (MainPage is NavigationPage && ((NavigationPage)MainPage).CurrentPage != networkErrorPage)
                {

                    this.page = ((NavigationPage)MainPage).CurrentPage;
                    await ((NavigationPage)MainPage).PushAsync(this.networkErrorPage);
                    this.isnetworkError = true;
                }               
            }
            else {
                if (this.isnetworkError && MainPage is NavigationPage) {
                        try
                        {
                            await ((NavigationPage)MainPage).PopAsync();
                        }
                        catch
                        {
                            await ((NavigationPage)MainPage).PushAsync(new SelectLibraryView());
                        }
                    this.isnetworkError = false;
                }
            }
        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test().Wait();  
        }
    }
}
