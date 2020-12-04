﻿using Syracuse.Mobitheque.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Requests
{
    public interface IRequestService
    {
        /// <summary>
        /// Get cookies of current httpClient.
        /// </summary>
        /// <returns>Current cookies</returns>
        IEnumerable<Cookie> GetCookies(string url = null);

        void LoadCookies(Cookie[] cookies);

        void ResetCookies();

        void clearCookies();

        void clearCookies(Cookie cotarget);

        String GetRedirectURL(string originalURL, string defaultURL = "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png");

        Task<SearchResult> Search(SearchOptions options, Action<Exception> error = null);

        Task<AccountSummary> GetSummary(Action<Exception> error = null);

        Task<SearchLibraryResult> SearchLibrary(SearchLibraryOptions options, Action<Exception> error = null);

        Task<LoginStatus> Authentication(string useraccount, string password, string baseUrl, Action<Exception> error = null);

        Task<LoansResult> GetLoans(Action<Exception> error = null);

        Task<BookingResult> GetBookings(Action<Exception> error = null);

        Task<BasketResult> SearchUserBasket(BasketOptions options, Action<Exception> error = null);

        Task<PlaceReservationResult> PlaceReservation(PlaceReservationOptions options, Action<Exception> error = null);

        Task<RenewLoanResult> RenewLoans(LoanOptions options, Action<Exception> error = null);

        Task<CancelBookingResult> CancelBooking(BookingOptions options, Action<Exception> error = null);
    }
}
