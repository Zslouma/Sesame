using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;
using System.Collections.ObjectModel;


namespace Syracuse.Mobitheque.UI.Views.Templates
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Accueil")]
    public partial class SearchBarTemplate : MvxContentView<SearchBarViewModel>
    {
        ObservableCollection<string> SearchList = new ObservableCollection<string>();
        public SearchBarViewModel searchBarModel;

        public SearchBarTemplate()
        {
            InitializeComponent();
            this.searchBarModel = new SearchBarViewModel(null, null);
            this.searchBarModel.Prepare();

            var list = this.searchBarModel.SearchHistory;
            if (list == null) return;
            foreach (var item in list)
                this.SearchList.Add(item);
        }

        private void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SearchBar.Unfocus();
            var tmp = new SearchResult();
            tmp.D = new D();

            Result[] tmpResult = { new Result() };
            FacetCollectionList[] tmpfacetCollectionList = { new FacetCollectionList() };

            tmp.D.Results = tmpResult;
            tmp.D.FacetCollectionList = tmpfacetCollectionList;

            var iiitem = e.Item as Result;
            var iiitemm = e.Item as FacetCollectionList;

            tmp.D.Results[0] = iiitem;
            tmp.D.FacetCollectionList[0] = iiitemm;
        }

        private void SearchBar_onEnter(object sender, TextChangedEventArgs e)
        {
            this.SearchList.Clear();
            var list = this.ViewModel.SearchHistory;
            foreach (var item in list)
                this.SearchList.Add(item);
            SearchBar.Unfocus();
            SearchBar.IsEnabled = true;

        }


        async private void ListView_OnItemTapped(Object sender, ItemTappedEventArgs e)
        {

            String listsd = e.Item as string;
            SearchBar.Text = listsd;
            SearchBar.Unfocus();
            ((ListView)sender).SelectedItem = null;
            await this.ViewModel.PerformSearch(listsd);
            SearchBar.IsEnabled = true;

        }
    }

}
