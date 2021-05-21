using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MvvmCross.Binding.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Recherche")]
    public partial class SearchView : MvxContentPage<SearchViewModel>
    {
        ObservableCollection<string> SearchList = new ObservableCollection<string>();
        private bool enableMultiSelect;

        public SearchView()
        {
            InitializeComponent();
            // Adding gesture detector
            SearchBar.Focused               += SearchBar_OnFocus;
            SearchBar.Unfocused             += SearchBar_OnUnfocus;
            SearchBar.SearchButtonPressed   += SearchBar_Onsearch;
            this.FacetteItemList.ItemTapped += FacetteItemList_ItemTapped;

            // Set bools for facetteList
            FacetteItemList.IsVisible   = false;
            resultsList.IsVisible       = true;
            resultCount.IsVisible       = true;
            SortPicker.IsVisible        = false;
            enableMultiSelect           = true;
            SearchButton.IsVisible      = false;
            DeleteButton.IsVisible      = false;
            
        }
        protected override void OnBindingContextChanged()
        {
            (this.DataContext as SearchViewModel).OnDisplayAlert += SearchViewModel_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        // Enable MultiSelect for MultiPicker Component
        public bool EnableMultiSelect
        {
            get { return enableMultiSelect; }
            set
            {
                enableMultiSelect = value;
                OnPropertyChanged();
            }
        }

        // Set height for the searchHistoryList based on number of items (ex: 5 cells = 90px x 5)
        private void setHeight(int height = -1)
        {
            if (height == 0)
            {
                searchHistoryList.HeightRequest = 0;
                return;
            }
            int i = searchHistoryList.ItemsSource.Count();
            if (i > 10)
                i = 10;
            int heightRowList = 90;
            i = (i * heightRowList);
            searchHistoryList.HeightRequest = i;
        }

        // Clear searchHistory and reset it
        protected override void OnAppearing()
        {
            this.SearchList.Clear();
            var list = this.ViewModel.SearchHistory;
            if (list != null)
            {
                foreach (var item in list)
                    this.SearchList.Add(item);
                setHeight();
                searchHistoryList.ItemsSource = this.SearchList;
                this.resultsList.ItemTapped += ResultsList_ItemTapped;
                searchHistoryList.IsVisible = false;
            }
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.resultsList.ItemTapped -= ResultsList_ItemTapped;
            searchHistoryList.IsVisible = false;

            base.OnDisappearing();
        }
        private void UpdateItemList()
        {
            FacetteItemList.ItemsSource = this.ViewModel.ExpandedFacetteList;
        }
        
        // Set tapped item to selected
        private async void FacetteItemList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await this.ViewModel.OnCheckSelection(e.Item as FacetteValue);
            if (this.ViewModel.SelectedItems.Count() > 0)
            {
                DeleteButton.IsVisible = true;
            }
            else
            {
                DeleteButton.IsVisible = false;
            }
            if (!this.ViewModel.Equals(this.ViewModel.SelectedItems, this.ViewModel.OldSelectedItems))
            {
                SearchButton.IsVisible = true;

            }
            else
            {
                SearchButton.IsVisible = false;
            }
                       
            this.UpdateItemList();
        }


        // Search for Detailed view of the Item
        private void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            setHeight();
            var tmp     = new SearchResult();
            tmp.D       = new D();

            Result[]                tmpResult                 = { new Result() };
            FacetCollectionList[] tmpfacetCollectionList = { new FacetCollectionList() };

            tmp.D.Results = tmpResult;
            tmp.D.FacetCollectionList = tmpfacetCollectionList;

            var iiitem = e.Item as Result;
            var iiitemm = e.Item as FacetCollectionList;

            tmp.D.Results[0]                = iiitem;
            tmp.D.FacetCollectionList[0]    = iiitemm;
            (this.DataContext as SearchViewModel).OpenDetailsCommand.ExecuteAsync(tmp);
        }

        // Trigger every input on the searchBar
        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            searchHistoryList.IsVisible = true;
            searchHistoryList.BeginRefresh();

            try {
                this.SearchList.Clear();
                var list = this.ViewModel.SearchHistory;
                if (list != null)
                {
                    foreach (var item in list)
                        this.SearchList.Add(item);
                    setHeight();
                    searchHistoryList.ItemsSource = this.SearchList;
                }

                searchHistoryList.EndRefresh();
                setHeight();
                if (e.NewTextValue == null || e.NewTextValue.Equals(string.Empty))
                {
                    searchHistoryList.IsVisible = false;
                    return;
                }
                return;
            }
            catch (Exception)
            {
                searchHistoryList.IsVisible = false;
                setHeight();
            }
            searchHistoryList.EndRefresh();
        }

        // Perform search
        async private void ListView_OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            String listsd = e.Item as string;
            SearchBar.Text = listsd;
            resultsList.IsVisible = true;
            resultCount.IsVisible = true;

            FacetteItemList.IsVisible = false;
            FacetteButton.FontAttributes = FontAttributes.None;
            FacetteButtonUnderline.IsVisible = false;

            SortPicker.IsVisible = false;
            SortPicker.FontAttributes = FontAttributes.None;
            SortButtonUnderline.IsVisible = false;

            SearchButton.IsVisible = false;
            DeleteButton.IsVisible = false;

            DeleteButtonUnderline.IsVisible = false;
            SearchButtonUnderline.IsVisible = false;

            enableMultiSelect = true;
            setHeight();
            SearchBar.Unfocus();
            ((ListView)sender).SelectedItem = null;
            await this.ViewModel.PerformSearch(listsd, null, true);
            if (this.ViewModel.Results.Count() > 0)
            {
                resultsList.ScrollTo(this.ViewModel.Results[0], 0, true);
            }
            SearchBar.IsEnabled = true;
            this.UpdateItemList();
        }

        private void SearchBar_OnFocus(Object sender, FocusEventArgs args)
        {
            searchHistoryList.IsVisible = true;
            setHeight();
        }
        private void SearchBar_OnUnfocus(Object sender, FocusEventArgs args)
        {
            searchHistoryList.IsVisible = false;
        }
        private void SearchBar_Onsearch(Object sender, EventArgs e)
        {
            resultsList.IsVisible = true;
            resultCount.IsVisible = true;

            FacetteItemList.IsVisible = false;
            FacetteButton.FontAttributes = FontAttributes.None;
            FacetteButtonUnderline.IsVisible = false;

            SortPicker.IsVisible = false;
            SortPicker.FontAttributes = FontAttributes.None;
            SortButtonUnderline.IsVisible = false;
            
            SearchButton.IsVisible = false;
            DeleteButton.IsVisible = false;
            
            DeleteButtonUnderline.IsVisible = false;
            SearchButtonUnderline.IsVisible = false;

            enableMultiSelect = true;
            if (this.ViewModel.Results.Count() > 0)
            {
                resultsList.ScrollTo(this.ViewModel.Results[0], 0, true);
            }
            this.UpdateItemList();

        }

        private void HeaderButton_Clicked(object sender, EventArgs e)
        {
            
            FacetteGroup facetteGroupSelected = (FacetteGroup)((Button)sender).CommandParameter;
            this.ViewModel.HeaderTapped(facetteGroupSelected);
            this.UpdateItemList();
        }

        private void HeaderImageButton_Clicked(object sender, EventArgs e)
        {

            FacetteGroup facetteGroupSelected = (FacetteGroup)((ImageButton)sender).CommandParameter;
            this.ViewModel.HeaderTapped(facetteGroupSelected);
            this.UpdateItemList();
        }

        private void FacetteButton_Clicked(object sender, EventArgs e)
        {
            SortButton.FontAttributes = FontAttributes.None;
            SortButtonUnderline.IsVisible = false;
            SortPicker.IsVisible = false;
            FacetteItemList.IsVisible   = !FacetteItemList.IsVisible;
            resultsList.IsVisible = !resultsList.IsVisible;
            resultCount.IsVisible = !resultCount.IsVisible;
            if (FacetteItemList.IsVisible)
            {
                
                if (!this.ViewModel.Equals(this.ViewModel.SelectedItems, this.ViewModel.OldSelectedItems))
                {
                    SearchButton.IsVisible = true;
                }
                else
                {
                    SearchButton.IsVisible = false;
                }

                if (this.ViewModel.SelectedItems.Count() > 0)
                {
                    DeleteButton.IsVisible = true;
                }
                else
                {
                    DeleteButton.IsVisible = false;
                }
            }
            else
            {
                SearchButton.IsVisible = false;
                DeleteButton.IsVisible = false;
            }
            
            if (FacetteButton.FontAttributes == FontAttributes.Bold)
            {
                FacetteButton.FontAttributes = FontAttributes.None;
                FacetteButtonUnderline.IsVisible = false;
            }
            else
            {
                FacetteButton.FontAttributes = FontAttributes.Bold;
                FacetteButtonUnderline.IsVisible = true;
            }
            SearchButton.FontAttributes = FontAttributes.None;
            SearchButtonUnderline.IsVisible = false;
            this.UpdateItemList();
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            FacetteItemList.IsVisible   = false;
            resultsList.IsVisible       = true;
            resultCount.IsVisible       = true;
            DeleteButton.IsVisible      = false;
            SearchButton.IsVisible      = false;
            Task.Run( async () => await this.ViewModel.PerformSearch());
            FacetteButton.FontAttributes = FontAttributes.None;
            FacetteButtonUnderline.IsVisible = false;
            if (SearchButton.FontAttributes == FontAttributes.Bold)
            {
                SearchButton.FontAttributes = FontAttributes.None;
                SearchButtonUnderline.IsVisible = false;
            }
            else
            {
                SearchButtonUnderline.IsVisible = true;
                SearchButton.FontAttributes = FontAttributes.Bold;
            }
            this.ViewModel.OldSelectedItems = new List<FacetteValue>(this.ViewModel.SelectedItems);
            this.UpdateItemList();
        }

        private void SortButton_Clicked(object sender, EventArgs e)
        {

            FacetteItemList.IsVisible = false;
            resultsList.IsVisible = true;
            resultCount.IsVisible = true;
            FacetteButton.FontAttributes = FontAttributes.None;
            FacetteButtonUnderline.IsVisible = false;
            SearchButton.IsVisible = false;
            DeleteButton.IsVisible = false;
            SortPicker.IsVisible = !SortPicker.IsVisible;
            SearchButton.FontAttributes = FontAttributes.None;
            SearchButtonUnderline.IsVisible = false;
            if (SortButton.FontAttributes == FontAttributes.Bold)
            {
                SortButton.FontAttributes = FontAttributes.None;
                SortButtonUnderline.IsVisible = false;
            }
            else
            {
                SortButton.FontAttributes = FontAttributes.Bold;
                SortButtonUnderline.IsVisible = true;
            }
            if (SortPicker.IsVisible)
            {
                SortPicker.Focus();
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if(this.ViewModel.SelectedItems.Count() > 0)
            {
                this.ViewModel.RemoveSelectedCommand();
                this.UpdateItemList();
                SearchButton.IsVisible = true;
            }

        }

        private void SearchViewModel_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
    
}