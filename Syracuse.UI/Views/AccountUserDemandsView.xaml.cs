using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "")]
    public partial class AccountUserDemandsView : MvxContentPage<AccountUserDemandsViewModel>
    {



        public AccountUserDemandsView()
        {
            InitializeComponent();
        }
        private void HeaderButton_Clicked(object sender, EventArgs e)
        {
            UserDemands facetteGroupSelected = (UserDemands)((Button)sender).CommandParameter;
            this.ViewModel.HeaderTapped(facetteGroupSelected);
        }

        private void HeaderImageButton_Clicked(object sender, EventArgs e)
        {
            UserDemands facetteGroupSelected = (UserDemands)((ImageButton)sender).CommandParameter;
            this.ViewModel.HeaderTapped(facetteGroupSelected);
        }

        private async void DemandList_ItemTapped(object sender, SelectionChangedEventArgs e)
        {
            this.ViewModel.IsBusy = true;
            await this.ViewModel.RaisePropertyChanged(nameof(this.ViewModel.IsBusy));
            if (e.CurrentSelection.Count > 0)
            {
                var item = e.CurrentSelection[0] as UserDemands;
                this.ViewModel.OnClickItem(item);
            }
            else
            {
                await this.DisplayAlert("Erreur", "Une erreur est survenue", "Ok");
            }
            this.ViewModel.IsBusy = false;
            await this.ViewModel.RaisePropertyChanged(nameof(this.ViewModel.IsBusy));
        }
    }
}