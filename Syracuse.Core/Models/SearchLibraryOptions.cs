using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{

    public class SearchLibraryOptions
    {
        [JsonProperty("Record")]
        public SearchLibraryOptionsDetails Record { get; set; } = new SearchLibraryOptionsDetails();
    }

    public class SearchLibraryOptionsDetails
    {
        [JsonProperty("RscId")]
        public string RscId { get; set; }

        [JsonProperty("Docbase")]
        public string Docbase { get; set; } = "SYRACUSE";
    }
}
