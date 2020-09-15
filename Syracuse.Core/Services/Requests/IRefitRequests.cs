using Refit;
using Syracuse.Mobitheque.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Requests
{
    public interface IRefitRequests
    {
        [Post("/logon.svc/logon")]
        Task<T> Authentication<T>([Body(BodySerializationMethod.UrlEncoded)]Dictionary<string, object> data);

        [Get("/Portal/UserAccountService.svc/RetrieveAccountSummary?serviceCode{code}&userUniqueIdentifier={uniqueId}&token={token}")]
        Task<T> GetSummary<T>([AliasAs("token")]string token,
                              [AliasAs("code")]string code = "",
                              [AliasAs("uniqueId")]string uniqueId = "");

        [Get("/Portal/UserAccountService.svc/ListLoans?serviceCode{code}&userUniqueIdentifier={uniqueId}&token={token}&timestamp={timestamp}")]
        Task<T> GetLoans<T>([AliasAs("timestamp")] string timestamp,
                            [AliasAs("token")] string token,
                            [AliasAs("code")]string code = "",
                            [AliasAs("uniqueId")]string uniqueId = "");

        [Get("/Portal/Services/UserAccountService.svc/ListBookings?serviceCode{code}&userUniqueIdentifier={uniqueId}&token={token}&timestamp={timestamp}")]
        Task<T> GetBookings<T>([AliasAs("timestamp")] string timestamp,
                               [AliasAs("token")] string token,
                               [AliasAs("code")]string code = "",
                               [AliasAs("uniqueId")]string uniqueId = "");

        [Get("/Portal/Services/UserAccountService.svc/ListHandings?serviceCode{code}&userUniqueIdentifier={uniqueId}&token={token}")]
        Task<T> GetHandlings<T>([AliasAs("token")] string token,
                                [AliasAs("code")]string code = "",
                                [AliasAs("uniqueId")]string uniqueId = "");


        [Post("/Portal/Recherche/Search.svc/Search")]
        Task<T> Search<T>([Body]SearchOptions body);

        [Post("/Portal/ILSClient.svc/GetHoldings")]
        Task<T> SearchLibrary<T>([Body]SearchLibraryOptions body);

        [Post("/Portal/ILSClient.svc/PlaceReservation")]
        Task<T> PlaceReservation<T>([Body]PlaceReservationOptions body);

        [Post("/Portal/Services/UserAccountService.svc/RenewLoans")]
        Task<T> RenewLoans<T>([Body]LoanOptions body);

        [Post("/Portal/Services/UserAccountService.svc/CancelBookings")]
        Task<T> CancelBooking<T>([Body]BookingOptions body);
    }
}
