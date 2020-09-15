using System.Threading.Tasks;
using Syracuse.Mobitheque.Core.Models;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class DisplayCardViewModel : BaseViewModel
    {
        private SummaryAccount accountSummary;
        public SummaryAccount SummaryAccount
        {
            get => this.accountSummary;
            set
            {
                SetProperty(ref this.accountSummary, value);
            }

        }

        private CookiesSave cookiesSave;
        public CookiesSave CookiesSave
        {
            get => this.cookiesSave;
            set
            {
                SetProperty(ref this.cookiesSave, value);
            }

        }

        public DisplayCardViewModel(CookiesSave cookiesSave)
        {
            this.CookiesSave = cookiesSave;
        }

        public async override Task Initialize()
        {
            await base.Initialize();          
        }
    }
}
