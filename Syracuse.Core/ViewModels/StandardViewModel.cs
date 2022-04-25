using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class StandardViewModel : BaseDownloadPageViewModel<SearchOptions, SearchOptions>
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        /// <summary>
        ///  searchOptions contient le scénario de recherche associé à la page 
        /// </summary>
        private SearchOptions searchOptions;

        /// <summary>
        /// searchQuery corespond a la query de la recherche associé à la page 
        /// </summary>
        private string searchQuery;
        
        /// <summary>
        /// IsBusy permet de savoir si la page procéde à des calculs ou non
        /// </summary>
        private bool isBusy { get; set; }
        public bool IsBusy 
        {  
            get { return isBusy; }
            set { this.isBusy = value; }
        }

        public D D { get; private set; }
        private Result[] results;
        public Result[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
                if (this.Results.Count() < this.NbrResults)
                {
                    this.DisplayLoadMore = true;
                }
                else
                {
                    this.DisplayLoadMore = false;
                }
            }
        }
        public long? NbrResults { get; private set; }
        private bool displayLoadMore = true;
        public bool DisplayLoadMore
        {
            get => this.displayLoadMore;
            set
            {
                SetProperty(ref this.displayLoadMore, value);
            }
        }

        public long? ResultCountInt { get; private set; }
        public string ResultCount { get; private set; }
        public string PageTitle { get; set; }
        public string PageIcone { get; set; }
        private int filterIndex = 0;
        private int page = 0;
        public FacetCollectionList[] FacetCollectionList { get; private set; }

        private string searchScenarioCode;
        public string SearchScenarioCode
        {
            get => this.searchScenarioCode;
            set
            {
                SetProperty(ref this.searchScenarioCode, value);
            }
        }
        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<Result> downloadDocumentCommand;
        public MvxAsyncCommand<Result> DownloadDocumentCommand => this.downloadDocumentCommand ??
            (this.downloadDocumentCommand = new MvxAsyncCommand<Result>((item) => this.DownloadDocument(item)));

        private MvxAsyncCommand<string> loadMore;
        public MvxAsyncCommand<string> LoadMore => this.loadMore ??
            (this.loadMore = new MvxAsyncCommand<string>((text) => this.getNextPage()));

        private CookiesSave user;

        public CookiesSave User 
        {
            get => this.user;
            set 
            {
                SetProperty(ref this.user, value);
            }
        }

        public StandardViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        async public override void Prepare(SearchOptions parameter)
        {
            this.IsBusy = true;
            this.searchOptions = parameter;
            this.PageTitle = " "+parameter.PageTitle;
            this.PageIcone = parameter.PageIcone;
            this.User = await App.Database.GetActiveUser();
            this.SearchScenarioCode = parameter.Query.ScenarioCode;
            this.searchQuery = parameter.Query.QueryString;
            this.Results = await LoadPage();
            await RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }
        public async Task GoToDetailView(Result item)
        {
            var parameter = new SearchResult[2];
            for (int i = 0; i < 2; i++)
            {
                parameter[i] = new SearchResult();
                parameter[i].D = new D();
            }
            Result[] tmpResults = { new Result() };
            parameter[0].D.Results = tmpResults;
            parameter[0].D.Results[0] = item;
            parameter[1].D.Results = tmpResults;
            parameter[1].D.Results = this.Results;
            var tempo = new SearchDetailsParameters()
            {
                parameter = parameter,
                searchOptions = this.searchOptions,
                nbrResults = this.NbrResults.ToString()
            };
            await this.navigationService.Navigate<SearchDetailsViewModel, SearchDetailsParameters>(tempo);
        }
        public async Task PerformSearch()
        {
            this.IsBusy = true;
            await RaiseAllPropertiesChanged();
            SearchOptions optionsTempo = this.searchOptions;
            SearchResult result = await this.requestService.Search(optionsTempo);

            // Result Handler
            if (result != null && result.D != null && result.Success)
            {
                this.D = result.D;
                result.D.Results = await this.CheckAvCheckAvailability(result.D.Results);
                this.Results = result.D.Results;
                this.ResultCountInt = this.D?.SearchInfo?.NbResults;
                this.ResultCount = this.ResultCountInt <= 1 ? (String.Format(ApplicationResource.SearchViewResultNull, this.D.SearchInfo.NbResults)) : (String.Format(ApplicationResource.SearchViewResultCount, this.D.SearchInfo.NbResults));
            }
            else
            {
#pragma warning disable S2259 // Null pointers should not be dereferenced
                this.DisplayAlert(ApplicationResource.Error, (result.Errors?[0]?.Msg) ?? ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
#pragma warning restore S2259 // Null pointers should not be dereferenced
                this.ResultCount = ApplicationResource.SearchViewResultNull;
            }
            this.IsBusy = false;
            await RaiseAllPropertiesChanged();

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


        public async Task<Result[]> CheckAvCheckAvailability(Result[] results)
        {

            List<RecordIdArray> RecordIdArray = new List<RecordIdArray>();
            CheckAvailabilityOptions optionsTempo = new CheckAvailabilityOptions();

            foreach (var result in results)
            {
                RecordIdArray.Add(new RecordIdArray(result.Resource.RscBase, result.Resource.RscId, result.Resource.Frmt));
            }

            optionsTempo.Query = new SearchOptionsDetails()
            {
                ScenarioCode = this.SearchScenarioCode,
                Page = this.page,
            };
            optionsTempo.RecordIdArray = RecordIdArray;

            // HTTP Request
            CheckAvailabilityResult rslts = await this.requestService.CheckAvailability(optionsTempo);

            var resultTempo = results.ToList();
            if (rslts.Success && rslts.D != null)
            {
                foreach (var rslt in rslts.D)
                {
                    int v = resultTempo.FindIndex(x => x.Resource.RscId == rslt.Id.RscId);
                    results[v].Resource.HtmlViewDisponibility = rslt.HtmlView;
                }
            }
            return results;
        }

        public async Task DownloadAllDocument(Result[] results)
        {
            foreach (var result in results)
            {
                if (result.CanDownload)
                {
                    this.DownloadDocument(result);
                }

            }
        }

        /// <summary>
        /// Déclenche une oppération de télecharchement de document 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task DownloadDocument(Result result)
        {
            this.isBusy = true;
            await RaiseAllPropertiesChanged();
            var statusDownload = await this.requestService.GetDownloadDocument(result.downloadOptions.parentDocumentId, result.downloadOptions.documentId, result.downloadOptions.fileName);
            if (statusDownload.Success)
            {
                SaveNewDocumentDatabaseObject(result, statusDownload.D);
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
            this.isBusy = false;
            this.ForceListUpdate();
            await RaiseAllPropertiesChanged();
        }

        private async Task getNextPage()
        {

            this.IsBusy = true;
            await this.RaisePropertyChanged(nameof(IsBusy));
            this.page += 1;
            Result[] res = await LoadPage();
            this.Results = this.Results.Concat(res).ToArray();
            this.IsBusy = false;
            await this.RaisePropertyChanged(nameof(IsBusy));
        }

        private async Task<Result[]> LoadPage()
        {
            this.IsBusy = true;
            SearchOptions optionsTempo = this.searchOptions;
            SearchResult result = await this.requestService.Search(optionsTempo);

            // Result Handler
            if (result != null && result.D != null && result.Success)
            {
                this.D = result.D;
                result.D.Results = await this.CheckAvCheckAvailability(result.D.Results);
                this.Results = result.D.Results;
                this.ResultCountInt = this.D?.SearchInfo?.NbResults;
                this.ResultCount = this.ResultCountInt <= 1 ? (String.Format(ApplicationResource.SearchViewResultNull, this.D.SearchInfo.NbResults)) : (String.Format(ApplicationResource.SearchViewResultCount, this.D.SearchInfo.NbResults));
            }
            else
            {
#pragma warning disable S2259 // Null pointers should not be dereferenced
                this.DisplayAlert(ApplicationResource.Error, (result.Errors?[0]?.Msg) ?? ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
#pragma warning restore S2259 // Null pointers should not be dereferenced
                this.ResultCount = ApplicationResource.SearchViewResultNull;
            }

            await RaiseAllPropertiesChanged();
            SearchOptions options = new SearchOptions();
            options.Query = new SearchOptionsDetails()
            {
                QueryString = optionsTempo.Query.QueryString,
                ScenarioCode = this.SearchScenarioCode,
                Page = this.page,
            };
            SearchResult search = await this.requestService.Search(options);
            if (search != null && !search.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, search.Errors?[0]?.Msg != null ? search.Errors?[0]?.Msg : ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                this.IsBusy = false;
                return new Result[0];
            }
            else
            {
                if (search != null)
                {
                    this.NbrResults = search?.D?.SearchInfo?.NbResults;
                    if (search.D != null)
                    {
                        search.D.Results = await this.CheckAvCheckAvailability(search.D.Results);
                    }
                }

            }
            var tempoResult = await this.HaveDownloadOption(search?.D?.Results, this.requestService);
            this.IsBusy = false;
            return tempoResult;


        }

        private void ForceListUpdate()
        {
            var tempo = this.Results;
            this.Results = new Result[0];
            this.Results = tempo;
        }
    }
}
