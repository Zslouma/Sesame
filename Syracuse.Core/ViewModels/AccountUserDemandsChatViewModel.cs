using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class AccountUserDemandsChatViewModel : BaseViewModel<UserDemands, UserDemands>
    {

        private readonly IRequestService requestService;

        public IRequestService GetrequestService
        {
            get
            {
                return requestService;

            }
        }

        private readonly IMvxNavigationService navigationService;

        private bool isBusy = true;

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        private UserDemands demands;
        public UserDemands Demands
        {
            get => this.demands;
            set
            {
                SetProperty(ref this.demands, value);
                RaisePropertyChanged(nameof(Demands));
            }
        }

        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get => this.messages;
            set
            {
                SetProperty(ref this.messages, value);
                RaisePropertyChanged(nameof(Messages));
            }
        }

        public bool CreatedByProfessional
        {
            get {
                if (Messages.Count > 0)
                {
                    return Messages[Messages.Count - 1].createdByProfessional;
                }
                else
                {
                    return false;
                }
                
            }

        }
        public bool StatusNotClosed
        {
            get
            {
                Console.WriteLine("StatusNotClosed: "+ (!Demands.status.Equals(UserDemandStatus.Closed) && !Demands.status.Equals(UserDemandStatus.Archived) && Messages.Count > 0 && !Messages[Messages.Count - 1].validated));
                return !Demands.status.Equals(UserDemandStatus.Closed) && !Demands.status.Equals(UserDemandStatus.Archived) && Messages.Count > 0 && !Messages[Messages.Count - 1].validated;
            }

        }



        public AccountUserDemandsChatViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public async override void Prepare(UserDemands parameter)
        {
            this.IsBusy = true;
            await this.RaiseAllPropertiesChanged();
            this.Demands = parameter;
            this.Messages = new ObservableCollection<Message>();
            foreach (var item in Demands.messages)
            {
                this.Messages.Add(item);
            }
            this.IsBusy = false;
            await this.RaiseAllPropertiesChanged();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            this.Update();
        }
        public async void Update()
        {
            await this.RaiseAllPropertiesChanged();
            this.IsBusy = true;
            var result = await this.requestService.GetUserDemands();
            bool HasDemands = false;
            ObservableCollection<UserDemands> DemandsTempo = new ObservableCollection<UserDemands>(); ;
            if (result.Success)
            {
                foreach (var item in result.D)
                {
                    DemandsTempo.Add(item);
                    HasDemands = true;
                }
            }
            if (HasDemands)
            {
                foreach (var demands in DemandsTempo)
                {
                    if (demands.id == this.Demands.id)
                    {
                        this.Demands = demands;
                        break;
                    }
                }
            }
            this.Messages = new ObservableCollection<Message>();
            foreach (var item in Demands.messages)
            {
                this.Messages.Add(item);
            }
            await this.RaiseAllPropertiesChanged();
            this.IsBusy = false;

        }

        public async Task AnswerDemand()
        {
            DemandsOptions demandsOptions = new DemandsOptions(this.Demands.id,"je vous remercie");
            this.requestService.AnswerDemand(demandsOptions);
        }
    }
}
