using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class AccountSummary
    {
        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public DataAccount D { get; set; }
    }

    public class DataAccount
    {
        public AccountMessage[] AccountAbstractMessageCollection { get; set; }

        public SummaryAccount AccountSummary { get; set; }
    }

    public class AccountMessage
    {
        public string Message { get; set; }

        public int Priority { get; set; }

        public int Type { get; set; }
    }

    public class SummaryAccount
    {
        public int BookingsAvailableCount { get; set; }

        public int BookingsNotAvailableCount { get; set; }

        public int BookingsTotalCount { get; set; }

        public string DisplayName { get; set; }

        public int HandingsCount { get; set; }

        public int LoansLateCount { get; set; }

        public int LoansNotLateCount { get; set; }

        public int LoansTotalCount { get; set; } 
    }
}
