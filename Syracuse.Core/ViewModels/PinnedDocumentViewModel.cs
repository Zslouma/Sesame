using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class PinnedDocumentViewModel : BaseDownloadPageViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<Result> downloadDocumentCommand;
        public MvxAsyncCommand<Result> DownloadDocumentCommand => this.downloadDocumentCommand ??
            (this.downloadDocumentCommand = new MvxAsyncCommand<Result>((item) => this.DownloadDocument(item)));

        public bool IsBusy { get; set; }
        public bool NotCurrentBasket { get; set; }
        public bool ReversNotCurrentBasket
        {
            get => !this.NotCurrentBasket;
        }
        private Result[] results;
        public Result[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
                this.DisplayLoadMore = value.Count() < this.ResultCountInt;
            }
        }

        public Task GoToDetailView(Result item)
        {
            throw new NotImplementedException();
        }

        private MvxAsyncCommand<string> loadMore;
        public MvxAsyncCommand<string> LoadMore => this.loadMore ??
            (this.loadMore = new MvxAsyncCommand<string>((text) => this.getNextPage()));

        public int page { get; private set; } = 0;

        private long? resultCountInt;
        public long? ResultCountInt
        {
            get => this.resultCountInt;
            set
            {
                SetProperty(ref this.resultCountInt, value);
            }
        }

        public bool DisplayLoadMore { get; private set; }

        private async Task getNextPage()
        {
            this.IsBusy = true;
            this.page += 1;
            Result[] res = await PerformBasketSearch();
            this.Results = this.Results.Concat(res).ToArray();
            this.IsBusy = false;
            await RaiseAllPropertiesChanged();
            await GetRedirectURL();
        }



        public PinnedDocumentViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        /// <summary>
        /// Déclenche une oppération de télecharcheement de document 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task DownloadDocument(Result result)
        {
            CookiesSave user = await App.Database.GetActiveUser();
            this.IsBusy = true;
            await RaiseAllPropertiesChanged();
            var url = user.DomainUrl;
            var statusDownload = await this.requestService.GetDownloadDocument(result.downloadOptions.parentDocumentId, result.downloadOptions.documentId, result.downloadOptions.fileName);
            if (statusDownload.Success)
            {
                await SaveNewDocumentDatabaseObject(result, statusDownload.D);
                foreach (var Result in this.Results)
                {
                    if (Result == result)
                    {
                        Result.CanDownload = false;
                        Result.IsDownload = true;
                    }
                }
            }
            else
            {
                this.DisplayAlert(ApplicationResource.Error, statusDownload.Errors?[0]?.Msg != null ? statusDownload.Errors?[0]?.Msg : ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
            }
            this.IsBusy = false;
            await RaiseAllPropertiesChanged();
            this.ForceListUpdate();
            await RaiseAllPropertiesChanged();

        }


        public override async void Prepare()
        {
            this.IsBusy = true;
            this.Results = await PerformBasketSearch();
            this.IsBusy = false;
            await RaiseAllPropertiesChanged();
            await GetRedirectURL();
        }

        private async Task<Result[]> PerformBasketSearch()
        {
            BasketOptions opt = new BasketOptions();
            opt.Query = new BasketOptionsDetails()
            {
                Page = this.page,
            };
            BasketResult basket = await this.requestService.SearchUserBasket(opt);
            if (!basket.Success)
            {
                this.NotCurrentBasket = true;
                this.DisplayAlert(ApplicationResource.Error, basket.Errors[0].Msg, ApplicationResource.ButtonValidation);
                return new Result[0] ;
            }
            if (basket != null && basket.D != null && basket.Success)
            {
                this.ResultCountInt = basket.D?.SearchInfo?.NbResults;
                if (basket.D.Results.Length == 0)
                {
                    this.NotCurrentBasket = true;
                }
                else
                {
                    this.NotCurrentBasket = false;
                }
            }
            return await this.HaveDownloadOption(basket.D.Results, this.requestService);

        }

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

        private async Task GetRedirectURL()
        {
            await this.RaiseAllPropertiesChanged();
        }

        private void ForceListUpdate()
        {
            var tempo = this.Results;
            this.Results = new Result[0];
            this.Results = tempo;
        }
    }
}
