using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Models
{
    public class LoginStatus
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public object D { get; set; }
    }

    public class Error
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        
        public Error(string msg)
        {
            this.Msg = msg;
        }
    }

    public class Data
    {
        [JsonProperty("__type")]
        public string Type { get; set; }

        [JsonProperty("badPasswordCount")]
        public long BadPasswordCount { get; set; }

        [JsonProperty("doCheckAdditionals")]
        public bool DoCheckAdditionals { get; set; }

        [JsonProperty("checkAdditionalsResult")]
        public bool CheckAdditionalsResult { get; set; }
    }
}
