using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel<LoginParameters, LoginParameters>
    {
        private string username;
        private string password;
        private bool isLoading;
        private IMvxAsyncCommand connectCommand;
        private readonly IRequestService requestService;
        private readonly IDepartmentService departmentService;
        private readonly IGeolocationService geolocationService;
        private readonly IMvxNavigationService navigationService;

        public string UserName
        {
            get => this.username;
            set {
                SetProperty(ref this.username, value);
                this.UserNameIsError = false;
                this.RaisePropertyChanged(nameof(this.ButtonColor));
                this.RaisePropertyChanged(nameof(this.TextColor));
                this.RaisePropertyChanged(nameof(this.UserNameIsError));
            } 
        }

        public List<SSO> ListSSO { get; set;}

        public string Password
        {
            get => this.password;
            set {
                SetProperty(ref this.password, value);
                this.PasswordIsError = false;
                this.RaisePropertyChanged(nameof(ButtonColor));
                this.RaisePropertyChanged(nameof(TextColor));
                this.RaisePropertyChanged(nameof(this.PasswordIsError));
            } 
        }

        private String library;

        public String Library
        {
            get => this.library;
            set
            {
                SetProperty(ref this.library, value);
            }
        }

        public IMvxAsyncCommand ConnectCommand
        {
            get
            {
                this.connectCommand = this.connectCommand ?? new MvxAsyncCommand(ConnectCommandHandler);
                return this.connectCommand;
            }
        }

        public string ButtonColor => this.Password?.Length > 0 && this.UserName?.Length > 0 ? "#0066ff" : "#EAEAEA";

        public string TextColor => this.Password?.Length > 0 && this.UserName?.Length > 0 ? "White" : "#0066ff";

        public bool IsLoading => this.isLoading;

        public bool UserNameIsError { get; private set; } = false;

        public bool PasswordIsError { get; private set; } = false;

        public string UserNameErrorString { get; private set; }

        public string PasswordErrorString { get; private set; }

        public CookiesSave department;

        public List<StandartViewList> departmentStandarViewList;

        public bool CanDisplayForgetMDP { get { return department.ForgetMdpUrl != ""; } }

        public LoginViewModel(
                              IRequestService requestService,
                              IGeolocationService geolocationService, 
                              IDepartmentService departmentService,
                              IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.geolocationService = geolocationService;
            this.departmentService = departmentService;
            this.navigationService = navigationService;
        }

#pragma warning disable S1006 // Method overrides should not change parameter defaults
        public override void ViewDestroy(bool viewFinishing = false)
#pragma warning restore S1006 // Method overrides should not change parameter defaults
        {
            viewFinishing = false;
            base.ViewDestroy(viewFinishing);
        }

        private async Task ConnectCommandHandler()
        {
            if (string.IsNullOrEmpty(this.UserName))
            {
                await this.SetUsernameError(String.Format(ApplicationResource.MissingField));
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await this.SetPasswordError(String.Format(ApplicationResource.MissingField));
                return;
            }
            isLoading = true;
            await RaisePropertyChanged(nameof(IsLoading));
            bool error = await ConnectApiCall();
            if (error) { 
                await this.navigationService.Navigate<MasterDetailViewModel>();
            }
            else { 
                isLoading = false;
                await RaisePropertyChanged(nameof(IsLoading));
            }
        }

        private async Task<bool> ConnectApiCall()
        {
  
            var result = await this.requestService.Authentication(this.username, this.password, this.department.LibraryUrl, (x) =>
            {
                this.DisplayAlert(ApplicationResource.Error, x.Message, ApplicationResource.ButtonValidation);
            });

            if (result == null)
                return false;
            else  if (!result.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, result.Errors?[0]?.Msg, ApplicationResource.ButtonValidation);
                return false;
            }
            CookiesSave b = await App.Database.GetByUsernameAsync(this.username);
            if (b == null)
            {
                CookiesSave item = new CookiesSave();
                item = department;
                item.Username = this.username;
                item.Active = true;
                item.Cookies = JsonConvert.SerializeObject(this.requestService.GetCookies().ToArray());
                await App.Database.SaveItemAsync(item);
                this.requestService.LoadCookies(JsonConvert.DeserializeObject<Cookie[]>(item.Cookies));
                item.DomainUrl = department.DomainUrl;
            else
            {
                b = department;
                b.Username = this.username;
                b.Active = true;
                b.Cookies = JsonConvert.SerializeObject(this.requestService.GetCookies().ToArray());
                await App.Database.SaveItemAsync(b);
                this.requestService.LoadCookies(JsonConvert.DeserializeObject<Cookie[]>(b.Cookies));
            }

                b.Department = department.Department;
                b.SearchScenarioCode = department.SearchScenarioCode;
                b.EventsScenarioCode = department.EventsScenarioCode;
                b.IsEvent = department.IsEvent;
                b.RememberMe = department.RememberMe;
                b.IsKm = department.IsKm;
                b.BuildingInfos = department.BuildingInfos;
                await App.Database.SaveItemAsync(b);
                this.requestService.LoadCookies(JsonConvert.DeserializeObject<Cookie[]>(b.Cookies));
            }
            foreach (var item in this.departmentStandarViewList)
            {
                item.Username = this.username;
            }
            List<StandartViewList> removeStandardList = await App.Database.GetActiveStandartView(b);
            foreach (var removeItem in removeStandardList)
            {
                await App.Database.DeleteItemAsync(removeItem);
            }
            var status = await this.requestService.RenderAccountWebFrame(new AccountWebFrameOptions());
            return true;
        }

        private async Task SetUsernameError(string message)
        {
            this.UserNameIsError = true;
            this.UserNameErrorString = message;
            await this.RaisePropertyChanged(nameof(this.UserNameIsError));
            await this.RaisePropertyChanged(nameof(this.UserNameErrorString));
        }

        private async Task SetPasswordError(string message)
        {
            this.PasswordIsError = true;
            this.PasswordErrorString = message;
            await this.RaisePropertyChanged(nameof(this.PasswordIsError));
            await this.RaisePropertyChanged(nameof(this.PasswordErrorString));
        }

        public override void Prepare(LoginParameters parameter)
        {
            this.department = parameter.CookiesSave;
            this.departmentStandarViewList = parameter.StandartViewList;
            this.ListSSO = parameter.ListSSO;
            this.Library = this.department.Library;
        }

        public void GetBarCodeResult()
        {
            // Method intentionally left empty.
        }
        public void OpenWebBrowser(string parameter)
        {
            try
            {
                WebAndCookiesAuthentificationParameters parameters = new WebAndCookiesAuthentificationParameters(parameter, this.department);
                this.navigationService.Navigate<WebAndCookiesAuthentificationViewModel, WebAndCookiesAuthentificationParameters>(parameters);
            }
            catch (Exception e)
            {
                Console.WriteLine("OpenWebBrowser Exception: " + e.ToString());
            }

        }
    }
}
