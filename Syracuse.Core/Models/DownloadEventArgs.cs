using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class DownloadEventArgs : EventArgs
    {
        public bool FileSaved = false;
        public DownloadEventArgs(bool fileSaved)
        {
            FileSaved = fileSaved;
        }
    }
}
