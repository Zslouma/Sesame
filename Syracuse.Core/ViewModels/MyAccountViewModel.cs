using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class MyAccountViewModel : BaseViewModel
    {
        private AccountInfoViewModel accountInfoViewModel;
        public AccountInfoViewModel AccountInfoViewModel
        {
            get => this.accountInfoViewModel;
            set => SetProperty(ref this.accountInfoViewModel, value);
        }

        private DisplayCardViewModel displayCardViewModel;
        public DisplayCardViewModel DisplayCardViewModel
        {
            get => this.displayCardViewModel;
            set => SetProperty(ref this.displayCardViewModel, value);
        }

        private SummaryAccount accountSummary;
        public SummaryAccount SummaryAccount
        {
            get => this.accountSummary;
            set
            {
                SetProperty(ref this.accountSummary, value);
            }

        }
        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));


        private IRequestService requestService { get; set; }
        private readonly IMvxNavigationService navigationService;


        public MyAccountViewModel(IMvxNavigationService navigationService, IRequestService requestService) 
        {
            this.requestService = requestService;
            this.navigationService = navigationService;

            CookiesSave CookiesSave = App.Database.GetByIDAsync().Result;

            this.AccountInfoViewModel = new AccountInfoViewModel(requestService);
            this.DisplayCardViewModel = new DisplayCardViewModel(CookiesSave);

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

        public async override Task Initialize()
        {
            await base.Initialize();

            var response = await this.requestService.GetSummary();
            this.SummaryAccount = response.D.AccountSummary;

            this.AccountInfoViewModel.SummaryAccount = this.SummaryAccount;

            this.DisplayCardViewModel.SummaryAccount = this.SummaryAccount;

            await this.AccountInfoViewModel.Initialize();
            await this.DisplayCardViewModel.Initialize();
        }
    }
}
