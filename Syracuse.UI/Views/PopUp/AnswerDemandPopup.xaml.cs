using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views.PopUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswerDemandPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly IRequestService requestService;

        private UserDemands demands;
        public UserDemands Demands
        {
            get => this.demands;
            set
            {
                this.demands = value;
            }
        }

        public AnswerDemandPopup(IRequestService requestService, UserDemands demands)
        {
            this.requestService = requestService;
            this.Demands = demands;
            InitializeComponent();
        }
        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        private void OnClose(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
        async void OnSend(object sender, EventArgs args)
        {
            DemandsOptions demandsOptions = new DemandsOptions(this.Demands.id, "je vous remercie");
            var result = await this.requestService.AnswerDemand(demandsOptions);
            if (result.Success && result.D.success)
            {
                CloseAllPopup();
            }
            else
            {

            }
            
        }
    }
}