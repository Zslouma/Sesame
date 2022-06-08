using Newtonsoft.Json;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using Refit;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.Services.Requests
{
    public class RequestService : IRequestService
    {
        private Uri httpUri;
        private IRefitRequests requests;
        private CookieContainer cookies;
        private HttpClientHandler handler;
        private String token;

        public RequestService()
        {
            this.cookies = new CookieContainer();
            this.handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = this.cookies,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            this.token = this.Timestamp();
        }
        /// <summary>
        /// Vérifie si l'url passé en paramétre posséde une redirection ou non
        /// </summary>
        /// <param name="originalURL"></param>
        /// <param name="defaultURL"></param>
        /// <returns></returns>
        public async Task<string> GetRedirectURL(string originalURL, string defaultURL = "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png")
        {
            try
            {
                var tempohandler = new HttpClientHandler()
                {
                    UseCookies = true,
                    CookieContainer = this.cookies,
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

                };
                tempohandler.AllowAutoRedirect = false;
                tempohandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient httpClient = new HttpClient(tempohandler);
                httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
                httpClient.DefaultRequestHeaders.CacheControl.NoStore = true;
                httpClient.DefaultRequestHeaders.CacheControl.NoCache = true;

                // Get the response ...
                using (var webResponse = (HttpResponseMessage)await httpClient.GetAsync(originalURL))
                {
                    // Now look to see if it's a redirect
                    if ((int)webResponse.StatusCode >= 300 && (int)webResponse.StatusCode <= 399)
                    {
                        string uriString = webResponse.RequestMessage.RequestUri.ToString();
                        return uriString;

                    }
                    else if ((int)webResponse.StatusCode >= 200 && (int)webResponse.StatusCode <= 299)
                    {
                        string uriString = webResponse.RequestMessage.RequestUri.ToString();
                        return uriString;
                    }
                    else
                    {
                        return defaultURL;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(defaultURL);
                return defaultURL;
            }
        }

        public String Timestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds).ToString();
        }



        #region Cookies
        public void clearCookies()
        {
            foreach (Cookie co in cookies.GetCookies(this.httpUri))
            {
                co.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
            }
        }
        public void clearCookies(Cookie cotarget)
        {
            foreach (Cookie co in cookies.GetCookies(this.httpUri))
            {
                if (co == cotarget)
                {
                    co.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                }
            }
        }

        public CookieContainer GetCookieContainer()
        {
            return this.cookies;
        }

        public IEnumerable<Cookie> GetCookies(string url = null, CookieContainer cookiesTempo = null)
        {
            if (!String.IsNullOrEmpty(url))
            {
                this.InitializeHttpClient(url);
            }
            foreach (Cookie cookie in cookiesTempo == null ? this.cookies.GetCookies(this.httpUri) : cookiesTempo.GetCookies(new Uri(url))){
                yield return cookie;
            }
        }
        public async Task UpdateCookies()
        {
            CookiesSave b = await App.Database.GetActiveUser();
            var cookiesGetted = GetCookies().ToArray();
            List<Cookie> cookiesGettedTempo = new List<Cookie> { };
            List<String> name = new List<String> { };
            foreach (var cook in cookiesGetted.OrderByDescending(cook => cook.Expires))
            {
                if (!name.Contains(cook.Name))
                {
                    name.Add(cook.Name);
                    if (cook.Expires < DateTime.Now)
                    {
                        cook.Expires = cook.TimeStamp.AddMonths(1);
                    }
                    cookiesGettedTempo.Add(cook);
                }
                else
                {
                    cook.Expires = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                }

            }
            cookiesGetted = cookiesGettedTempo.ToArray();
            b.Cookies = JsonConvert.SerializeObject(cookiesGetted);
            await App.Database.SaveItemAsync(b);
            var cookiestempo = JsonConvert.DeserializeObject<Cookie[]>(b.Cookies);
            this.LoadCookies(cookiestempo);

        }
        public void LoadCookies(Cookie[] cookies)
        {
            this.cookies = new CookieContainer();
            foreach (Cookie cookie in cookies)
            {
                this.cookies.Add(cookie);
            }
            this.handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = this.cookies,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
        }

        public void ResetCookies()
        {
            this.cookies = new CookieContainer();
            this.handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = this.cookies,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }

            };
        }


        #endregion



        public async Task<LoginStatus> Authentication(string useraccount, string password, string baseUrl, Action<Exception> error = null)
        {
            this.ResetCookies();
            this.InitializeHttpClient(baseUrl);

            try
            {
                var status = await this.requests.Authentication<LoginStatus>(new Dictionary<string, object>() {
                                                                              { "username", useraccount},
                                                                              { "password", password},
                                                                              { "rememberMe", true}});
                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }
        /// <summary>
        /// Fournie une url possédant un token d'Authentication lorsque c'est possible
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<UrlWithAuthenticationStatus> GetUrlWithAuthenticationTransfert(Uri uri)
        {

            await this.InitializeHttpClient();

            UrlWithAuthenticationStatus status;
            try
            {
                UrlWithAuthenticationTransfertOptions transfertOptions = new UrlWithAuthenticationTransfertOptions(uri.ToString());
                status = await this.requests.GetUrlWithAuthenticationTransfert<UrlWithAuthenticationStatus>(transfertOptions);
                return status;
            }
            catch (Exception)
            {
                status = new UrlWithAuthenticationStatus();
                return status;
            }

        }


        private async Task InitializeHttpClient()
        {
            CookiesSave user = await App.Database.GetActiveUser();
            if (user != null)
            {
                this.httpUri = new Uri(user.LibraryUrl);

                await this.UpdateCookies();

                HttpClient httpClient = new HttpClient(this.handler)
                {
                    BaseAddress = this.httpUri
                };
                httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
                httpClient.DefaultRequestHeaders.CacheControl.NoStore = true;
                httpClient.DefaultRequestHeaders.CacheControl.NoCache = true;
                this.requests = RestService.For<IRefitRequests>(httpClient);
            }
        }

        public void InitializeHttpClient(string url)
        {
            this.httpUri = new Uri(url);

            HttpClient httpClient = new HttpClient(this.handler)
            {
                BaseAddress = this.httpUri
            };
            httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
            httpClient.DefaultRequestHeaders.CacheControl.NoStore = true;
            httpClient.DefaultRequestHeaders.CacheControl.NoCache = true;
            this.requests = RestService.For<IRefitRequests>(httpClient);
        }

        public async Task<AccountSummary> GetSummary( string baseUrl = null, Cookie[] CookiesArray = null, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            if (baseUrl != null && CookiesArray != null)
            {
                this.ResetCookies();
                                this.InitializeHttpClient(baseUrl);
                this.LoadCookies(CookiesArray);


            }
            else
            {
                await this.InitializeHttpClient();
            }
            
            try
            {
                var status = await this.requests.GetSummary<AccountSummary>(this.token);
                if (status.Success)
                {
                    await UpdateCookies();
                }
                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }

        public async Task<SearchResult> Search(SearchOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (options.Query.ScenarioCode == "")
            {
                options.Query.ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode;
            }
            try
            {
                var status = await this.requests.Search<SearchResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                var status = new SearchResult();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
            
        }

        public async Task<CheckAvailabilityResult> CheckAvailability(CheckAvailabilityOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (options.Query.ScenarioCode == "") 
            {
                options.Query.ScenarioCode = (await App.Database.GetActiveUser()).SearchScenarioCode;
            }
            try
            {
                var status = await this.requests.CheckAvailability<CheckAvailabilityResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                var status = new CheckAvailabilityResult();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
        }

        public async Task<SearchLibraryResult> SearchLibrary(SearchLibraryOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();

            if (options == null)
                throw new ArgumentNullException(nameof(options));
            try
            {
                var status = await this.requests.SearchLibrary<SearchLibraryResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }

        public async Task<LoansResult> GetLoans(Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                var timestamp = this.Timestamp();
                this.token = this.Timestamp();
                var status = await this.requests.GetLoans<LoansResult>(timestamp, this.token);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                var status = new LoansResult();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
        }

        public async Task<BookingResult> GetBookings(Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                var timestamp = this.Timestamp();
                this.token = this.Timestamp();
                var status = await this.requests.GetBookings<BookingResult>(timestamp, this.token);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                var status = new BookingResult();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
        }

        public async Task<InstanceResult<DigitalDocumentCollection>> GetListDigitalDocuments(string parentDocumentId, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                if (parentDocumentId == null)
                    throw new ArgumentNullException(nameof(parentDocumentId));
                var status = await this.requests.GetListDigitalDocuments<InstanceResult<DigitalDocumentCollection>>(parentDocumentId);
                await UpdateCookies();
                return status;

            }
            catch (Exception ex)
            {
                var status = new InstanceResult<DigitalDocumentCollection>();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }

        }
        /// <summary>
        /// Permet de télécharger des documents sur ios et android 
        /// </summary>
        /// <param name="parentDocumentId"></param>
        /// <param name="documentId"></param>
        /// <param name="targetUrl"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public async Task<InstanceResult<string>> GetDownloadDocument(string parentDocumentId, string documentId,string filename, Action<Exception> error = null)
        {
            IDownloader downloader = DependencyService.Get<IDownloader>();
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                var user = App.Database.GetActiveUser();
                if (parentDocumentId == null)
                    throw new ArgumentNullException(nameof(parentDocumentId));

                string url = string.Format(this.httpUri + "/digitalCollection/DigitalCollectionAttachmentDownloadHandler.ashx?parentDocumentId={0}&documentId={1}&skipWatermark={2}&skipCopyright={3}", parentDocumentId, documentId, false, false);
                if (await Permissions.CheckStatusAsync<Permissions.StorageRead>() != PermissionStatus.Granted)
                {
                    await Permissions.RequestAsync<Permissions.StorageRead>();
                }
                if (await Permissions.CheckStatusAsync<Permissions.StorageWrite>() != PermissionStatus.Granted)
                {
                    await Permissions.RequestAsync<Permissions.StorageWrite>();
                }
                HttpClient httpClient = new HttpClient(this.handler)
                {
                    BaseAddress = this.httpUri
                };
                httpClient.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue();
                httpClient.DefaultRequestHeaders.CacheControl.NoStore = true;
                httpClient.DefaultRequestHeaders.CacheControl.NoCache = true;
                var path = downloader.GetPathStorage() + "/" + filename;
                byte[] fileBytes = await httpClient.GetByteArrayAsync(new Uri(url));
                System.IO.File.WriteAllBytes(path, fileBytes);
                this.requests = RestService.For<IRefitRequests>(httpClient);
                await UpdateCookies();
                var status = new InstanceResult<string>();
                status.Success = true;
                status.D = path;
                return status;
            }
            catch (Exception ex)
            {
                var status = new InstanceResult<string>();
                status.Success = false;
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else if (ex.Message != "" && ex.Message != null) {
                    status.Errors[0] = new Error(ex.Message);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
        }

        bool IsDownloading(IDownloadFile file)
        {
            if (file == null) return false;

            switch (file.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                case DownloadFileStatus.PAUSED:
                case DownloadFileStatus.PENDING:
                case DownloadFileStatus.RUNNING:
                    return true;

                case DownloadFileStatus.COMPLETED:
                case DownloadFileStatus.CANCELED:
                case DownloadFileStatus.FAILED:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Permet de récupérer le panier de l'utilisateur
        /// </summary>
        public async Task<BasketResult> SearchUserBasket(BasketOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options));
                var status = await this.requests.SearchUserBasket<BasketResult>(options);

                await UpdateCookies();
                return status;
            }
            catch (Exception ex)
            {
                var status = new BasketResult();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }

        }

        public async Task<InstanceResult<List<UserDemands>>> GetUserDemands(Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                var timestamp = this.Timestamp();
                this.token = this.Timestamp();
                var status = await this.requests.GetUserDemands<InstanceResult<List<UserDemands>>>(timestamp, this.token);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                var status = new InstanceResult<List<UserDemands>>();
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
                return status;
            }
        }
        public async Task<InstanceResult<object>> AnswerDemand(DemandsOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            var status = new InstanceResult<object>();
            try
            {
                var timestamp = this.Timestamp();
                this.token = this.Timestamp();
                if (options == null)
                    throw new ArgumentNullException(nameof(options));
                status = await this.requests.AnswerDemand<InstanceResult<object>>(options);
                await UpdateCookies();
            }
            catch (Exception ex)
            {
                status.Errors = new Error[1];
                if (!App.AppState.NetworkConnection)
                {
                    status.Errors[0] = new Error(ApplicationResource.NetworkDisable);
                }
                else
                {
                    status.Errors[0] = new Error(ApplicationResource.ErrorOccurred);
                }
                error?.Invoke(ex);
            }
            return status;

        }


            public async Task<RenewLoanResult> RenewLoans(LoanOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                this.token = this.Timestamp();
                var status = await this.requests.RenewLoans<RenewLoanResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }

        public async Task<CancelBookingResult> CancelBooking(BookingOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection " + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                this.token = this.Timestamp();
                var status = await this.requests.CancelBooking<CancelBookingResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }

        public async Task<PlaceReservationResult> PlaceReservation(PlaceReservationOptions options, Action<Exception> error = null)
        {
            if (!App.AppState.NetworkConnection)
            {
                Debug.WriteLine("NetworkConnection" + App.AppState.NetworkConnection);
            }
            await this.InitializeHttpClient();
            try
            {
                this.token = this.Timestamp();
                var status = await this.requests.PlaceReservation<PlaceReservationResult>(options);

                await UpdateCookies();

                return status;
            }
            catch (Exception ex)
            {
                error?.Invoke(ex);
                return null;
            }
        }


    }
}
