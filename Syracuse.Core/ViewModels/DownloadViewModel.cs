using MvvmCross.Commands;
using MvvmCross.Navigation;
using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public class DownloadViewModel : BaseViewModel
    {
        private readonly IRequestService requestService;
        private readonly IMvxNavigationService navigationService;

        IDownloader downloader = DependencyService.Get<IDownloader>();

        private MvxAsyncCommand<string> searchCommand;

        public MvxAsyncCommand<string> SearchCommand => this.searchCommand ?? (this.searchCommand = new MvxAsyncCommand<string>((text) => this.PerformSearch(text)));

        private async Task PerformSearch(string search)
        {
            var options = new SearchOptionsDetails()
            {
                QueryString = search
            };
            SearchOptions opt = new SearchOptions() { Query = options };
            if (App.AppState.NetworkConnection)
            {
                await this.navigationService.Navigate<SearchViewModel, SearchOptions>(opt);
            }
            else
            {
                this.DisplayAlert(ApplicationResource.Warning, ApplicationResource.NetworkDisable, ApplicationResource.ButtonValidation);
            }
        }

        private MvxAsyncCommand<DocumentSave> deleteDocumentCommand;
        public MvxAsyncCommand<DocumentSave> DeleteDocumentCommand => this.deleteDocumentCommand ??
            (this.deleteDocumentCommand = new MvxAsyncCommand<DocumentSave>((item) => this.DeleteDocument(item)));

        /// <summary>
        /// Déclenche une oppération de télecharcheement de document 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task DeleteDocument(DocumentSave result)
        {
            this.isBusy = true;
            await RaiseAllPropertiesChanged();
            if (System.IO.File.Exists(result.DocumentPath))
            {
                System.IO.File.Delete(result.DocumentPath);
            }
            await App.DocDatabase.DeleteItemAsync(result);
            this.Results = await App.DocDatabase.GetItemsAsync();
            this.isBusy = false;
            await RaiseAllPropertiesChanged();
            this.ForceListUpdate();
            await RaiseAllPropertiesChanged();
        }

        /// <summary>
        /// Booléen qui permet de permuté l'affichage en cas d'absence de resultat 
        /// </summary>
        private bool notCurrentDownload = false;
        public bool NotCurrentDownload
        {
            get => this.notCurrentDownload;
            set
            {
                this.ReversNotCurrentDownload = !value;
                SetProperty(ref this.notCurrentDownload, value);
            }
        }

        /// <summary>
        /// Booléen inverse de notCurrentDownload
        /// </summary>
        private bool reversNotCurrentDownload = true;
        public bool ReversNotCurrentDownload
        {
            get => this.reversNotCurrentDownload;
            set
            {
                SetProperty(ref this.reversNotCurrentDownload, value);
            }
        }

        /// <summary>
        /// List des resultats de recherche 
        /// </summary>
        private List<DocumentSave> results;
        public List<DocumentSave> Results
        {
            get => this.results;    
            set
            {
                SetProperty(ref this.results, value);
            }
        }
        /// <summary>
        /// variable contenant le nombres de resultats total à cette requête  
        /// </summary>
        private long? nbrResults;
        public long? NbrResults
        {
            get => this.nbrResults;
            set
            {
                SetProperty(ref this.nbrResults, value);
            }
        }

        /// <summary>
        /// Variable qui indique si la page est occupé 
        /// </summary>
        private bool isBusy = true;
        public bool IsBusy
        {
            get => this.isBusy;
            set
            {
                SetProperty(ref this.isBusy, value);
            }
        }

        public DownloadViewModel(IRequestService requestService, IMvxNavigationService navigationService)
        {
            this.requestService = requestService;
            this.navigationService = navigationService;
        }

        public async override Task Initialize()
        {
            this.IsBusy = true;
            var user = await App.Database.GetActiveUser();
            this.Results = await App.DocDatabase.GetItemsAsync();
            if (this.Results.Count == 0)
            {
                this.NotCurrentDownload = true;
            }
            else
            {
                this.NotCurrentDownload = false;
            }
            this.IsBusy = false;
            await base.Initialize();
        }

        ///// <summary>
        ///// Ouverture d'un fichier appartir de l'app 
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public async Task OpenFileDocument(DocumentSave item)
        //{
        //    IDownloader downloader = DependencyService.Get<IDownloader>();
            
        //    Console.WriteLine(item.DocumentPath);
        //    Console.WriteLine(downloader.GetPathStorage());
            
        //    if (System.IO.File.Exists(item.DocumentPath))
        //    {
        //        using (var memoryStream = new MemoryStream()) {
        //            var stream = System.IO.File.OpenRead(item.DocumentPath);
        //            await stream.CopyToAsync(memoryStream);
        //            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", memoryStream, PDFOpenContext.InApp);
        //        }
        //    }          

        //}

        private void ForceListUpdate()
        {
            var tempo = this.Results;
            this.Results = new List <DocumentSave>();
            this.Results = tempo;
        }
    }
}
