using Syracuse.Mobitheque.UI.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            this.MainPage = new NavigationPage();
            XF.Material.Forms.Material.Init(this);

        }
    }
}
