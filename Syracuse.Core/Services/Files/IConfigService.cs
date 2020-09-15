using Syracuse.Mobitheque.Core.Models;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public interface IConfigService
    {
        Task<Config> GetConfig();
    }
}
