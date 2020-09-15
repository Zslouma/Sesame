using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class NetworkErrorViewModel : BaseViewModel
    {

        private IRequestService requestService { get; set; }
        private readonly IMvxNavigationService navigationService;

        public NetworkErrorViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }
    }
}
