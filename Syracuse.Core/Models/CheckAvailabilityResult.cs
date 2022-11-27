using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class CheckAvailabilityResult
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public CheckAvailabilityData[] D { get; set; }
    }
    public class CheckAvailabilityData
    {
        [JsonProperty("HtmlView")]
        public string HtmlView { get; set; }

        [JsonProperty("Id")]
        public IdCheckAvailability Id { get; set; }
    }
    public class IdCheckAvailability
    {
        [JsonProperty("Docbase")]
        public string Docbase { get; set; }

        [JsonProperty("RscId")]
        public string RscId { get; set; }
    }

}
