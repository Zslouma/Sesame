using MvvmCross.Forms.Views;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Forms;
using ZXing;

namespace Syracuse.Mobitheque.UI.Views
{
    public partial class BarcodeSearchView : MvxNavigationPage<BarcodeSearchModel>
    {

        public BarcodeSearchView()
        {
            InitializeComponent();
            ZxingScannerView scanner = new ZxingScannerView();
            scanner.OnScanResult += (Result r) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Scanned result", r.Text, "OK");
                    this.ViewModel.Result = r.Text;
                });
            };
            this.PushAsync(scanner);
        }
    }
}
