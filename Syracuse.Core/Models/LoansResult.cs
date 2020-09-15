using System;
using Newtonsoft.Json;
using Syracuse.Mobitheque.Core.Services.Requests;

namespace Syracuse.Mobitheque.Core.Models
{
    public class LoansResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public DataLoans D { get; set; }
    }


    public class DataLoans
    {
        [JsonProperty("Loans")]
        public Loans[] Loans { get; set; }
    }

    public class Loans
    {
        public string Id { get; set; }

        public string RecordId { get; set; }

        public string HoldingId { get; set; }

        public String Title { get; set; }

        public bool CanRenew { get; set; }

        public string CannotRenewReason { get; set; }

        public String TypeOfDocument { get; set; }

        public bool IsLate { get; set; }

        public bool IsSoonLate { get; set; }

        public String State { get; set; }

        public String Location { get; set; }

        public DateTime WhenBack { get; set; }

        public string WhenBack_String
        {
            get
            {

                return (DateTime.Now > WhenBack)
                    ? "En retard depuis le " + WhenBack.Date.ToShortDateString()
                    : "A rendre avant le " + WhenBack.Date.ToShortDateString();
            }
        }

        public String DefaultThumbnailUrl { get; set; }

        public String ThumbnailUrl { get; set; }

        public String dateColor {
            get
            {
                if (DateTime.Now > WhenBack)
                    return "#FF0000"; //red
                else if (DateTime.Now > WhenBack.AddDays(7.0))
                    return "#FFFF00"; // yellow
                return "#008000"; // vert
            }
        }

        public String buttonColor
        {
            get
            {
                return (CanRenew) ? "#0066ff": "#EAEAEA";
            }
        }

        public IRequestService RequestService { get; set; }
    }
}
