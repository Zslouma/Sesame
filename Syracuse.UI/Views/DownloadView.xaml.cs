using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.ViewModels;
using Xamarin.Essentials;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.UI.Views
{
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Download")]
    public partial class DownloadView : MvxContentPage<DownloadViewModel>
    {
        public DownloadView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.resultsListDownloadDocument.ItemTapped += ResultsList_ItemTapped;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.resultsListDownloadDocument.ItemTapped -= ResultsList_ItemTapped;
        }

        protected async void ResultsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as DocumentSave;

            await this.OpenFileDocument(item);
        }

        protected override void OnBindingContextChanged()
        {
            (this.DataContext as DownloadViewModel).OnDisplayAlert += DownloadView_OnDisplayAlert;
            base.OnBindingContextChanged();
        }

        /// <summary>
        /// Ouverture d'un fichier appartir de l'app 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task OpenFileDocument(DocumentSave item)
        {
            IDownloader downloader = DependencyService.Get<IDownloader>();

            if (System.IO.File.Exists(item.DocumentPath))
            {
                string application = "";
                string extension = Path.GetExtension(item.DocumentPath);

                // get mimeTye
                switch (extension.ToLower())
                {
                    case ".txt":
                        application = "text/plain";
                        await this.TryOpen(item.DocumentPath);
                        break;
                    case ".doc":
                    case ".docx":
                        application = "application/msword";
                        await this.TryOpen(item.DocumentPath);
                        break;
                    case ".pdf":
                        application = "application/pdf";
                        using (var memoryStream = new MemoryStream())
                        {
                            var stream = System.IO.File.OpenRead(item.DocumentPath);
                            await stream.CopyToAsync(memoryStream);
                            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", memoryStream, PDFOpenContext.InApp);
                        }
                        break;
                    case ".xls":
                    case ".xlsx":
                        application = "application/vnd.ms-excel";
                        await this.TryOpen(item.DocumentPath);
                        break;
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        application = "image/jpeg";
                        await this.TryOpen(item.DocumentPath);
                        break;
                    default:
                        application = "*/*";
                        await this.TryOpen(item.DocumentPath);
                        break;
                }

            }

        }

        public async Task TryOpen(string path)
        {
            try
            {
                await Launcher.TryOpenAsync(new Uri("/storage/emulated/0/Download/image.jpeg"));
            }
            catch (Exception)
            {
                Console.WriteLine("Erreur sur l'ouverture du fichier");
            }
        }

        private void DownloadView_OnDisplayAlert(string title, string message, string button) => this.DisplayAlert(title, message, button);
    }
}