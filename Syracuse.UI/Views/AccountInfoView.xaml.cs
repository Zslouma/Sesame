using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "")]
    public partial class AccountInfoView : MvxContentPage<AccountInfoViewModel>
    {
        public AccountInfoView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.ViewModel.PropertyChanged += (sender, args) => {
                if (!args.PropertyName.Equals("InTimeBorrowedDocuments"))
                    return;
                string tempo = "";
                foreach (var a in ((AccountInfoViewModel)sender).InTimeBorrowedDocuments)
                {
                    tempo = $"{a}";
                    InfoList.Children.Add(new Label { Text = tempo, TextColor = (Color)Application.Current.Resources["AccountInfoTextColor"], FontSize = 18 , Margin = new Thickness(10,0,0,0)});
                }
                
            }; 
        }
        protected override void OnBindingContextChanged()
        {
            (this.DataContext as AccountInfoViewModel).OnDisplayAlert += AccountInfoView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }
        private void AccountInfoView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}
