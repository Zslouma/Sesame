using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;

namespace Syracuse.UI.Views
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
#pragma warning disable S1643 // Strings should not be concatenated using '+' in a loop
                    tempo += $"{a}\n";
#pragma warning restore S1643 // Strings should not be concatenated using '+' in a loop
                }
                InfoList.Children.Add(new Label { Text = tempo, TextColor = Color.Green });
            };
        }
    }
}
