using System.Net;
using Newtonsoft.Json;

namespace Syracuse.Mobitheque.Core.Services.Requests
{
    public static class CookiesManagerService
    {
        public static string CookiesToString(Cookie[] cookies)
        {
            string ret = JsonConvert.SerializeObject(cookies);
            
            return ret;
        }
    }
}
