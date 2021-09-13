using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class WebAndCookiesViewModel : BaseViewModel<string, string>
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        private bool isBusy = true;

        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        private CookieContainer cookies { get; set; }

        public CookieContainer Cookies { 
            get { return cookies; }
            set { this.cookies = value; }
        }


        public string Url { get; set; }

        public WebAndCookiesViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }


        public override void Prepare(string parameter)
        {
            this.IsBusy = true;
            this.Cookies = this.requestService.GetCookieContainer();
            Console.WriteLine("GetCookieContainer = " + this.Cookies.Count.ToString());
            this.Url = parameter;
            this.RaiseAllPropertiesChanged();
            this.IsBusy = false;
        }

        public CookieContainer GetCookies()
        {
            return requestService.GetCookieContainer(); ;
        } 
    }
}
