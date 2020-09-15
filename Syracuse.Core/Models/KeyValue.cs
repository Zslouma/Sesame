using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class KeyValue
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }

}
