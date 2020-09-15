using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Syracuse.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Master, WrapInNavigationPage = false, Title = "Menu")]
    public partial class MenuView : MvxContentPage<MenuViewModel>
    {
        public MenuView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.Items.ItemSelected += this.OnItemSelected;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.Items.ItemSelected -= this.OnItemSelected;
            base.OnDisappearing();
        }

        /*
         * Disable cell click higlight.
         */
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}