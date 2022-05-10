
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Navigation.EventArguments;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService;

        private ObservableCollection<MenuNavigation> menuItemList = new ObservableCollection<MenuNavigation>();
        public ObservableCollection<MenuNavigation> MenuItemList
        {
            get => this.menuItemList;
            set => SetProperty(ref this.menuItemList, value);
        }

        private ObservableCollection<MenuNavigation> menuItemListTempo = new ObservableCollection<MenuNavigation>();
        public ObservableCollection<MenuNavigation> MenuItemListTempo
        {
            get => this.menuItemListTempo;
            set => SetProperty(ref this.menuItemListTempo, value);
        }



        private Dictionary<string, string> DictionaryViewModelLabel = new Dictionary<string, string>();

        private String displayName = "";
        public String DisplayName
        {
            get => this.displayName;
            set
            {
                SetProperty(ref this.displayName, value);
            }
        }

        private CookiesSave user = new CookiesSave();
        public CookiesSave User
        {
            get => this.user;
            set
            {
                SetProperty(ref this.user, value);
            }
        }

        private String library = "";

        public String Library
        {
            get => this.library;
            set
            {
                SetProperty(ref this.library, value);
            }
        }

        private bool menuIsVisible = false;

        public bool MenuIsVisible
        {
            get => this.menuIsVisible;
            set
            {
                SetProperty(ref this.menuIsVisible, value);
            }
        }

        private List<StandartViewList> standartViewLists = new List<StandartViewList>();

        public List<StandartViewList> StandartViewLists
        {
            get => this.standartViewLists;
            set
            {
                SetProperty(ref this.standartViewLists, value);
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

        private string tag = "Mobidoc";


        public MenuViewModel(IMvxNavigationService navigationService,
            IRequestService requestService)
        {
            this.DictionaryViewModelLabel.Add("Syracuse.Mobitheque.Core.ViewModels.BookingViewModel", ApplicationResource.Bookings);
            this.DictionaryViewModelLabel.Add("Syracuse.Mobitheque.Core.ViewModels.LoansViewModel", ApplicationResource.Loans);
            this.navigationService = navigationService;
            this.navigationService.AfterNavigate += LoansNavigation;
            this.requestService = requestService;
            Log.Warning("Mobidoc", "NavigationService CanNavigate: " + this.navigationService.CanNavigate<PinnedDocumentViewModel>().ToString());
        }

        public void AddStandardView()
        {
            UnicodeEncoding unicode = new UnicodeEncoding();
            foreach (var item in this.StandartViewLists)
            {
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = item.ViewName, IsEnabled = App.AppState.NetworkConnection, IconFontAwesome = item.ViewIcone, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
            }
        }

        public async Task NavigationStandardView(string name)
        {
            try
            {
                foreach (var item in this.StandartViewLists)
            {
                if (name == item.ViewName)
                {
                    SearchOptions searchOptions = new SearchOptions();
                    searchOptions.Query = new SearchOptionsDetails()
                    {
                        QueryString = item.ViewQuery,
                        ScenarioCode = item.ViewScenarioCode,
                    };
                    searchOptions.PageTitle = name;
                    searchOptions.PageIcone = item.ViewIcone;
                    await this.navigationService.Navigate<StandardViewModel, SearchOptions>(searchOptions);
                }
            }
            }
            catch (Exception ex)
            {
                this.DisplayAlert(ApplicationResource.Error, ex.Message, ApplicationResource.ButtonValidation);
            }
        }
        private async void VariableChangeHandler(bool newVal)
        {
            await this.CreateMenuItemList();
        }
        public override void ViewAppeared()
        {
            App.AppState.OnVariableChange += VariableChangeHandler;
            this.navigationService.AfterNavigate += LoansNavigation;
            this.MenuIsVisible = true;
            base.ViewAppeared();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            App.AppState.OnVariableChange -= VariableChangeHandler;
            base.ViewDestroy(viewFinishing);
        }

        public override async void Prepare()
        {
            Log.Warning(tag, "Mobidoc Prepare Start");
            this.MenuIsVisible = false;
            try
            {
                Log.Warning(tag, "Mobidoc Prepare User Start");
                this.User = await App.Database.GetActiveUser();
                Log.Warning(tag, "Mobidoc Prepare User End");
                Log.Warning(tag, "Mobidoc Prepare StandartViewLists Start");
                this.StandartViewLists = await App.Database.GetActiveStandartView(this.User);
                Log.Warning(tag, "Mobidoc Prepare StandartViewLists End");
                if (this.User != null)
                {
                    this.IsKm = this.User.IsKm;
                    this.Library = this.User.Library;
                    if (string.IsNullOrEmpty(this.User.DisplayName))
                    {
                        AccountSummary account = await requestService.GetSummary();
                        if (account.Success)
                        {
                            this.User.DisplayName = account.D.AccountSummary.DisplayName;
                            await App.Database.SaveItemAsync(this.User);
                            this.DisplayName = this.User.DisplayName;
                        }
                    }
                    else
                    {
                        this.DisplayName = this.User.DisplayName;
                    }
                }
                await this.CreateMenuItemList();
            }
            catch (Exception ex)
            {
                Log.Warning(tag, ex.Message);
                this.DisplayAlert(ApplicationResource.Error, ex.Message, ApplicationResource.ButtonValidation);
            }
            await RaiseAllPropertiesChanged();
            base.Prepare();
            this.MenuIsVisible = true;
            Log.Warning(tag, "Mobidoc Prepare End");
        }

        private async Task CreateMenuItemList()
        {
            Log.Warning(tag, "Start CreateMenuItemList");

            try
            {
            this.MenuItemListTempo = new ObservableCollection<MenuNavigation>() { };
            this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Home, IconFontAwesome = "\uf015", IsSelected = true, IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Account, IconFontAwesome = "\uf007", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.OtherAccount, IconFontAwesome = "\uf0c0", IsEnabled = true, Color = "WhiteSmoke" });
                if (!IsKm)
                {
                    this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Bookings, IconFontAwesome = "\uf017", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                    this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Loans, IconFontAwesome = "\uf02d", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                }
                else
                {
                    this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.PinnedDocuments, IconFontAwesome = "\uf08d", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                    this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Download, IconFontAwesome = "\uf019", IsEnabled = true, Color = "WhiteSmoke" });
                }
                this.AddStandardView();
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Scan, IconFontAwesome = "\uf02a", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Library, IconFontAwesome = "\uf1ad", IsEnabled = App.AppState.NetworkConnection, Color = App.AppState.NetworkConnection ? "WhiteSmoke" : "LightSlateGray" });
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.About, IconFontAwesome = "\uf05a", IsEnabled = true, Color = "WhiteSmoke" });
                this.MenuItemListTempo.Add(new MenuNavigation() { Text = ApplicationResource.Disconnect, IconFontAwesome = "\uf011", IsEnabled = true, Color = "WhiteSmoke" });
                this.MenuItemList = MenuItemListTempo;
                await this.RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                Log.Warning(tag, ex.Message);
                this.DisplayAlert(ApplicationResource.Error, ex.Message, ApplicationResource.ButtonValidation);
            }
            Log.Warning(tag, "End CreateMenuItemList");
        }

        private async Task RefreshMenuItem( string name)
        {
            try
            {
            var MenuItemListtempo = this.MenuItemList;
            foreach (var item in MenuItemListtempo)
            {
                if (item.Text == name)
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
            this.MenuItemList = new ObservableCollection<MenuNavigation>();
            this.MenuItemList = MenuItemListtempo;
            await this.RaiseAllPropertiesChanged();
            }
            catch (Exception ex)
            {
                this.DisplayAlert(ApplicationResource.Error, ex.Message, ApplicationResource.ButtonValidation);
            }
        }
        public async void CheckNavigation(string name)
        {
            Log.Warning(tag, "Start CheckNavigation");
            try
            {
                Log.Warning("Mobidoc", "NavigationService CanNavigate: " + this.navigationService.CanNavigate<PinnedDocumentViewModel>().ToString());
                if (name == ApplicationResource.Home)
                    await this.navigationService.Navigate<HomeViewModel>();
                else if (name == ApplicationResource.PinnedDocuments)
                    await this.navigationService.Navigate<PinnedDocumentViewModel>();
                else if (name == ApplicationResource.Download)
                    await this.navigationService.Navigate<DownloadViewModel>();
                else if (name == ApplicationResource.Bookings)
                    await this.navigationService.Navigate<BookingViewModel>();
                else if (name == ApplicationResource.Scan)
                    await this.navigationService.Navigate<BarcodeSearchModel>();
                else if (name == ApplicationResource.Loans)
                    await this.navigationService.Navigate<LoansViewModel>();
                else if (name == ApplicationResource.Account)
                    await this.navigationService.Navigate<MyAccountViewModel>();
                else if (name == ApplicationResource.OtherAccount)
                    await this.navigationService.Navigate<OtherAccountViewModel>();
                else if (name == ApplicationResource.Disconnect)
                {
                    CookiesSave user = await App.Database.GetActiveUser();
                    user.Active = false;
                    await App.Database.SaveItemAsync(user);
                    await this.navigationService.Navigate<SelectLibraryViewModel>();
                }
                else if (name == ApplicationResource.Library)
                    await this.navigationService.Navigate<LibraryViewModel>();
                else if (name == ApplicationResource.About)
                    await this.navigationService.Navigate<AboutViewModel>();
                else
                    await this.NavigationStandardView(name);

            }
            catch (Exception e)
            {
                Log.Warning(tag, e.Message);
            }
            await this.RaiseAllPropertiesChanged();
            Log.Warning(tag, "End CheckNavigation");
        }

        [Obsolete]
        public async Task ShowDetailPageAsync(MenuNavigation item)
        {
            var name = item.Text;
            try
            {
            Log.Warning(tag, "Start ShowDetailPageAsync");
            await this.RefreshMenuItem(name);
            if (item.IsEnabled)
            {
                this.CheckNavigation(name);
            }
            Log.Warning(tag, "End ShowDetailPageAsync");
            /*
            Close left side menu. 
            */
            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
                masterDetailPage.IsPresented = false;
            else if (Application.Current.MainPage is NavigationPage navigationPage &&
                        navigationPage.CurrentPage is MasterDetailPage nestedMasterDetail)
                nestedMasterDetail.IsPresented = false;

            }
            catch (Exception e)
            {
                Log.Warning(tag, e.Message);
            }
        }

        /// <summary>
        /// Permet de detecter une navigation hors menu et de changer l'affichage du menu en conséquent 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoansNavigation(object sender, IMvxNavigateEventArgs e)
        {
            
            var key = e.ViewModel.ToString();
            if (DictionaryViewModelLabel.ContainsKey(key)) {
                this.RefreshMenuItem(DictionaryViewModelLabel[key]).Wait();
            }

        }
    }
}
