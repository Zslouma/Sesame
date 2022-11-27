using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{

    public class SearchOptions
    {
        [JsonProperty("query")]
        public SearchOptionsDetails Query { get; set; } = new SearchOptionsDetails();

        public string PageTitle { get; set; } = "";
        public string PageIcone { get; set; } = "";
    }

    public class SearchOptionsDetails
    {

        public bool ForceSearch { get; set; } = true;

        public int Page { get; set; } = 0;

        public string QueryString { get; set; } = "*";

        public int ResultSize { get; set; } = 15;

        public string ScenarioCode { get; set; } = "";

        public string SortField { get; set; } = "";

        public string FacetFilter { get; set; } = "";

        public int SortOrder { get; set; } = 0;

        public bool InjectFields { get; set; } = true;
    }
}
