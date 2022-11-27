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

        private CookiesSave cookiesSave = new CookiesSave();
        public CookiesSave CookiesSave
        {
            get => this.cookiesSave;
            set
            {
                SetProperty(ref this.cookiesSave, value);
            }

        }

        public DisplayCardViewModel()
        {
            
        }

        public async override Task Initialize()
        {
            this.CookiesSave = await App.Database.GetActiveUser();
            await base.Initialize();          
        }
    }
}
