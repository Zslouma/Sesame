using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
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
            this.Items.SelectionChanged += this.OnItemSelected;
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            this.Items.SelectionChanged -= this.OnItemSelected;
            base.OnDisappearing();
        }

        /*
         * Disable cell click higlight.
         */
        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
            if (e.CurrentSelection.Count > 0)
            {
                MenuNavigation item = e.CurrentSelection[0] as MenuNavigation;
                Console.WriteLine(item.Text);
                await this.ViewModel.ShowDetailPageAsync(item);
            }
            else
            {
                await this.DisplayAlert("Erreur", "Une erreur est survenue", "Ok");
            }
            }
            catch (Exception ex)
            {
                Log.Warning("Mobidoc", ex.Message);
                throw;
            }
        }
    }
}