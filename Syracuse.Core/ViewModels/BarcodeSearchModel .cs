using System;
using MvvmCross.Navigation;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class BarcodeSearchModel : BaseViewModel
    {

        public string Result
        {
            get => this._result;
            set
            {
                SetProperty(ref this._result, value);
                PerformSearch();
            }
        }
        private String _result;

        private IMvxNavigationService navigationService {get; set;}
        public BarcodeSearchModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void PerformSearch()
        {
            this.navigationService.Navigate<MasterDetailViewModel, string, string>(Result);

        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
        }
    }
}
