using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{

    public class SearchDetailsViewModel : BaseViewModel<SearchDetailsParameters, SearchResult>
    {
        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService ;

        private SearchLibraryResult library;

        public bool success { get; set; } = false;

        private string authorDate;

        private string star;
        public string Star
        {
            get => this.star;
            set { SetProperty(ref this.star, value); }
        }

        public string AuthorDate
        {
            get => this.authorDate;
            set { SetProperty(ref this.authorDate, value); }
        }

        private string desc;
        public string Desc
        {
            get => this.desc;
            set { SetProperty(ref this.desc, value); }
        }
        private ObservableCollection<Result> itemsSource;
        public ObservableCollection<Result> ItemsSource
        {
            get => this.itemsSource;
            set { SetProperty(ref this.itemsSource, value); }
        }
        private Result currentItem;
        public Result CurrentItem
        {
            get => this.currentItem;
            set { SetProperty(ref this.currentItem, value); }
        }
        private int position;
        public int Position
        {
            get => this.position;
            set{
                this.DisplayPosition = (value + 1).ToString() + " / " + this.NbrResults;
                SetProperty(ref this.position, value); 
            }
        }
        private string displayPosition = "";
        public string DisplayPosition
        {
            get => this.displayPosition;
            set { SetProperty(ref this.displayPosition, value); }
        }

        private string nbrResults;
        public string NbrResults
        {
            get => this.nbrResults;
            set { SetProperty(ref this.nbrResults, value); }
        }

        private IList<Result> results;
        public IList<Result> Results
        {
            get => this.results;
            set { SetProperty(ref this.results, value); }
        }

        private string query;
        public string Query
        {
            get => this.query;
            set { SetProperty(ref this.query, value); }
        }

        private SearchOptions searchOptions;
        public SearchOptions SearchOptions
        {
            get => this.searchOptions;
            set { SetProperty(ref this.searchOptions, value); }
        }

        private bool inLoadMore = false;
        public bool InLoadMore
        {
            get => this.inLoadMore;
            set { SetProperty(ref this.inLoadMore, value); }
        }

        public SearchLibraryResult Library
        {
            get => this.library;
            set { SetProperty(ref this.library, value); }
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

        private bool isPositionVisible = true;
        public bool IsPositionVisible
        {
            get => this.isPositionVisible;
            set
            {
                SetProperty(ref this.isPositionVisible, value);
                //if (value)
                //{
                //    this.TimeVisibility(this.Position);
                //}
            }
        }

        private bool reversIsKm = false;
        public bool ReversIsKm
        {
            get => this.reversIsKm;
            set { SetProperty(ref this.reversIsKm, value); }
        }



        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
        (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));



        public SearchDetailsViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        private string setStar(long star)
        {
            if (star == 1)      return "https://upload.wikimedia.org/wikipedia/commons/d/dd/Star_rating_1_of_5.png";
            else if (star == 2) return "https://upload.wikimedia.org/wikipedia/commons/9/95/Star_rating_2_of_5.png";
            else if (star == 3) return "https://upload.wikimedia.org/wikipedia/commons/2/2f/Star_rating_3_of_5.png";
            else if (star == 4) return "https://upload.wikimedia.org/wikipedia/commons/f/fa/Star_rating_4_of_5.png";
            else if (star == 5) return "https://upload.wikimedia.org/wikipedia/commons/1/17/Star_rating_5_of_5.png";
            return null;
        }
        async public override void Prepare(SearchDetailsParameters parameter)
        {
            this.IsBusy = true;
            await this.CanHolding();
            this.Results = new List<Result>();
            int finalPosition = 0;
            this.SearchOptions = parameter.searchOptions;
            this.NbrResults = parameter.nbrResults;
            var parameterTempo = parameter.parameter;
            for (int i = 0; i < 2; i++)
            {
                var positionTempo = 0;
                foreach (var resultTempo in parameterTempo[i].D.Results)
                {
                    if (resultTempo.Resource.Desc != null)
                        resultTempo.DisplayValues.Desc = resultTempo.Resource.Desc;
                    resultTempo.DisplayValues.Star = this.setStar(resultTempo.Resource.AvNt);
                    if (resultTempo.DisplayValues.Star == null)
                    {
                        resultTempo.DisplayValues.DisplayStar = false;
                    }
                    else { 
                        resultTempo.DisplayValues.DisplayStar = true; 
                    }
                    //resultTempo.DisplayValues.SeekForHoldings = resultTempo.SeekForHoldings && this.ReversIsKm;
                    await PerformSearch(resultTempo.FieldList.Identifier[0]);
                    this.BuildHoldingsStatements();
                    this.BuildHoldings();
                    resultTempo.DisplayValues.Library = this.Library;
                    //resultTempo.DisplayValues.Library.success = resultTempo.DisplayValues.Library.success && resultTempo.DisplayValues.SeekForHoldings;
                    if (i == 1)
                    {
                        if (resultTempo == parameterTempo[0].D.Results[0])
                        {
                            finalPosition = positionTempo;
                        }
                        this.Results.Add(resultTempo);
                    }
                    positionTempo += 1;
                }
            }
            this.ItemsSource = new ObservableCollection<Result>(this.Results);
            this.CurrentItem = this.Results.Skip(finalPosition).FirstOrDefault();
            this.Position = finalPosition;
            if (this.Position >= (this.ItemsSource.Count() - 5) && int.Parse(this.NbrResults) > this.ItemsSource.Count)
            {
                await LoadMore();
            }
            this.IsBusy = false;
            this.IsPositionVisible = true;
        }

        public void BuildHoldingsStatements()
        {
            if (this.Library.success && this.Library.Dataa.HoldingsStatements.Count > 0)
            {
                foreach (var item in this.Library.Dataa.HoldingsStatements)
                {
                    Dictionary<string, bool> DisplayHoldingsStatements = new Dictionary<string, bool>();
                    List<string> strValues = new List<string>();
                    foreach (var value in this.Library.Dataa.DisplayHoldingsStatements)
                    {
                        if (value.Value)
                        {
                            var itemObject = item.GetType().GetProperty(value.Key)?.GetValue(item, null);
                            string itemValue = itemObject != null ? itemObject.ToString() : "";
                            if (String.IsNullOrEmpty(itemValue))
                            {
                                DisplayHoldingsStatements.Add(value.Key, false);
                                continue;
                            }
                            else
                            {
                                if (value.Key != "Site")
                                {
                                    strValues.Add(itemValue);
                                }
                            }
                        }
                        DisplayHoldingsStatements.Add(value.Key, value.Value);

                    }
                    if (strValues.Count > 0)
                    {
                        item.DisplayValue = String.Join(" | ", strValues);
                    }
                    item.DisplayHoldingsStatements = DisplayHoldingsStatements;
                }
            }
        }
        public void BuildHoldings()
        {
            if (this.Library.success && this.Library.Dataa.Holdings.Count > 0)
            {
                foreach (var item in this.Library.Dataa.Holdings)
                {
                    Dictionary<string, bool> DisplayHoldings = new Dictionary<string, bool>();
                    List<string> strValues = new List<string>();
                    foreach (var value in this.Library.Dataa.DisplayHoldings)
                    {
                        if (value.Value)
                        {
                            var itemObject = item.GetType().GetProperty(value.Key)?.GetValue(item, null);
                            string itemValue = itemObject != null? itemObject.ToString() : "";
                            if (String.IsNullOrEmpty(itemValue))
                            {
                                DisplayHoldings.Add(value.Key, false);
                                continue;
                            }
                            else
                            {
                                if (value.Key != "Site")
                                {
                                    strValues.Add(itemValue);
                                }
                            }
                        }
                        DisplayHoldings.Add(value.Key, value.Value);

                    }
                    if (strValues.Count > 0)
                    {
                        item.DisplayValue = String.Join(" | ", strValues);
                    }
                    if (item.IsHaveWhenBack)
                    {
                        item.DisponibilityText = String.Format(ApplicationResource.HoldingDisponibilityTextDateBack, item.WhenBack, item.Site);
                    }
                    else
                    {
                        item.DisponibilityText = String.Format(ApplicationResource.HoldingDisponibilityText, item.Site);
                    }
                    item.DisplayHoldings = DisplayHoldings;
                }
            }
        }

        public async Task CanHolding()
        {
            var user = await App.Database.GetActiveUser();
            this.ReversIsKm = !user.IsKm;
        }

        public async Task NavigationBack()
        {
            await navigationService.Close(this);
        }

        public async Task<Uri> RelativeUriToAbsolute(string uri)
        {
            var user = await App.Database.GetActiveUser();
            string url = user.DomainUrl + uri;
            Uri reslt = new Uri(url);
            return reslt;
        }

        public async Task<Uri> GetUrlTransfert(Uri uri)
        {
            UrlWithAuthenticationStatus status = await this.requestService.GetUrlWithAuthenticationTransfert(uri);
            if (status.Success)
            {
                return status.D;
            }
            else
            {
                return uri;
            }
            
        }

        private async Task FormateToCarrousel(Result[] results)
        {
            foreach (var resultTempo in results)
            {
                if (resultTempo.Resource.Desc != null)
                    resultTempo.DisplayValues.Desc = resultTempo.Resource.Desc;
                resultTempo.DisplayValues.Star = this.setStar(resultTempo.Resource.AvNt);
                if (resultTempo.DisplayValues.Star == null)
                {
                    resultTempo.DisplayValues.DisplayStar = false;
                }
                else
                {
                    resultTempo.DisplayValues.DisplayStar = true;
                }
                resultTempo.DisplayValues.SeekForHoldings = resultTempo.SeekForHoldings && this.ReversIsKm;
                await PerformSearch(resultTempo.FieldList.Identifier[0]);
                this.BuildHoldingsStatements();
                this.BuildHoldings();
                resultTempo.DisplayValues.Library = this.Library;
                resultTempo.DisplayValues.Library.success = resultTempo.DisplayValues.Library.success && resultTempo.DisplayValues.SeekForHoldings;
                this.ItemsSource.Add(resultTempo);
            }
        }

        public async Task LoadMore()
        {
            this.InLoadMore = true;
            this.SearchOptions.Query.Page += 1;
            SearchResult search = await this.requestService.Search(this.SearchOptions);
            if (search != null && !search.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, search.Errors?[0]?.Msg, ApplicationResource.ButtonValidation);
                return;
            }
            else
            {
                await this.FormateToCarrousel(search?.D?.Results);
            }

            this.InLoadMore = false;
        }

        private async Task PerformSearch(string search = null)
        {
            if (search == null)
            {
                search = this.Query;
            } else
            {
                this.Query = search;
            }
            var options = new SearchLibraryOptionsDetails()
            {
                RscId = search
            };
            var res = await this.requestService.SearchLibrary(new SearchLibraryOptions() { Record = options });

            this.Library = res;

        }

        public async Task Holding(string Holdingid, string RecordId, string BaseName)
        {
            var options = new HoldingItem()
            {
                HoldingId = Holdingid,
                RecordId = RecordId,
                BaseName = BaseName
                
            };

            PlaceReservationResult res = await this.requestService.PlaceReservation(new PlaceReservationOptions() { HoldingItem = options });
            if (res == null)
            {
                this.DisplayAlert(ApplicationResource.Error, ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
            }
            else if (!res.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, ApplicationResource.FailBookingRequest, ApplicationResource.ButtonValidation);
            }
            else
            {
                await PerformSearch(null);
                this.DisplayAlert(ApplicationResource.Success, ApplicationResource.SuccessBookingRequest, ApplicationResource.ButtonValidation);
            }
        }    

    }
}
