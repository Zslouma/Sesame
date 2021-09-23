using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Syracuse.Mobitheque.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebAndCookiesView : MvxContentPage<WebAndCookiesViewModel>
    {
        bool CanRefresh { get; set; } = true;
        public WebAndCookiesView()
        {
            InitializeComponent();
        }

    }

}