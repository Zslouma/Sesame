using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{

        public class UrlWithAuthenticationTransfertOptions
        {
            [JsonProperty("url")]
            public string url { get; set; }

        public UrlWithAuthenticationTransfertOptions(string url)
        {
            this.url = url;
        }
    }
}
