using System;
using System.Threading.Tasks;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class AccountInfoViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;

        private readonly IMvxNavigationService navigationService;

        private SummaryAccount accountSummary;
        public SummaryAccount SummaryAccount
        {
            get => this.accountSummary;
            set
            {
                SetProperty(ref this.accountSummary, value);
            }          
        }

        private String totalBorrowedDocuments;
        public String TotalBorrowedDocuments
        {
            get => this.totalBorrowedDocuments;
            set
            {
                SetProperty(ref this.totalBorrowedDocuments, value);
            }
        }

        private String lateBorrowedDocuments;
        public String LateBorrowedDocuments
        {
            get => this.lateBorrowedDocuments;
            set
            {
                SetProperty(ref this.lateBorrowedDocuments, value);
            }
        }

        private String[] inTimeBorrowedDocuments;
        public String[] InTimeBorrowedDocuments
        {
            get => this.inTimeBorrowedDocuments;
            set
            {
                SetProperty(ref this.inTimeBorrowedDocuments, value);
            }
        }

        private String totalBookingDocuments;
        public String TotalBookingDocuments
        {
            get => this.totalBookingDocuments;
            set
            {
                SetProperty(ref this.totalBookingDocuments, value);
            }
        }

        private String availableBookingDocuments;
        public String AvailableBookingDocuments
        {
            get => this.availableBookingDocuments;
            set
            {
                SetProperty(ref this.availableBookingDocuments, value);
            }
        }

        public AccountInfoViewModel(IMvxNavigationService navigationService, IRequestService requestService)
        {
            this.navigationService = navigationService;
            this.requestService = requestService;
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
        private MvxAsyncCommand tapBorrowedCommand;
        public MvxAsyncCommand TapBorrowedCommand => (this.tapBorrowedCommand = new MvxAsyncCommand(NavigateToLoans));

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task NavigateToLoans()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                await this.navigationService.Navigate<LoansViewModel>();
            }
        private MvxAsyncCommand tapBookingCommand;
        public MvxAsyncCommand TapBookingCommand => (this.tapBorrowedCommand = new MvxAsyncCommand(NavigateToBooking));

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task NavigateToBooking()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            await this.navigationService.Navigate<BookingViewModel>();
        }

        public async override Task Initialize()
        {

            await base.Initialize();
            this.IsBusy = true;
            if (SummaryAccount == null)
            {
                var response = await this.requestService.GetSummary();
                this.SummaryAccount = response.D.AccountSummary;
            }
            if (SummaryAccount != null)
            {
                this.TotalBorrowedDocuments = String.Format(ApplicationResource.AccountInfoCountOfLoans, SummaryAccount.LoansTotalCount);
                this.LateBorrowedDocuments =
                    (this.SummaryAccount.LoansLateCount > 1) ? String.Format(ApplicationResource.AccountInfoCountLateLoans, SummaryAccount.LoansLateCount)
                    : String.Format(ApplicationResource.AccountInfoCountLateLoan, SummaryAccount.LoansLateCount);
                if (SummaryAccount.LoansNotLateCount > 0)
                {
                    var documents = new List<string>();
                    var response = await this.requestService.GetLoans();
                    var loans = new List<Loans>(response.D.Loans);
                    DateTimeOffset date;
                    while (loans.Count > 0)
                    {
                        date = loans[0].WhenBack;
                        int total = 0;
                        foreach (Loans loan in loans)
                        {
                            if (date == loan.WhenBack)
                            {
                                total += 1;
                            }
                        }
                        loans.RemoveAll(item => item.WhenBack == date);
                        String tmp = (total > 1) ?
                        String.Format(ApplicationResource.AccountInfoReturnDateLoans, total , date.Date.ToShortDateString())
                        : String.Format(ApplicationResource.AccountInfoReturnDateLoan, total, date.Date.ToShortDateString());
                        documents.Add(tmp);
                    }
                    InTimeBorrowedDocuments = documents.ToArray();
                    Console.WriteLine(InTimeBorrowedDocuments);
                }
                this.TotalBookingDocuments = String.Format(ApplicationResource.AccountInfoCountOfBookings, SummaryAccount.BookingsTotalCount); 
                this.AvailableBookingDocuments =
                    (this.SummaryAccount.BookingsAvailableCount != 1) ? String.Format(ApplicationResource.AccountInfoCountAvailableBookings, SummaryAccount.BookingsAvailableCount)
                    : String.Format(ApplicationResource.AccountInfoCountAvailableBooking, SummaryAccount.BookingsAvailableCount);
            }
            this.IsBusy = false;
        }
    }
}
