using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class PinnedDocumentViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        public PinnedDocumentViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }

        public override async void Prepare()
        {
            await PerformBasket();
        }

        private async Task PerformBasket()
        {
            BasketOptions opt = new BasketOptions();
            await this.requestService.SearchUserBasket(opt);
        }
    }
}
