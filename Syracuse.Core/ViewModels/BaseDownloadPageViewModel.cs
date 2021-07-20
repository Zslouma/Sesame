using Syracuse.Mobitheque.Core.Models;
using Syracuse.Mobitheque.Core.Services.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public abstract class BaseDownloadPageViewModel : BaseViewModel
    {

        
        internal async Task<Result[]> HaveDownloadOption(Result[] results, IRequestService requestService)
        {
            foreach (var result in results)
            {
                if (result.HasViewerDr)
                {
                    var status = await requestService.GetListDigitalDocuments(result.Resource.RscId);
                    if (status.Success)
                    {
                        if (status.D.Documents[0].canDownload)
                        {
                            result.CanDownload = true;
                            result.downloadOptions.parentDocumentId = status.D.Documents[0].parentDocumentId;
                            result.downloadOptions.documentId = status.D.Documents[0].documentId;
                            result.downloadOptions.fileName = status.D.Documents[0].fileName;
                            if (await App.DocDatabase.GetDocumentsByDocumentID(result.Resource.RscId) != null)
                            {
                                result.CanDownload = false;
                                result.IsDownload = true;
                            }
                        }
                    }
                }
            }
            return results;
        }
    }
}
