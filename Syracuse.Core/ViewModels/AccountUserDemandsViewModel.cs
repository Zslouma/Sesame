using Syracuse.Mobitheque.Core.Services.Requests;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using Syracuse.Mobitheque.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class AccountUserDemandsViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService;

        private bool isBusy = true;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        private bool hasDemands { get; set; } = false;
        public bool HasDemands
        {
            get{
                return this.hasDemands;
            }
            set
            {
                this.hasDemands = value;
            }

        }

        private ObservableCollection<UserDemands> demands;
        public ObservableCollection<UserDemands> Demands
        {
            get => this.demands;
            set
            {
                SetProperty(ref this.demands, value);
                if (value.Count > 0)
                {
                    this.HasDemands = true;
                }
                else
                {
                    this.HasDemands = false;
                }
            }
        }

        public AccountUserDemandsViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }
        public override Task Initialize()
        {
            this.Update();
            return base.Initialize();
        }
        public override void ViewAppeared()
        {
            this.Update();
            base.ViewAppeared();
        }
        public async Task Update()
        {
            this.IsBusy = true;
            var result = await this.requestService.GetUserDemands();
            if (result.Success)
            {
                this.Demands = new ObservableCollection<UserDemands>();
                foreach (var item in result.D)
                {
                    this.Demands.Add(item);
                    this.HasDemands = true;
                }
            }
            await this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }

        

        public void HeaderTapped(UserDemands facetteGroupSelected)
        {
            this.IsBusy = true;
            var selectedIndex = this.Demands.IndexOf(facetteGroupSelected);
            if (selectedIndex < 0)
            {
                this.IsBusy = false;
                return;
            }
            if (!this.Demands[selectedIndex].Expanded)
            {
                foreach (var facette in this.Demands)
                {
                    facette.Expanded = false;
                }
            }
            this.Demands[selectedIndex].Expanded = !this.Demands[selectedIndex].Expanded;
            this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }

        public void OnClickItem(UserDemands parameter)
        {
            this.navigationService.Navigate<AccountUserDemandsChatViewModel, UserDemands>(parameter);
        }
    }
}
