using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private IRequestService requestService { get; set; }
        private readonly IMvxNavigationService navigationService;

        private string versionLabel;
        public string VersionLabel
        {
            get => this.versionLabel;
            set
            {
                SetProperty(ref this.versionLabel, value);
            }
        }
        private string copyrightLabel;
        public string CopyrightLabel
        {
            get => this.copyrightLabel;
            set
            {
                SetProperty(ref this.copyrightLabel, value);
            }
        }
        public AboutViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }
        private async Task PerformSearch(string search)
        {
            var options = new SearchOptionsDetails() { QueryString = search };
            SearchOptions opt = new SearchOptions() { Query = options };
            await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
        }
    }
}
