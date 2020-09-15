using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService;

        private ObservableCollection<MenuItem> menuItemList;
        public ObservableCollection<MenuItem> MenuItemList
        {
            get => this.menuItemList;
            set => SetProperty(ref this.menuItemList, value);
        }

        private IMvxAsyncCommand<string> showDetailPageCommand;
        public IMvxAsyncCommand<string> ShowDetailPageCommand
        {
            get => this.showDetailPageCommand;
            set => SetProperty(ref this.showDetailPageCommand, value);
        }

        private String displayName;
        public String DisplayName
        {
            get => this.displayName;
            set
            {
                SetProperty(ref this.displayName, value);
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


        public MenuViewModel(IMvxNavigationService navigationService,
            IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.menuItemList = new ObservableCollection<MenuItem>()
            {
                new MenuItem() { Text = ApplicationResource.Home, IconImageSource = "home" },
                new MenuItem() { Text = ApplicationResource.Account, IconImageSource = "profile" },
                new MenuItem() { Text = ApplicationResource.OtherAccount, IconImageSource = "allaccounts" },
                new MenuItem() { Text = ApplicationResource.Bookings, IconImageSource = "reservation" },
                new MenuItem() { Text = ApplicationResource.Loans, IconImageSource = "borrowing" },
                new MenuItem() { Text = ApplicationResource.Scan, IconImageSource = "borrowing" },
                new MenuItem() { Text = ApplicationResource.Library, IconImageSource = "library" },
                new MenuItem() { Text = ApplicationResource.About, IconImageSource = "library" },
                new MenuItem() { Text = ApplicationResource.Disconect, IconImageSource = "library" },
            };

            this.requestService = requestService;
            this.ShowDetailPageCommand = new MvxAsyncCommand<string>(this.ShowDetailPageAsync);
        }


        public override async void Prepare()
        {

            CookiesSave user = App.Database?.GetActiveUser()?.Result;
            if (user != null)
            {
                this.Library = user.Library;
                if (string.IsNullOrEmpty(user?.DisplayName))
                {
                    AccountSummary account = await requestService.GetSummary();
                    user.DisplayName = account?.D?.AccountSummary?.DisplayName;
                    await App.Database.SaveItemAsync(user);
                    this.DisplayName = user.DisplayName;
                }
                else
                {
                    this.DisplayName = user.DisplayName;
                }
            }
        }


        private async Task ShowDetailPageAsync(string name)
        {
            if (name == ApplicationResource.Home)
                _ = this.navigationService.Navigate<HomeViewModel>();
            else if (name == ApplicationResource.Bookings)
                _ = this.navigationService.Navigate<BookingViewModel>();
            else if (name == ApplicationResource.Scan)
                _ = this.navigationService.Navigate<BarcodeSearchModel>();
            else if (name == ApplicationResource.Loans)
                _ = this.navigationService.Navigate<LoansViewModel>();
            else if (name == ApplicationResource.Account)
                _ = this.navigationService.Navigate<MyAccountViewModel>();
            else if (name == ApplicationResource.OtherAccount)
                _ = this.navigationService.Navigate<OtherAccountViewModel>();
            else if (name == ApplicationResource.Disconect) {
                var user = await App.Database.GetActiveUser();
                user.Active = false;
                await App.Database.SaveItemAsync(user);
                await this.navigationService.Navigate<SelectLibraryViewModel>();
            }
            else if (name == ApplicationResource.Library)
                await this.navigationService.Navigate<LibraryViewModel>();
            else if (name == ApplicationResource.About)
                await this.navigationService.Navigate<AboutViewModel>();
            /*
             * Close left side menu. 
             */
            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
                masterDetailPage.IsPresented = false;
            else if (Application.Current.MainPage is NavigationPage navigationPage && 
                     navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
                nestedMasterDetail.IsPresented = false;
        }
    }
}
