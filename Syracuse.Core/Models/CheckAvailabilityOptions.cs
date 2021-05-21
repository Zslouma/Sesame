using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class CheckAvailabilityOptions
    {
        [JsonProperty("RecordIdArray")]
        public List<RecordIdArray> RecordIdArray { get; set; }

        [JsonProperty("searchQuery")]
        public SearchOptionsDetails Query { get; set; } = new SearchOptionsDetails();
    }

    public class RecordIdArray
    {
        public string Docbase { get; set; }

        public string RscId { get; set; }

        public string RscType { get; set; }

        public RecordIdArray(string Docbase, string RscId, string RscType)
        {
            this.Docbase = Docbase;
            this.RscId = RscId;
            this.RscType = RscType;
        }

    }
}
