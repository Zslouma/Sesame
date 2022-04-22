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

        public MasterDetailViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public async override void Prepare(string parameter)
        {
            param = parameter;
            this.Connectivity_test();
            if (App.AppState.NetworkConnection)
            {
                await this.JsonSynchronisation();
            }
            base.Prepare();
        }

         public async Task JsonSynchronisation()
        {
            Console.WriteLine("JsonSynchronisation");
            CookiesSave user = await App.Database.GetActiveUser();
            if (user != null)
            {
                Library Alllibraries = await this.departmentService.GetLibraries(user.LibraryJsonUrl,true);
                if (!(Alllibraries is null) )
                {
                    try
                    {
                        Library library = Alllibraries;
                        user.Department = library.DepartmentCode;
                        user.Library = library.Name;
                        user.LibraryCode = library.Code;
                        user.LibraryUrl = library.Config.BaseUri;
                        user.DomainUrl = library.Config.DomainUri;
                        user.ForgetMdpUrl = library.Config.ForgetMdpUri;
                        user.EventsScenarioCode = library.Config.EventsScenarioCode;
                        user.SearchScenarioCode = library.Config.SearchScenarioCode;
                        user.IsEvent = library.Config.IsEvent;
                        user.RememberMe = library.Config.RememberMe;
                        user.IsKm = library.Config.IsKm;
                        user.CanDownload = library.Config.CanDownload;
                        user.DailyPressName = library.Config.DailyPress.PressName;
                        user.DailyPressQuery = library.Config.DailyPress.Query;
                        user.DailyPressScenarioCode = library.Config.DailyPress.PressScenarioCode;
                        user.InternationalPressName = library.Config.InternationalPress.PressName;
                        user.InternationalPressQuery = library.Config.InternationalPress.Query;
                        user.InternationalPressScenarioCode = library.Config.InternationalPress.PressScenarioCode;
                        user.BuildingInfos = JsonConvert.SerializeObject(library.Config.BuildingInformations);
                        List<StandartViewList> standartViewList = new List<StandartViewList>();
                        foreach (var item in library.Config.StandardsViews)
                        {
                            var tempo = new StandartViewList();

                            tempo.ViewName = item.ViewName;
                            tempo.ViewIcone = item.ViewIcone;
                            Console.WriteLine("ViewIcone :" + item.ViewIcone);
                            tempo.ViewQuery = item.ViewQuery;
                            tempo.ViewScenarioCode = item.ViewScenarioCode;
                            tempo.Username = user.Username;
                            tempo.Library = user.Library;
                            standartViewList.Add(tempo);
                        }

                        await App.Database.UpdateItemsAsync(standartViewList, user);
                    }
                    catch (Exception e)
                    {
                        this.DisplayAlert(ApplicationResource.Error, e.ToString(), ApplicationResource.ButtonValidation);
                    }
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
            this.Connectivity_test();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
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
            Console.WriteLine("Connectivity_ConnectivityChanged Start");
            Connectivity_test();
            Console.WriteLine("Connectivity_ConnectivityChanged End");
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
