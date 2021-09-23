using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class AccountUserDemandsChatViewModel : BaseViewModel<UserDemands, UserDemands>
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

        private UserDemands demands;
        public UserDemands Demands
        {
            get => this.demands;
            set
            {
                SetProperty(ref this.demands, value);
            }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get => this.messages;
            set
            {
                SetProperty(ref this.messages, value);
            }
        }

        public AccountUserDemandsChatViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public override void Prepare(UserDemands parameter)
        {
            this.IsBusy = true;
            this.Demands = parameter;
            this.Messages = new ObservableCollection<Message>();
            foreach (var item in Demands.messages)
            {
                this.Messages.Add(item);
            }
            this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }
    }
}
