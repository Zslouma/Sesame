using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel<string, string>
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IRequestService requestService;
        private readonly DepartmentService departmentService = new DepartmentService();
        private bool _isnetworkError = false;
        private bool _isnetworkErrorAppend = false; 
        private string param;
        private bool viewCreate = false;

        public MasterDetailViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public override void Prepare(string parameter)
        {
            param = parameter;
            base.Prepare();
        }

        public override void Start()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            this.Connectivity_test().Wait();
            if (App.AppState.NetworkConnection)
            {
                this.JsonSynchronisation();
            }
            base.Start();
        }
         public async Task JsonSynchronisation()
        {
            CookiesSave user = await App.Database.GetActiveUser();
            if (user != null)
            {
                Library[] Alllibraries = await this.departmentService.GetLibraries(true);
                try
                {
                    Library library = Array.Find(Alllibraries, element => element.Name == user.Library && element.Code == user.LibraryCode);
                    user.LibraryUrl = library.Config.BaseUri;
                    user.DomainUrl = library.Config.DomainUri;
                    user.EventsScenarioCode = library.Config.EventsScenarioCode;
                    user.SearchScenarioCode = library.Config.SearchScenarioCode;
                    user.IsEvent = library.Config.IsEvent;
                    user.IsKm = library.Config.IsKm;
                    user.CanDownload = library.Config.CanDownload;
                    user.DailyPressQuery = library.Config.DailyPress.Query;
                    user.DailyPressScenarioCode = library.Config.DailyPress.PressScenarioCode;
                    user.InternationalPressQuery = library.Config.InternationalPress.Query;
                    user.InternationalPressScenarioCode = library.Config.InternationalPress.PressScenarioCode;
                    user.BuildingInfos = JsonConvert.SerializeObject(library.Config.BuildingInformations);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                await App.Database.SaveItemAsync(user);
            }
           
        }
        public override void ViewDestroy(bool viewFinishing = true)
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            base.ViewDestroy(viewFinishing);
        }
        public override async void ViewAppearing()
        {
            base.ViewAppearing();
            await this.Connectivity_test();
            CookiesSave user = await App.Database.GetActiveUser();
            Cookie[] cookies = JsonConvert.DeserializeObject<Cookie[]>(user.Cookies);
            bool found = false;
            DateTime now = DateTime.Now;
            foreach (var cookie in cookies)
            {
                if (cookie.Expires > now)
                {
                    found = true;
                }
                else
                {
                    cookie.Expired = true;
                }
            }
            user.Cookies = JsonConvert.SerializeObject(cookies);
            if (!found)
            {
                user.Active = false;
                await App.Database.SaveItemAsync(user);
                await this.navigationService.Navigate<SelectLibraryViewModel>();
                return;
            }
            else
            {
                await App.Database.SaveItemAsync(user);
                var a = JsonConvert.DeserializeObject<Cookie[]>(user.Cookies);
                this.requestService.LoadCookies(a);
            }
            await this.navigationService.Navigate<MenuViewModel>();
            if (param != null)
            {
                var options = new SearchOptionsDetails()
                {
                    QueryString = param
                };
                SearchOptions opt = new SearchOptions() { Query = options };
                await this.navigationService.Navigate<SearchViewModel, SearchOptions, SearchOptions>(opt);
            }
            else
            {
                if (this._isnetworkError)
                {
                    await this.navigationService.Navigate<DownloadViewModel>();
                    this._isnetworkErrorAppend = true;
                }
                else
                {
                    if (!this.viewCreate || this._isnetworkErrorAppend)
                    {
                        await this.navigationService.Navigate<HomeViewModel>();
                        this._isnetworkErrorAppend = false;
                    }
                }
                
            }
            this.viewCreate = true;

        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test().Wait();
        }

        private async Task Connectivity_test()
        {
            Console.WriteLine("Connectivity_test for MasterDetail");
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Console.WriteLine("test 1");
                Console.WriteLine("App.AppState.NetworkConnection : " + App.AppState.NetworkConnection.ToString());
                App.AppState.NetworkConnection = false;
                Console.WriteLine("App.AppState.NetworkConnection : " + App.AppState.NetworkConnection.ToString());
                Console.WriteLine("End test 1");
                if (this.viewCreate)
                {
                    //await this.navigationService.Navigate<NetworkErrorViewModel>();
                }
                this._isnetworkError = true;
            }
            else
            {
                Console.WriteLine("test 2");
                Console.WriteLine("App.AppState.NetworkConnection : " + App.AppState.NetworkConnection.ToString());
                App.AppState.NetworkConnection = true;
                Console.WriteLine("App.AppState.NetworkConnection : " + App.AppState.NetworkConnection.ToString());
                Console.WriteLine("End test 2");
                if (this._isnetworkError) {
                    this._isnetworkError = false;
                }
            }
        }
    }
}
