using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class WebAndCookiesAuthentificationParameters
    {
        public WebAndCookiesAuthentificationParameters(string url, CookiesSave department)
        {
            Url = url;
            Department = department;
        }

        public string Url { get; set; }

        public CookiesSave Department { get; set; }
    }
}
