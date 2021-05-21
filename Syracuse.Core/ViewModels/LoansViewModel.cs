using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class LoansViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private bool notCurrentLoan = false;
        public bool NotCurrentLoan
        {
            get => this.notCurrentLoan;
            set
            {
                this.ReversNotCurrentLoan = !value;
                SetProperty(ref this.notCurrentLoan, value);
            }
        }

        private bool reversNotCurrentLoan = true;
        public bool ReversNotCurrentLoan
        {
            get => this.reversNotCurrentLoan;
            set
            {
                SetProperty(ref this.reversNotCurrentLoan, value);
            }
        }

        private Loans[] results;
        public Loans[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
            }
        }

        private bool isBusy = true;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        private MvxAsyncCommand<string> searchCommand;
        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ??
            (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<Loans> renewLoanCommand;
        public MvxAsyncCommand<Loans> RenewLoanCommand => this.renewLoanCommand ??
            (this.renewLoanCommand = new MvxAsyncCommand<Loans>((Id) => this.RenewLoan(Id)));

        private async Task RenewLoan(Loans loans)
        {
            bool answer = await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.RenewChoice, loans.Title), ApplicationResource.Yes, ApplicationResource.No);
            if (answer)
            {
                if (!loans.CanRenew)
                {
                    this.DisplayAlert(ApplicationResource.FailExtendLoan, loans.CannotRenewReason, ApplicationResource.ButtonValidation);
                    return;
                }
                this.IsBusy = true;
                LoansOption[] loanOptions = { new LoansOption { HoldingId = loans.HoldingId, Id = loans.Id, RecordId = loans.RecordId } };

                LoanOptions opt = new LoanOptions()
                {
                    serviceCode = "SYRACUSE",
                    loans = loanOptions,
                };

                RenewLoanResult res = await this.requestService.RenewLoans(opt);
                if (res == null)
                {
                    this.DisplayAlert(ApplicationResource.Error, ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                }
                else if (!res.Success)
                {
                    this.DisplayAlert(ApplicationResource.Error, res.Errors[0].Msg, ApplicationResource.ButtonValidation);
                }
                else
                {
                    if (res.D.SuccessCount > 0)
                    {
                        this.DisplayAlert(ApplicationResource.Success, String.Format(ApplicationResource.SuccessExtendLoan, loans.Title), ApplicationResource.ButtonValidation);
                    }
                    else
                    {
                        this.DisplayAlert(ApplicationResource.Error, res.D.Errors[0].Value, ApplicationResource.ButtonValidation);
                    }
                }
                await this.refreshLoans();
            }


        }

        public LoansViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
        }


        public async override void Prepare()
        {
            await refreshLoans();
        }

        private async Task refreshLoans()
        {
            this.IsBusy = true;
            LoansResult loans = await this.requestService.GetLoans();
            if (!loans.Success)
            {
                this.NotCurrentLoan = true;
                this.DisplayAlert(ApplicationResource.Error, loans.Errors[0].Msg, ApplicationResource.ButtonValidation);
                this.IsBusy = false;
                return;
            }
            this.Results = loans.D.Loans;
            List<Loans> tmp = new List<Loans>(this.Results);
            tmp.Sort((x, y) => DateTimeOffset.Compare(x.WhenBack, y.WhenBack));
            this.Results = tmp.ToArray();
            if (this.results.Length == 0)
            {
                this.NotCurrentLoan = true;
            }
            else
            {
                this.NotCurrentLoan = false;
            }
            await this.GetRedirectURL();
            await this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }
        private async Task GetRedirectURL()
        {
            foreach (var Loans in this.Results)
            {
                if (Loans.DefaultThumbnailUrl != null)
                {
                    Loans.ThumbnailUrl = await this.requestService.GetRedirectURL(Loans.ThumbnailUrl, Loans.DefaultThumbnailUrl);
                }
                else
                {
                    Loans.ThumbnailUrl = await this.requestService.GetRedirectURL(Loans.ThumbnailUrl);
                }
            }
            await this.RaiseAllPropertiesChanged();
        }
        private async Task PerformSearch(string search)
        {
            var options = new SearchOptionsDetails()
            {
                QueryString = search
            };
            SearchOptions opt = new SearchOptions() { Query = options };
            if (App.AppState.NetworkConnection)
            {
                await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
            }
            else
            {
                this.DisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
            }

        }
    }
}
