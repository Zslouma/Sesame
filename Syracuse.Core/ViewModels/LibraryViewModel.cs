using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class LibraryViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;
        private ObservableCollection<LibraryInformations> itemsSource;
        public ObservableCollection<LibraryInformations> ItemsSource
        {
            get => this.itemsSource;
            set { SetProperty(ref this.itemsSource, value); }
        }

        private bool headerVisibility;
        public bool HeaderVisibility
        {
            get => this.headerVisibility;
            set { SetProperty(ref this.headerVisibility, value); }
        }

        private bool absoluteIndicatorVisibility;
        public bool AbsoluteIndicatorVisibility
        {
            get => this.absoluteIndicatorVisibility;
            set { SetProperty(ref this.absoluteIndicatorVisibility, value); }
        }

        #region SearchBar
        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private async Task PerformSearch(string search)
        {
            var options = new SearchOptionsDetails()
            {
                QueryString = search
            };
            SearchOptions opt = new SearchOptions() { Query = options };
            if (App.AppState.NetworkConnection)
            {
                await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
            }
            else
            {
                this.DisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
            }
        }

        #endregion

        public LibraryViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;

        }

        public override void Prepare()
        {
            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.Intitalsation();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            base.Prepare(); 
        }

        public async Task Intitalsation()
        {
            CookiesSave user = await App.Database.GetActiveUser();
            this.ItemsSource = JsonConvert.DeserializeObject<ObservableCollection<LibraryInformations>>(user.BuildingInfos);
            if (this.ItemsSource.Count == 1)
            {
                this.HeaderVisibility = false;
            }
            else
            {
                this.HeaderVisibility = true;
            }
            await this.RaiseAllPropertiesChanged();


        }

        public void WebViewError(LibraryInformations Result, bool displayNavigationError)
        {
            int indice = this.ItemsSource.IndexOf(Result);
            if (this.ItemsSource[indice].DisplayNavigationError != displayNavigationError)
            {
                Result.DisplayNavigationError = displayNavigationError;
                this.ItemsSource[indice] = Result;
            }
        }
    }
}
