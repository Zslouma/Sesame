using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public interface IGeolocationService
    {
        Task<string> GetCurrentPostalCode();
        Task<Location> GetLocation();
    }
}
