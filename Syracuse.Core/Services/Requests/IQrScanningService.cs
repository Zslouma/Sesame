using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Requests
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
