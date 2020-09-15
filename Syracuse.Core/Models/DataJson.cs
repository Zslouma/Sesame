using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class DataJson
    {
        [JsonProperty("libraries")]
        public Library[] Libraries { get; set; }

        [JsonProperty("departments")]
        public Department[] Departments { get; set; }
    }
}
