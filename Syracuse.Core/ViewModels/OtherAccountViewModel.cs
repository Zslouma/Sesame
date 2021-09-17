using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class OtherAccountViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;
        public ICommand RemoveItemCommand { get; set; }

        private CookiesSave[] otherAccount;
        public CookiesSave[] OtherAccount
        {
            get => this.otherAccount;
            set
            {
                SetProperty(ref this.otherAccount, value);
            }
        }

        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        public OtherAccountViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
            RemoveItemCommand = new Command(async (object obj) => await ExecuteRemoveItemCommand(obj));
        }
        public async Task Disconnect(bool navigate = false)
        {
            CookiesSave user = await App.Database.GetActiveUser();
            user.Active = false;
            await App.Database.SaveItemAsync(user);
            this.requestService.ResetCookies();
            if (navigate)
            {
                await this.navigationService.Navigate<SelectLibraryViewModel>();
            }
        }
        public async Task ChangeAccount(CookiesSave itemTapped)
        {
            Cookie[] a = JsonConvert.DeserializeObject<Cookie[]>(itemTapped.Cookies);
            await this.Disconnect();
            itemTapped.Active = true;
            await App.Database.SaveItemAsync(itemTapped);
            this.requestService.LoadCookies(a);
            this.DisplayAlert(ApplicationResource.Success, String.Format(ApplicationResource.AccountSwitchSuccess, itemTapped.DisplayName), ApplicationResource.ButtonValidation);
            await this.navigationService.Navigate<MasterDetailViewModel>();
        }
        public async Task ExecuteRemoveItemCommand(object sender)
        {
            List<CookiesSave> cookies = new List<CookiesSave>();
            foreach (var item in this.OtherAccount)
            {
                if (item != sender)
                {
                    cookies.Add(item);
                }
                else{
                    await App.Database.DeleteItemAsync(item);
                }

            }
            this.OtherAccount = cookies.ToArray(); 
        }
        public override async Task Initialize()
        {
            CookiesSave activeUser = await App.Database.GetActiveUser();
            List<CookiesSave> tmp = new List<CookiesSave>();
            foreach (var item in await App.Database.GetItemsAsync())
            {
                if (item.Username != activeUser.Username)
                {
                    tmp.Add(item);
                }
            }
            this.OtherAccount = tmp.ToArray();
        }
        private async Task PerformSearch(string search)
        {
            var options = new SearchOptionsDetails()
            {
                QueryString = search
            };
            SearchOptions opt = new SearchOptions() { Query = options };
            await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
        }
    }
}
