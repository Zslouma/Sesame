using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
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
        }

        public override void Start()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            this.Connectivity_test().Wait();
            base.Start();
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
                    await this.navigationService.Navigate<NetworkErrorViewModel>();
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                App.AppState.NetworkConnection = false;
                if (this.viewCreate)
                {
                    await this.navigationService.Navigate<NetworkErrorViewModel>();
                }
                this._isnetworkError = true;
            }
            else
            {
                App.AppState.NetworkConnection = true;
                if (this._isnetworkError) {
                    this._isnetworkError = false;
                }
            }
        }
    }
}
