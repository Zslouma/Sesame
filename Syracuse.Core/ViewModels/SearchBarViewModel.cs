using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using Syracuse.Mobitheque.Core.ViewModels.Sorts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class SearchBarViewModel : BaseViewModel<SearchOptions, SearchOptions>
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private List<string> searchHistory;
        public List<string> SearchHistory
        {
            get => this.searchHistory;
            set
            {
                SetProperty(ref this.searchHistory, value);
            }
        }

        private D d;
        public D D
        {
            get => this.d;
            set
            {
                SetProperty(ref this.d, value);
            }
        }

        private long? resultCount;
        public long? ResultCount
        {
            get => this.resultCount;
            set
            {
                SetProperty(ref this.resultCount, value);
            }
        }


        private FacetCollectionList[] facetCollectionList;
        public FacetCollectionList[] FacetCollectionList
        {
            get => this.facetCollectionList;
            set
            {
                SetProperty(ref this.facetCollectionList, value);
            }
        }


        private string searchQuery;
        public string SearchQuery
        {
            get => this.searchQuery;
            set
            {
                SetProperty(ref this.searchQuery, value);
            }
        }

        private string filterName;
        public string FilterName
        {
            get => this.filterName;
            set
            {
                SetProperty(ref this.filterName, value);
            }
        }


        private int sortOrder = 0;
        public int SortOrder
        {
            get => this.sortOrder;
            set
            {
                SetProperty(ref this.sortOrder, value);
            }
        }

        async public override void Prepare(SearchOptions parameter)
        {
            CookiesSave b = await App.Database.GetActiveUser();
            if (b.SearchValue != null)
                this.SearchHistory = b.SearchValue.Split(',').ToList();
            else
                this.SearchHistory = new List<string>();
        }

        private MvxAsyncCommand<SearchResult> openDetailsCommand;
        public MvxAsyncCommand<SearchResult> OpenDetailsCommand => this.openDetailsCommand ??
            (this.openDetailsCommand = new MvxAsyncCommand<SearchResult>((result) => this.OpenResultDetails(result)));

        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private readonly Dictionary<string, Func<IEnumerable<Result>, IEnumerable<Result>>> filters =
        new Dictionary<string, Func<IEnumerable<Result>, IEnumerable<Result>>>()
        {
                    { "A à Z (Titre)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Title")},
                    { "Z à A (Titre)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Title")},
                    { "A à Z (Auteur)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Author")},
                    { "Z à A (Auteur)", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Author")},
                    { "+ récent", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Date")},
                    { "- récent", (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Date")}
        };

        public List<string> FiltersName
        {
            get
            {
                Dictionary<string, Func<IEnumerable<Result>, IEnumerable<Result>>>.KeyCollection keys = this.filters.Keys;
#pragma warning disable S2365 // Properties should not make collection or array copies
                return keys.ToList();
#pragma warning restore S2365 // Properties should not make collection or array copies
            }
        }

        public async Task fillHistory()
        {
            CookiesSave b = await App.Database.GetActiveUser();
            if (b.SearchValue != null)
                this.SearchHistory = b.SearchValue.Split(',').ToList();
            else
                this.SearchHistory = new List<string>();
        }
        public SearchBarViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
            Task.Run(async () => await fillHistory());
        }


        public async Task PerformSearch(string search = null)
        {
            var options = new SearchOptionsDetails()
            {
                QueryString = search
            };
            SearchOptions opt = new SearchOptions() { Query = options };

            CookiesSave b = await App.Database.GetActiveUser();

            if (b.SearchValue != null)
            {
                var list = b.SearchValue.Split(',').ToList();
                if (!list.Contains(search))
                {
                    list.Insert(0, search);
                }
                this.SearchHistory = list;
                b.SearchValue = string.Join(",", list);
                await App.Database.SaveItemAsync(b);
            }

            else
            {
                this.SearchHistory = new List<string>();
                this.SearchHistory.Add(search);
                b.SearchValue = search;
                await App.Database.SaveItemAsync(b);
            }

            await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);

        }

        private async Task OpenResultDetails(SearchResult result)
        {
            var parameter = new SearchResult[2];
            parameter[0] = result;
            parameter[1] = new SearchResult();
            parameter[1].D = new D();
            parameter[1].D = this.D;
            this.ResultCount = this.D?.SearchInfo?.NbResults;
            SearchOptions searchOptions = new SearchOptions();
            searchOptions.Query = new SearchOptionsDetails()
            {
                QueryString = this.SearchQuery
            };
            var tempo = new SearchDetailsParameters()
            {
                parameter = parameter,
                searchOptions = searchOptions,
                nbrResults = this.ResultCount.ToString()
            };
            await this.navigationService.Navigate<SearchDetailsViewModel, SearchDetailsParameters>(tempo);
        }

    }
}