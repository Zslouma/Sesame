using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
     public class WebAndCookiesAuthentificationViewModel : BaseViewModel<WebAndCookiesAuthentificationParameters, WebAndCookiesAuthentificationParameters>
    {

        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private bool isBusy = true;

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        private CookieContainer cookies = new CookieContainer();

        public CookieContainer Cookies
        {
            get { return cookies; }
            set {
                if (value != null)
                {
                    SetProperty(ref this.cookies, value);
                    Console.WriteLine("CookieContainer change : " + value.Count.ToString());

                this.RaiseAllPropertiesChanged();
                }

            }
        }

        private CookiesSave _departement { get; set; }

        public CookiesSave Departement
        {
            get { return _departement; }
            set { this._departement = value; }
        }

        private UrlWebViewSource _urlWebViewSource;


        public UrlWebViewSource UrlWebViewSource
        {
            get => this._urlWebViewSource;
            set
            {
                SetProperty(ref this._urlWebViewSource, value);
                Console.WriteLine("UrlWebViewSource change : " + value);
                this.RaiseAllPropertiesChanged();
            }
        }

        private UrlWebViewSource _defaultUrl;


        public UrlWebViewSource DefaultUrl
        {
            get => this._defaultUrl;
            set
            {
                SetProperty(ref this._defaultUrl, value);
                this.RaiseAllPropertiesChanged();
                // take any additional actions here which are required when MyProperty is updated
            }
        }

        public WebAndCookiesAuthentificationViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public override void Prepare(WebAndCookiesAuthentificationParameters parameters)
        {
            try
            {
                this.IsBusy = true;
                this.DefaultUrl = new UrlWebViewSource() { Url = parameters.Url };
                this.UrlWebViewSource = new UrlWebViewSource() { Url = parameters.Url };
                this.Departement = parameters.Department;
                this.RaiseAllPropertiesChanged();
                this.IsBusy = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Prepare Exception: " + e.ToString());
                throw;
            }
           
        }
        public IEnumerable<Cookie> ChangeToArray(CookieCollection cookieContainer)
        {
            foreach (Cookie cookie in cookieContainer)
            {
                Console.WriteLine("Name = {0} ; Value = {1} ; Domain = {2}", cookie.Name, cookie.Value, cookie.Domain);
                yield return cookie;
            }
        }
        public  async Task AuthenticationAndRedirect(CookieCollection cookieCollection)
        {
            Cookie[] cookiesArray = ChangeToArray(cookieCollection).ToArray();
            foreach (var cookie in cookiesArray)
            {
                Console.WriteLine("AuthenticationAndRedirect: Name = {0} ; Value = {1} ; Domain = {2}", cookie.Name, cookie.Value, cookie.Domain);
            }
            bool test = true;
            while (test)
            {
                var d = await App.Database.GetActiveUser();
                if (d != null)
                {
                    d.Active = false;
                    await App.Database.SaveItemAsync(d);
                }
                else
                {
                    test = false;
                }
            }
            var username = "_";
            this.requestService.LoadCookies(cookiesArray);
            Console.WriteLine(JsonConvert.SerializeObject(cookiesArray));
            var tempoCookiesSave = await App.Database.GetByUsernameAsync(username);
            CookiesSave item = tempoCookiesSave != null ? tempoCookiesSave : new CookiesSave();
            item.Active = true;
            item.Username = username;
            item.Cookies = JsonConvert.SerializeObject(cookiesArray);
            item.Library = this.Departement.Library;
            item.LibraryCode = this.Departement.LibraryCode;
            item.LibraryUrl = this.Departement.LibraryUrl;
            item.DomainUrl = this.Departement.DomainUrl;
            item.ForgetMdpUrl = this.Departement.ForgetMdpUrl;
            item.Department = this.Departement.Department;
            item.SearchScenarioCode = this.Departement.SearchScenarioCode;
            item.EventsScenarioCode = this.Departement.EventsScenarioCode;
            item.IsEvent = this.Departement.IsEvent;
            item.RememberMe = this.Departement.RememberMe;
            item.IsKm = this.Departement.IsKm;
            item.BuildingInfos = this.Departement.BuildingInfos;
            await App.Database.SaveItemAsync(item);
            this.requestService.InitializeHttpClient(this.Departement.DomainUrl);
            cookiesArray = this.requestService.GetCookies().ToArray();
            foreach (var cookie in cookiesArray)
            {
                Console.WriteLine("AuthenticationAndRedirect2: Name = {0} ; Value = {1} ; Domain = {2}", cookie.Name, cookie.Value, cookie.Domain);
            }
            var data = await this.requestService.GetSummary();
            cookiesArray = this.requestService.GetCookies().ToArray();
            foreach (var cookie in cookiesArray)
            {
                Console.WriteLine("AuthenticationAndRedirect3: Name = {0} ; Value = {1} ; Domain = {2}", cookie.Name, cookie.Value, cookie.Domain);
            }
            if (data != null && data.Success)
            {
                var b = await App.Database.GetByUsernameAsync(username);
                b.Active = false;
                await App.Database.SaveItemAsync(b);
                username = data.D.AccountSummary.DisplayName;
                CookiesSave tempo = await App.Database.GetByUsernameAsync(username);
                item = tempoCookiesSave != null ? tempo : new CookiesSave();
                item.Active = true;
                item.Username = username;
                item.Cookies = JsonConvert.SerializeObject(cookiesArray);
                item.Library = this.Departement.Library;
                item.LibraryCode = this.Departement.LibraryCode;
                item.LibraryUrl = this.Departement.LibraryUrl;
                item.DomainUrl = this.Departement.DomainUrl;
                item.ForgetMdpUrl = this.Departement.ForgetMdpUrl;
                item.Department = this.Departement.Department;
                item.SearchScenarioCode = this.Departement.SearchScenarioCode;
                item.EventsScenarioCode = this.Departement.EventsScenarioCode;
                item.IsEvent = this.Departement.IsEvent;
                item.RememberMe = this.Departement.RememberMe;
                item.IsKm = this.Departement.IsKm;
                item.BuildingInfos = this.Departement.BuildingInfos;
                await App.Database.SaveItemAsync(item);
                this.requestService.LoadCookies(JsonConvert.DeserializeObject<Cookie[]>(item.Cookies));
            }
            await this.navigationService.Navigate<MasterDetailViewModel>();

        }
    }
}
