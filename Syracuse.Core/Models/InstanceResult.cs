using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class InstanceResult<T>
    {
        [JsonProperty("errors")]
        public Error[] Errors { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("d")]
        public T D { get; set; }
    }
}
