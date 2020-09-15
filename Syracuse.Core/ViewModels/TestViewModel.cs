using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Core.ViewModels
{
    public class TestViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService navigationService;

        private MvxAsyncCommand searchButtonPressedCommand;
        public MvxAsyncCommand SearchButtonPressedCommand => this.searchButtonPressedCommand ?? (this.searchButtonPressedCommand =
                    new MvxAsyncCommand(async () => await this.navigationService.Navigate<SearchViewModel>()));

        private SearchViewModel searchViewModel;
        public SearchViewModel SearchViewModel
        {
            get => this.searchViewModel;
            set => SetProperty(ref this.searchViewModel, value);
        }

        public TestViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
///            this.SearchViewModel = new SearchViewModel(navigationService, requestService);
        }

    }
}
