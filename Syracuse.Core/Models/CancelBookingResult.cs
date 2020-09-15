using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class CancelBookingResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public DataCancelBooking D { get; set; }
    }

    public class DataCancelBooking
    {
        [JsonProperty("ErrorCount")]
        public int ErrorCount { get; set; }

        [JsonProperty("SuccessCount")]
        public int SuccessCount { get; set; }

        [JsonProperty("Errors")]
        public DataCancelBookingDetail[] Errors { get; set; }

    }

    public class DataCancelBookingDetail
    {
        public string Value { get; set; }
    }
}
