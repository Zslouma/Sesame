using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class BookingViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;
        private Booking[] results;
        public Booking[] Results
        {
            get => this.results;
            set
            {
                SetProperty(ref this.results, value);
            }
        }
        private bool notCurrentBooking = false;
        public bool NotCurrentBooking
        {
            get => this.notCurrentBooking;
            set
            {
                this.ReversNotCurrentBooking = !value;
                SetProperty(ref this.notCurrentBooking, value);
            }
        }

        private bool reversNotCurrentBooking = true;
        public bool ReversNotCurrentBooking
        {
            get => this.reversNotCurrentBooking;
            set
            {
                SetProperty(ref this.reversNotCurrentBooking, value);
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

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private MvxAsyncCommand<Booking> cancelBookingCommand;
        public MvxAsyncCommand<Booking> CancelBookingCommand => this.cancelBookingCommand ??
            (this.cancelBookingCommand = new MvxAsyncCommand<Booking>((item) => this.CancelBooking(item)));

        private async Task CancelBooking(Booking booking)
        {
            bool answer = await DisplayAlert(ApplicationResource.Warning, String.Format(ApplicationResource.BookingCancelChoice, booking.Title), ApplicationResource.Yes, ApplicationResource.No);
            if (answer)
            {
                this.IsBusy = true;
                BookingOption[] bookingOption = { new BookingOption { HoldingId = booking.HoldingId, Id = booking.Id, RecordId = booking.RecordId } };

                BookingOptions opt = new BookingOptions()
                {
                    serviceCode = "SYRACUSE",
                    bookings = bookingOption,
                };

                CancelBookingResult res = await this.requestService.CancelBooking(opt);
                if (res == null)
                {
                    this.DisplayAlert(ApplicationResource.Error, ApplicationResource.ErrorOccurred, ApplicationResource.ButtonValidation);
                    return;
                }
                else if (!res.Success)
                {
                    this.DisplayAlert(ApplicationResource.Error, res.Errors[0].Msg, ApplicationResource.ButtonValidation);
                }
                else
                {
                    if (res.D.SuccessCount > 0)
                    {
                        this.DisplayAlert(ApplicationResource.Success, String.Format(ApplicationResource.SuccessCancelBooking, booking.Title), ApplicationResource.ButtonValidation);
                    }
                    else
                    {
                        this.DisplayAlert(ApplicationResource.Error, res.D.Errors[0].Value, ApplicationResource.ButtonValidation);
                    }
                }
                await refreshBooking();
            }
        }

        public BookingViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public async override void Prepare()
        {
            await refreshBooking();
        }

        private async Task refreshBooking()
        {
            this.IsBusy = true;
            BookingResult bookings = await this.requestService.GetBookings();
            if (!bookings.Success)
            {
                this.DisplayAlert(ApplicationResource.Error, bookings.Errors[0].Msg, ApplicationResource.ButtonValidation);
                return;
            }
            this.Results = bookings.D.Bookings;
            List<Booking> tmp = new List<Booking>(this.Results);
            this.Results = tmp.ToArray();
            if (this.results.Length == 0)
            {
                this.NotCurrentBooking = true;
            }
            else
            {
                this.NotCurrentBooking = false;
            }
            await this.GetRedirectURL();
            await this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }
        private async Task GetRedirectURL()
        {
            foreach (var booking in this.Results)
            {
                booking.ThumbnailUrl = this.requestService.GetRedirectURL(booking.ThumbnailUrl, booking.DefaultThumbnailUrl);
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
            await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
        }
    }
}
