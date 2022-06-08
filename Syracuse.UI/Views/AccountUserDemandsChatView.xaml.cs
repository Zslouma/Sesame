using MvvmCross.Forms.Views;
using Rg.Plugins.Popup.Services;
using Syracuse.Mobitheque.Core.ViewModels;
using Syracuse.Mobitheque.UI.Views.PopUp;
using System;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountUserDemandsChatView : MvxContentPage<AccountUserDemandsChatViewModel>
    {
        private AnswerDemandPopup _answerDemandPopup;
        public AccountUserDemandsChatView()
        {
            InitializeComponent();
        }

        private async void AnswerDemand_OnClicked(object sender, EventArgs e)
        {
            _answerDemandPopup = new AnswerDemandPopup(this.ViewModel.GetrequestService, this.ViewModel.Demands);
            await PopupNavigation.Instance.PushAsync(_answerDemandPopup);
            await _answerDemandPopup.PopupClosedTask;
            this.ViewModel.Update();
            //await this.ViewModel.AnswerDemand();
        }
    }
}