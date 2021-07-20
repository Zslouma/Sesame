using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using Syracuse.Mobitheque.Core.ViewModels.Sorts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class SearchViewModel : BaseViewModel<SearchOptions, SearchOptions>
    {
        #region member

        private SearchOptions options;

        private int filterIndex = 0;

        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService;

        private List<string> searchHistory;
        public SelectableObservableCollection<FacetteValue> Itemss { get; set; }
        private ObservableCollection<FacetteGroup> facetteList { get; set; } = new ObservableCollection<FacetteGroup>();
        public ObservableCollection<FacetteGroup> FacetteList
        {
            get { return this.facetteList; }
            set { this.facetteList = value; }
        }
        private ObservableCollection<FacetteGroup> expandedFacetteList { get; set; } = new ObservableCollection<FacetteGroup>();
        public ObservableCollection<FacetteGroup> ExpandedFacetteList
        {
            get { return this.expandedFacetteList; }
            set { this.expandedFacetteList = value; }
        }

        private List<FacetteValue> oldSelectedItems = new List<FacetteValue>();
        public List<FacetteValue> OldSelectedItems
        {
            get => this.oldSelectedItems;
            set
            {
                SetProperty(ref this.oldSelectedItems, value);
            }
        }

        private List<FacetteValue> selectedItems = new List<FacetteValue>();
        public List<FacetteValue> SelectedItems
        {
            get => this.selectedItems;
            set
            {
                SetProperty(ref this.selectedItems, value);
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

        private bool displayLoadMore = true;
        public bool DisplayLoadMore
        {
            get => this.displayLoadMore;
            set
            {
                SetProperty(ref this.displayLoadMore, value);
            }
        }

        #endregion

        #region getterSetter
        public List<string> SearchHistory
        {
            get => this.searchHistory;
            set
            {
                SetProperty(ref this.searchHistory, value);
            }
        }


        private FacetteValue[] facetteChoices;
        public FacetteValue[] FacetteChoices
        {
            get => this.facetteChoices;
            set
            {
                SetProperty(ref this.facetteChoices, value);
                RaisePropertyChanged(nameof(this.facetteChoices));

            }
        }

        private int page = 0;

        private D d;
        public D D
        {
            get => this.d;
            set {
                SetProperty(ref this.d, value);
            }
        }
        private bool isFilterable = true;
        public bool IsFilterable
        {
            get => this.isFilterable;
            set
            {
                SetProperty(ref this.isFilterable, value);
            }
        }

        private string resultCount;
        public string ResultCount
        {
            get => this.resultCount;
            set
            {
                SetProperty(ref this.resultCount, value);
            }
        }
        private long? resultCountInt;
        public long? ResultCountInt
        {
            get => this.resultCountInt;
            set
            {
                SetProperty(ref this.resultCountInt, value);

                if (value == 0)
                {
                    this.DisplayLoadMore = false;
                    this.IsFilterable = false;
                    return;
                }
                else
                {
                    this.IsFilterable = true;
                }

                if (this.Results.Count() < this.ResultCountInt)
                {
                    this.DisplayLoadMore = true;
                }
                else
                {
                    this.DisplayLoadMore = false;
                }
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



        private Result[] results;
        public Result[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
                if (this.Results.Count() < this.ResultCountInt)
                {
                    this.DisplayLoadMore = true;
                }
                else
                {
                    this.DisplayLoadMore = false;
                }
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

        private string facetFilter;
        public string FacetFilter
        {
            get => this.facetFilter;
            set
            {
                SetProperty(ref this.facetFilter, value);
            }
        }

        private string sortName;
        public string SortName
        {
            get => this.sortName;
            set
            {
                SetProperty(ref this.sortName, value);
            }
        }

        private string sortNamee;
        public string SortNamee
        {
            get => this.sortNamee;
            set
            {
                if (Convert.ToInt32(value) >= 0)
                {
                    SetProperty(ref this.sortNamee, value);
                    this.SetSort(Convert.ToInt32(this.SortNamee));
                }
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
        private ObservableCollection<Facette> myList;
        public ObservableCollection<Facette> MyList
        {
            get => this.myList;
            set
            {
                SetProperty(ref this.myList, value);
            }
        }

        private async void SetSort(int filter)
        {
            if (this.filterIndex == filter)
                return;

            this.filterIndex = (filter);
            if (this.filterIndex == 0)
            {
                this.SortName = null;
                this.SortOrder = 0;
                this.PerformSearch(null, null, false);
            }
            if (this.filterIndex == 1)
            {
                this.SortName = "Title_sort";
                this.SortOrder = 1;
            }
            else if (this.filterIndex == 2)
            {
                this.SortName = "Title_sort";
                this.SortOrder = 0;
            }
            else if (this.filterIndex == 3)
            {
                this.SortName = "Author_sort";
                this.SortOrder = 1;
            }
            else if (this.filterIndex == 4)
            {
                this.SortName = "Author_sort";
                this.SortOrder = 0;
            }
            else if (this.filterIndex == 5)
            {
                this.SortName = "YearOfPublication_sort";
                this.SortOrder = 0;
            }
            else if (this.filterIndex == 6)
            {
                this.SortName = "YearOfPublication_sort";
                this.SortOrder = 1;
            }
            if (this.D == null || this.D.Results == null)
                return;
            if (this.SearchQuery == null)
                return;


            this.PerformSearch(null, this.SortName, false);
        }
        #endregion

        async public override void Prepare(SearchOptions parameter) {
            this.options = parameter;
            CookiesSave b = await App.Database.GetActiveUser();
            if (b.SearchValue != null)
                this.SearchHistory = b.SearchValue.Split(',').ToList();
            else
                this.SearchHistory = new List<string>();

            if (parameter.Query.QueryString != "")
            {
                this.SearchQuery = parameter.Query.QueryString;
                PerformSearch(parameter.Query.QueryString);
            }

        }

        #region Command

        private MvxAsyncCommand<SearchResult> openDetailsCommand;
        public MvxAsyncCommand<SearchResult> OpenDetailsCommand => this.openDetailsCommand ??
            (this.openDetailsCommand = new MvxAsyncCommand<SearchResult>((result) => this.OpenResultDetails(result)));

        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<string> loadMore;
        public MvxAsyncCommand<string> LoadMore => this.loadMore ??
            (this.loadMore = new MvxAsyncCommand<string>((text) => this.getNextPage()));

        private Command<int> sortsCommand;
        public Command<int> SortsCommand => this.sortsCommand ??
            (this.sortsCommand = new Command<int>((filter) => this.SetSort(filter)));

        #endregion

        #region commandFunction
        public void HeaderTapped(FacetteGroup facetteGroupSelected)
        {
            this.IsBusy = true;
            var selectedIndex = this.ExpandedFacetteList.IndexOf(facetteGroupSelected);
            if (selectedIndex < 0)
            {
                this.IsBusy = false;
                return;
            }
            if (!this.FacetteList[selectedIndex].Expanded)
            {
                foreach (var facette in this.FacetteList)
                {
                    facette.Expanded = false;
                }
            }
            this.FacetteList[selectedIndex].Expanded = !this.FacetteList[selectedIndex].Expanded;
            this.UpdateListContent();
            this.IsBusy = false;
        }
        private void UpdateListContent()
        {
            this.ExpandedFacetteList = new ObservableCollection<FacetteGroup>();

            foreach (FacetteGroup group in this.FacetteList)
            {
                FacetteGroup newGroup = new FacetteGroup(group.Name, group.Expanded);
                if (group.Expanded)
                {
                    foreach (var facette in group)
                    {
                        bool found = false;
                        foreach (var selectedItem in SelectedItems)
                        {
                            if (selectedItem.id == facette.id && selectedItem.value == facette.value && selectedItem.groupIndex == facette.groupIndex)
                            {
                                facette.IsSelected = true;
                                newGroup.Add(facette);
                                found = true;
                                break;
                            }
                        }
                        if (!found){
                            facette.IsSelected = false;
                            newGroup.Add(facette);
                        }
                    }
                }
                this.ExpandedFacetteList.Add(newGroup);
            }
            this.RaiseAllPropertiesChanged();
        }
        private void OnRemoveSelected()
        {
            foreach (var facette in this.FacetteList)
            {
                foreach (var item in facette)
                {
                    item.IsSelected = false;
                }
            }
            this.SelectedItems.Clear();
            this.UpdateListContent();
        }
        public void RemoveSelectedCommand()
        {
            this.OnRemoveSelected();
        }

        public async Task OnCheckSelection(FacetteValue test)
        {
            // Avoid adding title to facet list
            if (!test.noTitle) return;

            this.FacetteList[test.groupIndex][FacetteList[test.groupIndex].IndexOf(test)].IsSelected = !this.FacetteList[test.groupIndex][FacetteList[test.groupIndex].IndexOf(test)].IsSelected;
            if (this.FacetteList[test.groupIndex][FacetteList[test.groupIndex].IndexOf(test)].IsSelected)
            {
                SelectedItems.Add(this.FacetteList[test.groupIndex][FacetteList[test.groupIndex].IndexOf(test)]);
            }
            else
            {
                SelectedItems.Remove(this.FacetteList[test.groupIndex][FacetteList[test.groupIndex].IndexOf(test)]);
            }
            this.UpdateListContent();
            await RaisePropertyChanged(nameof(this.FacetteList));

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
                SortOrder = this.SortOrder,
                SortField = this.SortName,
                ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode,
                FacetFilter = this.FacetFilter,
                QueryString = this.SearchQuery,
                Page = this.page
            };
            var tempo = new SearchDetailsParameters()
            {
                parameter = parameter,
                searchOptions = searchOptions,
                nbrResults = this.ResultCountInt.ToString()
            };
            await this.navigationService.Navigate<SearchDetailsViewModel, SearchDetailsParameters>(tempo);
        }

        #endregion
        private async Task getNextPage()
        {
            this.IsBusy = true;
            this.page += 1;
            Result[] res = await loadPage();
            this.Results = this.Results.Concat(res).ToArray();
            GetRedirectURL();
            this.IsBusy = false;
        }
        public bool Equals(List<FacetteValue> NewItems, List<FacetteValue> OldItem)
        {
            if (NewItems.Count != OldItem.Count)
            {
                return false;
            }
            else
            {
                if (NewItems.Count == 0 && OldItem.Count == 0)
                {
                    return true;
                }
                else
                {
                    bool equal = true;
                    foreach (var item in NewItems)
                    {
                        if (!OldItem.Contains(item))
                        {
                            equal = false;
                            break;
                        }
                    }
                    return equal;
                }
            }
        }
        private async Task<Result[]> loadPage()
        {
            SearchOptions optionsTempo = new SearchOptions();

            optionsTempo.Query = new SearchOptionsDetails()
            {
                SortOrder = this.sortOrder,
                SortField = this.SortName,
                ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode,
                QueryString = this.SearchQuery,
                FacetFilter = this.FacetFilter,
                Page = this.page
            };
            SearchResult search = await this.requestService.Search(optionsTempo);
            if (search != null && !search.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, search.Errors?[0]?.Msg != null ? search.Errors?[0]?.Msg : ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                return new Result[0];
            }
            else if (search.Success && search.D != null)
            {
                search.D.Results = await this.CheckAvCheckAvailability(search.D.Results, this.SearchQuery);
            }
            return search?.D?.Results;
        }

        private Dictionary<string, Func<IEnumerable<Result>, IEnumerable<Result>>> sorts =
        new Dictionary<string, Func<IEnumerable<Result>, IEnumerable<Result>>>()
        {
                    { ApplicationResource.SortOptionPertinence, (x) => null},
                    { ApplicationResource.SortOptionTitleASC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Title")},
                    { ApplicationResource.SortOptionTitleDESC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Title")},
                    { ApplicationResource.SortOptionAutorASC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Author")},
                    { ApplicationResource.SortOptionAutorDESC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Author")},
                    { ApplicationResource.SortOptionTimeASC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.ASCENDING).Sort(x, "Date")},
                    { ApplicationResource.SortOptionTimeDESC, (x) => SortAlgorithmFactory.GetAlgorithm(SortAlgorithm.DESCENDING).Sort(x, "Date")}
        };

        public List<string> SortsName
        {
            get => this.sorts.Keys.ToList();
        }

        public SearchViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        #region search

        // Get history search from SQLite Database
        private async Task initSearchHistory(string search = null, string sort = null, bool filter = true)
        {
            CookiesSave b = await App.Database.GetActiveUser();

            if ((this.SearchQuery == null && search != null) || (this.SearchQuery != null && search != null))
            {
                this.SearchQuery = search;
                this.page = 0;
            }
            else return;

            if (sort == null && this.SortName != null)
                sort = this.SortName;
            else if (sort != null)
                this.SortName = sort;

            // If history exist
            if (b.SearchValue != null)
            {
                var list = b.SearchValue.Split(',').ToList();
                if (!list.Contains(search))
                {
                    list.Insert(0, search);
                }
                this.SearchHistory = new List<string>();
                await RaisePropertyChanged(nameof(SearchHistory));
                this.SearchHistory = list;
                b.SearchValue = string.Join(",", list);
                await App.Database.SaveItemAsync(b);
            }
            // Create new one, add query and save
            else
            {
                this.SearchHistory = new List<string>();
                this.SearchHistory.Add(search);
                b.SearchValue = search;
                await App.Database.SaveItemAsync(b);
            }
        }

        // Convert FacetList to Archimed Json
        private string CheckFacetteSelected()
        {

            Dictionary<int, List<string>> choiceList = new Dictionary<int, List<string>>();
            List<string> value = new List<string>();
            SearchOptions optionsTempo = new SearchOptions();
            StringBuilder facetFilter = new StringBuilder("");
            if (this.FacetteList != null && this.SelectedItems != null)
            {
                foreach (var choices in this.SelectedItems)
                {
                    // If it's a title
                    if (choices.id == 0) continue;

                    // Sort and fill array with Facette of the same ID => if index doesn't exist, create it
                    if (!choiceList.TryGetValue((int)choices.id, out value))
                    {
                        choiceList.Add((int)choices.id, new List<string> { choices.value });
                    }
                    else
                    {
                        choiceList[(int)choices.id].Add(choices.value);
                    }
                }

                // Convertin to JSON Archimed type
                if (choiceList != null)
                {
                    facetFilter.Append("{");
                    int comma = 0;
                    foreach (KeyValuePair<int, List<string>> facet in choiceList)
                    {
                        facetFilter.Append("\"_");
                        facetFilter.Append(facet.Key + "\"");
                        facetFilter.Append(":" + "\"");
                        for (int j = 0; j < facet.Value.Count; j++)
                        {
                            facetFilter.Append(facet.Value[j]);
                            if (j + 1 < facet.Value.Count)
                                facetFilter.Append("||");
                            else { facetFilter.Append("\""); }
                        }
                        if (comma + 1 < choiceList.Count)
                            facetFilter.Append(",");
                        comma++;
                    }
                    facetFilter.Append("}");
                }
            }
            // Avoid empty facetteList {}
            if (facetFilter.Length < 3) return "";
            return facetFilter.ToString();
        }

        public async Task PerformSearch(string search = null, string sort = null, bool resetFacette = true)
        {
            this.IsBusy = true;
            if (sort == null)
            {
                this.SortName = null;
                this.SortOrder = 0;
            }
            // Showing user search is processing
            await RaiseAllPropertiesChanged();
            if (search == null) resetFacette = false;
            // Init searchHistory
            if (search == null && this.SearchQuery != null)
                search = this.SearchQuery;
            await initSearchHistory(search, sort, resetFacette);

            // Check if facettes are selected
            if (!resetFacette)
            {
                this.FacetFilter = CheckFacetteSelected();
            }
            else
            {
                this.SelectedItems = new List<FacetteValue>();
                this.FacetFilter = "";
            }
            

            // Search Query Json
            SearchOptions optionsTempo = new SearchOptions();
            optionsTempo.Query = new SearchOptionsDetails()
            {
                SortOrder = this.SortOrder,
                SortField = this.SortName,
                ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode,
                QueryString = search,
                FacetFilter = this.FacetFilter
            };

            // HTTP Request
            SearchResult result = await this.requestService.Search(optionsTempo);
            
            // Result Handler
            if (result != null && result.D != null && result.Success)
            {
                this.D = result.D;
                result.D.Results = await this.CheckAvCheckAvailability(result.D.Results, search, facetFilter);
                this.Results = result.D.Results;
                GetRedirectURL();
                this.ResultCountInt = this.D?.SearchInfo?.NbResults;
                this.ResultCount = this.ResultCountInt <= 1 ? (String.Format(ApplicationResource.SearchViewResultNull, this.D.SearchInfo.NbResults)) : (String.Format(ApplicationResource.SearchViewResultCount, this.D.SearchInfo.NbResults));
                this.FacetCollectionList = result.D.FacetCollectionList;

                // Avoid resetting facette on sortFilter change
                if (resetFacette && search != null)
                {
                    this.FacetteList = new ObservableCollection<FacetteGroup>();
                    this.Itemss = new SelectableObservableCollection<FacetteValue>();

                    int groupIndex = 0;
                    // Parsing FacetteList from ResultList
                    foreach (var tmp in this.FacetCollectionList)
                    {
                        FacetteGroup facetteGroupe = new FacetteGroup(tmp.FacetLabel);
                        foreach (var FacetListItems in tmp.FacetList)
                        {

                            // Adding Facette
                            var facettetempo = new FacetteValue
                            {
                                id = tmp.FacetId,
                                value = FacetListItems.Label,
                                displayValue = FacetListItems.DisplayLabel ?? FacetListItems.Label,
                                count = FacetListItems.Count.ToString(),
                                font = FontAttributes.None,
                                noTitle = true,
                                groupIndex = groupIndex
                            };
                            facetteGroupe.Add(facettetempo);
                        }

                        this.FacetteList.Add(facetteGroupe);
                        // Adding Title
                        var tmp3 = new FacetteValue
                        {
                            id = 0,
                            value = tmp.FacetLabel,
                            displayValue = tmp.FacetLabel,
                            font = FontAttributes.Bold,
                            noTitle = false
                        };
                        this.Itemss.Add(tmp3);
                        foreach (var tmp2 in tmp.FacetList)
                        {
                            // Adding Facette
                            tmp3 = new FacetteValue
                            {
                                id = tmp.FacetId,
                                value = tmp2.Label,
                                displayValue = tmp2.DisplayLabel ?? tmp2.Label,
                                count = tmp2.Count.ToString(),
                                font = FontAttributes.None,
                                noTitle = true
                            };
                            this.Itemss.Add(tmp3);
                        }
                        groupIndex += 1;
                    }
                }
                this.UpdateListContent();
            }
            else
            {
                this.DisplayAlert(ApplicationResource.Error, (result.Errors?[0]?.Msg) ?? ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                this.ResultCount = ApplicationResource.SearchViewResultNull;
            }
            await RaisePropertyChanged(nameof(this.Itemss));

            await RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }

        public async Task<Result[]> CheckAvCheckAvailability (Result[] results, string search, string facetFilter="") {

            List<RecordIdArray> RecordIdArray = new List<RecordIdArray>();
            CheckAvailabilityOptions optionsTempo = new CheckAvailabilityOptions();

            foreach (var result in results)
            {
                RecordIdArray.Add(new RecordIdArray(result.Resource.RscBase , result.Resource.RscId , result.Resource.Frmt));
            }

            optionsTempo.Query = new SearchOptionsDetails()
            {
                SortOrder = this.SortOrder,
                SortField = this.SortName,
                ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode,
                QueryString = search,
                FacetFilter = facetFilter
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

        private async Task GetRedirectURL()
        {
            await Task.Run(async () =>
            {
                foreach (var search in this.Results)
                {
                    if (search.FieldList.ThumbMedium != null && search.FieldList.ThumbMedium[0] != null)
                        search.FieldList.ThumbMedium[0] = new Uri(await this.requestService.GetRedirectURL(search.FieldList.ThumbMedium[0].ToString()));
                    else if (search.FieldList.ThumbSmall != null && search.FieldList.ThumbSmall[0] != null)
                        search.FieldList.ThumbSmall[0] = new Uri(await this.requestService.GetRedirectURL(search.FieldList.ThumbSmall[0].ToString()));
                    search.FieldList.HaveImage = true;
                    await this.RaisePropertyChanged(nameof(search.FieldList.HaveImage));
                }
            });
            this.ForceListUpdate();
            await this.RaiseAllPropertiesChanged();
        }

        #endregion search
        private void ForceListUpdate()
        {
            var tempo = this.Results;
            this.Results = new Result[0];
            this.Results = tempo;
        }
    }

}