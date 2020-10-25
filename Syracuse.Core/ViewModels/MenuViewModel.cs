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

        private ObservableCollection<MenuNavigation> menuItemList;
        public ObservableCollection<MenuNavigation> MenuItemList
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
        private bool isKm = false;

        public bool IsKm
        {
            get => this.isKm;
            set
            {
                SetProperty(ref this.isKm, value);
            }
        }


        public MenuViewModel(IMvxNavigationService navigationService,
            IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.menuItemList = new ObservableCollection<MenuNavigation>()
            {
                new MenuNavigation() { Text = ApplicationResource.Home, IconImageSource = "home" , IsSelected = true},
                new MenuNavigation() { Text = ApplicationResource.Account, IconImageSource = "profile" },
                new MenuNavigation() { Text = ApplicationResource.OtherAccount, IconImageSource = "allaccounts" },
                new MenuNavigation() { Text = ApplicationResource.Bookings, IconImageSource = "reservation" },
                new MenuNavigation() { Text = ApplicationResource.Loans, IconImageSource = "borrowing" },
                new MenuNavigation() { Text = ApplicationResource.Scan, IconImageSource = "borrowing" },
                new MenuNavigation() { Text = ApplicationResource.Library, IconImageSource = "library" },
                new MenuNavigation() { Text = ApplicationResource.About, IconImageSource = "library" },
                new MenuNavigation() { Text = ApplicationResource.Disconnect, IconImageSource = "library" },
            };

            this.requestService = requestService;
        }
        public override async void Prepare()
        {

            CookiesSave user = App.Database?.GetActiveUser()?.Result;
            if (user != null)
            {
                this.IsKm = user.IsKm;
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
                if (this.IsKm)
                {
                    this.menuItemList = new ObservableCollection<MenuNavigation>()
                    {
                        new MenuNavigation() { Text = ApplicationResource.Home, IconImageSource = "home", IsSelected = true },
                        new MenuNavigation() { Text = ApplicationResource.Account, IconImageSource = "profile"},
                        new MenuNavigation() { Text = ApplicationResource.OtherAccount, IconImageSource = "allaccounts"},
                        new MenuNavigation() { Text = ApplicationResource.Scan, IconImageSource = "borrowing"},
                        new MenuNavigation() { Text = ApplicationResource.Library, IconImageSource = "library"},
                        new MenuNavigation() { Text = ApplicationResource.About, IconImageSource = "library"},
                        new MenuNavigation() { Text = ApplicationResource.Disconnect, IconImageSource = "library"},
                    };
                   
                }
            }
            await this.RaiseAllPropertiesChanged();
            this.ShowDetailPageCommand = new MvxAsyncCommand<string>(this.ShowDetailPageAsync);
            
        }


        private async Task ShowDetailPageAsync(string name)
        {
            var MenuItemListtempo = this.MenuItemList;
            foreach (var item in MenuItemListtempo)
            {
                if (item.Text == name)
                {
                    item.IsSelected = true;
                }
                else { 
                    item.IsSelected = false;
                }
            }
            this.MenuItemList = new ObservableCollection<MenuNavigation>();
            this.MenuItemList = MenuItemListtempo;
            await this.RaiseAllPropertiesChanged();
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
            else if (name == ApplicationResource.Disconnect) {
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
