using System.Collections.Generic;
using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{

    public class LibraryInformations
    {
        [JsonProperty("building_name")]
        public string BuildingName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        public bool DisplayNavigationError { get; set; } = false;


    }
    public class ConfigLibrary
    {
        [JsonProperty("baseUri")]
        public string BaseUri { get; set; }
        [JsonProperty("search_scenario_code")]
        public string SearchScenarioCode { get; set; }
        [JsonProperty("events_scenario_code")]
        public string EventsScenarioCode { get; set; }
        [JsonProperty("is_event")]
        public bool IsEvent { get; set; }
        [JsonProperty("remember_me")]
        public bool RememberMe { get; set; }
        [JsonProperty("is_km")]
        public bool IsKm { get; set; }
        [JsonProperty("library_informations")]
        public List<LibraryInformations> BuildingInformations { get; set; }
    }

    public class Library
    {
        [JsonProperty("department_code")]
        public string DepartmentCode { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("config")]
        public ConfigLibrary Config { get; set; }
    }
}
