using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel<string, string>
    {
        private readonly IMvxNavigationService navigationService;
        private readonly IRequestService requestService;
        private readonly DepartmentService departmentService = new DepartmentService();
        private bool __isnetworkError = false;
        private bool _isnetworkError {
            get { return __isnetworkError; }
            set {
                this.__isnetworkError = value;
                if (this.viewCreate && value)
                {
                    this.navigationService.Navigate<DownloadViewModel>();
                }
            }
        }

        private bool _isnetworkErrorAppend = false;
        private string param;
        private bool viewCreate = false;

        private CookiesSave user;
        public CookiesSave User {
            get => this.user;
            set
            {
                SetProperty(ref this.user, value);
            }
        }

        public MasterDetailViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public async override void Prepare(string parameter)
        {
            param = parameter;
            this.Connectivity_test();
            base.Prepare();
        }

        public async Task JsonSynchronisation()
        {
            this.User = await App.Database.GetActiveUser();
            if (User != null)
            {
                Library Alllibraries = await this.departmentService.GetLibraries(User.LibraryJsonUrl,true);
                if (!(Alllibraries is null) )
                {
                    try
                    {
                        Library library = Alllibraries;
                        this.User.Department = library.DepartmentCode;
                        this.User.Library = library.Name;
                        this.User.LibraryCode = library.Code;
                        this.User.LibraryUrl = library.Config.BaseUri;
                        this.User.DomainUrl = library.Config.DomainUri;
                        this.User.ForgetMdpUrl = library.Config.ForgetMdpUri;
                        this.User.EventsScenarioCode = library.Config.EventsScenarioCode;
                        this.User.SearchScenarioCode = library.Config.SearchScenarioCode;
                        this.User.IsEvent = library.Config.IsEvent;
                        this.User.RememberMe = library.Config.RememberMe;
                        this.User.IsKm = library.Config.IsKm;
                        this.User.CanDownload = library.Config.CanDownload;
                        this.User.BuildingInfos = JsonConvert.SerializeObject(library.Config.BuildingInformations);
                        List<StandartViewList> standartViewList = new List<StandartViewList>();
                        foreach (var item in library.Config.StandardsViews)
                        {
                            var tempo = new StandartViewList();

                            tempo.ViewName = item.ViewName;
                            tempo.ViewIcone = item.ViewIcone;
                            tempo.ViewQuery = item.ViewQuery;
                            tempo.ViewScenarioCode = item.ViewScenarioCode;
                            tempo.Username = this.User.Username;
                            tempo.Library = this.User.Library;
                            standartViewList.Add(tempo);
                        }

                        await App.Database.UpdateItemsAsync(standartViewList, this.User);
                    }
                    catch (Exception e)
                    {
                        this.DisplayAlert(ApplicationResource.Error, e.ToString(), ApplicationResource.ButtonValidation);
                    }
                }
                await App.Database.SaveItemAsync(this.User);
            }
           
        }
        public override void ViewDestroy(bool viewFinishing = true)
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            base.ViewDestroy(viewFinishing);
        }
        public override async void ViewAppearing()
        {
            this.Connectivity_test();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            if (App.AppState.NetworkConnection)
            {
                await this.JsonSynchronisation();
            }
            Cookie[] cookies = JsonConvert.DeserializeObject<Cookie[]>(this.User.Cookies);
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
            this.User.Cookies = JsonConvert.SerializeObject(cookies);
            if (!found)
            {
                this.User.Active = false;
                await App.Database.SaveItemAsync(this.User);
                await this.navigationService.Navigate<SelectLibraryViewModel>();
                return;
            }
            else
            {
                await App.Database.SaveItemAsync(this.User);
                var a = JsonConvert.DeserializeObject<Cookie[]>(this.User.Cookies);
                this.requestService.LoadCookies(a);
            }
            Log.Warning("Mobidoc", "Mobidoc Navigate Menu Start");
            await this.navigationService.Navigate<MenuViewModel>();
            Log.Warning("Mobidoc", "Mobidoc Navigate Menu End");
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
            base.ViewAppearing();
            this.viewCreate = true;

        }
        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Connectivity_test();
        }

        private void Connectivity_test()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                App.AppState.NetworkConnection = false;
                this._isnetworkError = true;                
            }
            else
            {
                App.AppState.NetworkConnection = true;
                if (this._isnetworkError){
                    this._isnetworkError = false;
                }
            }
        }

    }
}
