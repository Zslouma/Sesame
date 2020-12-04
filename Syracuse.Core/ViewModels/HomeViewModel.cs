using MvvmCross.Binding.Extensions;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using Syracuse.Mobitheque.Core.ViewModels.Sorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.Models;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private int filterIndex = 0;
        private int page = 0;
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

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
                else {
                    this.DisplayLoadMore = false;
                }
            }
        }
        private bool notCurrentEvent = false;
        public bool NotCurrentEvent
        {
            get => this.notCurrentEvent;
            set
            {
                SetProperty(ref this.notCurrentEvent, value);
                this.NotCurrentEventReverse = !this.notCurrentEvent;
            }
        }
        private long? nbrResults;
        public long? NbrResults
        {
            get => this.nbrResults;
            set
            {
                SetProperty(ref this.nbrResults, value);
            }
        }

        private bool displayLoadMore = true;
        public bool DisplayLoadMore
        {
            get => this.displayLoadMore;
            set
            {
                SetProperty(ref this.displayLoadMore, value);
            }
        }

        private bool notCurrentEventReverse = false;
        public bool NotCurrentEventReverse
        {
            get => this.notCurrentEventReverse;
            set
            {
                SetProperty(ref this.notCurrentEventReverse, value);
            }
        }

        private bool isEvent = false;
        public bool IsEvent
        {
            get => this.isEvent;
            set
            {
                this.ReverseIsEvent = !value;
                SetProperty(ref this.isEvent, value && NotCurrentEventReverse);
            }
        }

        private bool reverseIsEvent = false;
        public bool ReverseIsEvent
        {
            get => this.reverseIsEvent;
            set
            {
                SetProperty(ref this.reverseIsEvent, value && NotCurrentEventReverse);
            }
        }

        private bool isBusy = true;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        private MvxAsyncCommand<SearchResult> openDetailsCommand;
        public MvxAsyncCommand<SearchResult> OpenDetailsCommand => this.openDetailsCommand ??
            (this.openDetailsCommand = new MvxAsyncCommand<SearchResult>((result) => this.OpenResultDetails(result)));

        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<string> loadMore;
        public MvxAsyncCommand<string> LoadMore => this.loadMore ??
            (this.loadMore = new MvxAsyncCommand<string>((text) => this.getNextPage()));


        private Command<MaterialMenuResult> filtersCommand;
        public Command<MaterialMenuResult> FiltersCommand => this.filtersCommand ??
            (this.filtersCommand = new Command<MaterialMenuResult>((filter) => this.SetFilter(filter)));

        private readonly Dictionary<string, Func<IEnumerable<SearchResult>, IEnumerable<SearchResult>>> filters =
        new Dictionary<string, Func<IEnumerable<SearchResult>, IEnumerable<SearchResult>>>()
        {
                    { "A à Z (Titre)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Title")},
                    { "Z à A (Titre)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Title")},
                    { "A à Z (Auteur)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Author")},
                    { "Z à A (Auteur)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Author")},
                    { "+ récent", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Date")},
                    { "- récent", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Date")}
        };

        public List<string> FiltersName()
        {
            return this.filters.Keys.ToList();
        }

        private String library;
        public string Library
        {
            get => this.library;
            set
            {
                SetProperty(ref this.library, value);
            }
        }
        private String searchScenarioCode;
        public string SearchScenarioCode
        {
            get => this.searchScenarioCode;
            set
            {
                SetProperty(ref this.searchScenarioCode, value);
            }
        }
        private String eventsScenarioCode;
        public string EventsScenarioCode
        {
            get => this.eventsScenarioCode;
            set
            {
                SetProperty(ref this.eventsScenarioCode, value);
            }
        }

        public HomeViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        public async override Task Initialize()
        {
            this.IsBusy = true;
            var user = await App.Database.GetActiveUser();
            this.Library = user.Library;
            this.EventsScenarioCode = user.EventsScenarioCode;
            this.SearchScenarioCode = user.SearchScenarioCode;
            this.Results = await this.loadPage(user.IsEvent);
            if (this.results.Length == 0)
            {
                this.NotCurrentEvent = true;
            }
            else
            {
                this.NotCurrentEvent = false;
            }
            this.IsEvent = user.IsEvent;

            await base.Initialize();
        }

        private async Task getNextPage()
        {

            this.page += 1;

            Result[] res = await loadPage(this.IsEvent);
            this.Results = this.Results.Concat(res).ToArray();
        }

        private async Task<Result[]> loadPage(bool IsSortField)
        {
            this.IsBusy = true;
            SearchOptions options = new SearchOptions();
            if (IsSortField)
            {
                options.Query = new SearchOptionsDetails()
                {
                    ScenarioCode = this.eventsScenarioCode,
                    Page = this.page,
                    SortField = "DateStart_sort",
                    SortOrder = 1,

                };
            }
            else
            {
                options.Query = new SearchOptionsDetails()
                {
                    ScenarioCode = this.eventsScenarioCode,
                    Page = this.page,
                };
            }
            SearchResult search = await this.requestService.Search(options);
            if (search != null && !search.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, search.Errors?[0]?.Msg, ApplicationResource.ButtonValidation);
                return new Result[0];
            }
            else
            {
                if (search != null )
                {
                    this.NbrResults = search?.D?.SearchInfo?.NbResults;
                }
                
            }
           
            this.IsBusy = false;
            return search?.D?.Results;
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
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Query = new SearchOptionsDetails()
            {
                ScenarioCode = this.eventsScenarioCode,
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
            await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);

        }

        private void SetFilter(MaterialMenuResult filter)
        {
            if (this.filterIndex == filter.Index)
                return;
            if (this.Results == null)
                return;
            this.filterIndex = filter.Index;

            
        }

        private async Task OpenResultDetails(SearchResult result)
        {
            var parameter = new SearchResult[2];
            parameter[0] = result;
            parameter[1] = new SearchResult();
            parameter[1].D = new D();
            parameter[1].D.Results = this.Results;
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Query = new SearchOptionsDetails()
            {
                ScenarioCode = this.eventsScenarioCode,
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

    }
}
