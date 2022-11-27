using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public interface IDownloader
    {
        string DownloadFile(string url, string folder);
        
        event EventHandler<DownloadEventArgs> OnFileDownloaded;

        string GetPathStorage();
    }
}
