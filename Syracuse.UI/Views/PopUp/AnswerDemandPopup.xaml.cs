using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core;
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

        private TaskCompletionSource<bool> taskCompletionSource;
        public Task PopupClosedTask { get { return taskCompletionSource.Task; } }

        private UserDemands demands;
        public UserDemands Demands
        {
            get => this.demands;
            set
            {
                this.demands = value;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            taskCompletionSource.SetResult(true);
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
            DemandsOptions demandsOptions = new DemandsOptions(this.Demands.id, this.EditorMessage.Text);
            var result = await this.requestService.AnswerDemand(demandsOptions);
            if (result.Success)
            {

                CloseAllPopup();
            }
            else
            {
                if (!result.Success && result.Errors.Length > 0)
                {
                    await this.DisplayAlert(ApplicationResource.ErrorOccurred, result.Errors[0].Msg, ApplicationResource.ButtonValidation);
                    CloseAllPopup();
                }
            }
            
        }
    }
}