using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class RenewLoanResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public DataRenewLoan D { get; set; }
    }

    public class DataRenewLoan
    {
        [JsonProperty("ErrorCount")]
        public int ErrorCount { get; set; }

        [JsonProperty("SuccessCount")]
        public int SuccessCount { get; set; }

        [JsonProperty("Errors")]
        public DataRenewLoanDetail[] Errors { get; set; }
    }

    public class DataRenewLoanDetail
    {
        //public Loans Key { get; set; } Les champs sont null ce qui fait crash la serialization

        public string Value { get; set; }
    }
}
