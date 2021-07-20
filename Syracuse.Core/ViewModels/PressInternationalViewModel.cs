using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class PressInternationalViewModel : BaseDownloadPageViewModel
    {
        private int filterIndex = 0;
        private int page = 0;
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<Result> downloadDocumentCommand;
        public MvxAsyncCommand<Result> DownloadDocumentCommand => this.downloadDocumentCommand ??
            (this.downloadDocumentCommand = new MvxAsyncCommand<Result>((item) => this.DownloadDocument(item)));

        /// <summary>
        /// Booléen qui permet de permuté l'affichage en cas d'absence de resultat 
        /// </summary>
        private bool notCurrentPress = false;
        public bool NotCurrentPress
        {
            get => this.notCurrentPress;
            set
            {
                this.ReversNotCurrentPress = !value;
                SetProperty(ref this.notCurrentPress, value);
            }
        }

        /// <summary>
        /// Booléen inverse de notCurrentPress
        /// </summary>
        private bool reversNotCurrentPress = true;
        public bool ReversNotCurrentPress
        {
            get => this.reversNotCurrentPress;
            set
            {
                SetProperty(ref this.reversNotCurrentPress, value);
            }
        }
        /// <summary>
        /// List des resultats de recherche 
        /// </summary>
        private Result[] results;
        public Result[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
                this.DisplayLoadMore = value.Count() < this.NbrResults;
            }
        }

        /// <summary>
        /// Code du scenario de recherche 
        /// </summary>
        private String internationalPressScenarioCode;
        public string InternationalPressScenarioCode
        {
            get => this.internationalPressScenarioCode;
            set
            {
                SetProperty(ref this.internationalPressScenarioCode, value);
            }
        }


        private MvxAsyncCommand<string> loadMore;
        public MvxAsyncCommand<string> LoadMore => this.loadMore ??
            (this.loadMore = new MvxAsyncCommand<string>((text) => this.getNextPage()));

        /// <summary>
        /// Code du scenario de recherche 
        /// </summary>
        private String searchQuery;
        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                SetProperty(ref this.searchQuery, value);
            }
        }


        /// <summary>
        /// variable contenant le nombres de resultats total à cette requête  
        /// </summary>
        private long? nbrResults;
        public long? NbrResults
        {
            get => this.nbrResults;
            set
            {
                SetProperty(ref this.nbrResults, value);
            }
        }

        /// <summary>
        /// Booléen indiquant si l'on peux charger de nouveaux elements suplémentaires ou non 
        /// </summary>
        private bool displayLoadMore = true;
        public bool DisplayLoadMore
        {
            get => this.displayLoadMore;
            set
            {
                SetProperty(ref this.displayLoadMore, value);
            }
        }

        /// <summary>
        /// Variable qui indique si la page est occupé 
        /// </summary>
        private bool isBusy = true;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        public PressInternationalViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        public async override Task Initialize()
        {
            this.IsBusy = true;
            var user = await App.Database.GetActiveUser();
            this.InternationalPressScenarioCode = user.InternationalPressScenarioCode;
            this.SearchQuery = user.InternationalPressQuery;
            this.Results = await this.loadPage();
            if (this.results.Length == 0)
            {
                this.NotCurrentPress = true;
            }
            else
            {
                this.NotCurrentPress = false;
            }
            await this.GetRedirectURL();
            this.IsBusy = false;
            await base.Initialize();
        }

        /// <summary>
        /// Déclenche une oppération de télecharcheement de document 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task DownloadDocument(Result result)
        {
            CookiesSave user = await App.Database.GetActiveUser();
            this.isBusy = true;
            await RaiseAllPropertiesChanged();
            var status = await this.requestService.GetListDigitalDocuments(result.Resource.RscId);
            var url = user.DomainUrl;
            var statusDownload = await this.requestService.GetDownloadDocument(status.D.Documents[0].parentDocumentId, status.D.Documents[0].documentId, status.D.Documents[0].fileName);
            if (statusDownload.Success)
            {
                var json = JsonConvert.SerializeObject(result);
                DocumentSave b = await App.DocDatabase.GetDocumentsByDocumentID(result.Resource.RscId);
                if (b == null)
                {
                    b = new DocumentSave();
                }
                b.UserID = user.ID;
                b.JsonValue = json;
                b.DocumentID = result.Resource.RscId;
                b.ImagePath = result.FieldList.Image;
                b.DocumentPath = statusDownload.D;
                await App.DocDatabase.SaveItemAsync(b);

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
            this.page += 1;
            Result[] res = await loadPage();
            this.Results = this.Results.Concat(res).ToArray();
            this.IsBusy = false;
            await GetRedirectURL();
        }

        private async Task<Result[]> loadPage()
        {
            SearchOptions options = new SearchOptions();
            options.Query = new SearchOptionsDetails()
            {
                QueryString = this.SearchQuery,
                ScenarioCode = this.InternationalPressScenarioCode,
                Page = this.page,
            };
            var search = await this.requestService.Search(options);
            if (!search.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, search.Errors?[0]?.Msg != null ? search.Errors?[0]?.Msg : ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                return new Result[0];
            }
            else
            {
                if (search != null)
                {
                    this.NbrResults = search?.D?.SearchInfo?.NbResults;
                }
            }
            return await this.HaveDownloadOption(search?.D?.Results, this.requestService);
        }


        /// <summary>
        /// Permet la navigation jusqu'a la page détail d'un object / article selectionné
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Query = new SearchOptionsDetails()
            {
                ScenarioCode = this.InternationalPressScenarioCode,
                Page = this.page,

            };
            var tempo = new SearchDetailsParameters()
            {
                parameter = parameter,
                searchOptions = searchOptions,
                nbrResults = this.NbrResults.ToString()
            };
            await this.navigationService.Navigate<SearchDetailsViewModel, SearchDetailsParameters>(tempo);
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
            foreach (var search in this.Results)
            {
                if (search.FieldList.ThumbMedium != null && search.FieldList.ThumbMedium[0] != null)
                    search.FieldList.ThumbMedium[0] = new Uri(await this.requestService.GetRedirectURL(search.FieldList.ThumbMedium[0].ToString()));
                else if (search.FieldList.ThumbSmall != null && search.FieldList.ThumbSmall[0] != null)
                    search.FieldList.ThumbSmall[0] = new Uri(await this.requestService.GetRedirectURL(search.FieldList.ThumbSmall[0].ToString()));
                else
                {
                    Debug.Write("Invalid object. ");
                }

            }
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
