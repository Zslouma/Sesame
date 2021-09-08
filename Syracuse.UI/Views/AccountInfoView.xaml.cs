using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true)]
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
    }
}
