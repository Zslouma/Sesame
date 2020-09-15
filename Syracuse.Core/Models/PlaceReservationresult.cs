using System;
using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class PlaceReservationResult
    {
        [JsonProperty("errors")]
        public object[] PlaceReservationError { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

 
    }
    public class PlaceReservationError
    {
        [JsonProperty("msg")]
        public String Msg { get; set; }

    }


}
