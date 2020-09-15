using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public class GeolocationService : IGeolocationService
    {
        public async Task<string> GetCurrentPostalCode()
        {
            var location = await this.GetLocation();

            if (location != null)
            {
                try
                {
                    var placeMarks = await Xamarin.Essentials.Geocoding.GetPlacemarksAsync(location);

                    if (placeMarks != null)
                        return placeMarks.FirstOrDefault()?.PostalCode;
                } catch (Exception) {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public async Task<Location> GetLocation()
        {
            try
            {
                var locationTask = Xamarin.Essentials.Geolocation.GetLocationAsync();
                var lastLocationTask = Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
                var result = await Task.WhenAny<Location>(new List<Task<Location>>() { locationTask, lastLocationTask });

                if (result == null)
                    return null;
                return await result;
            } catch (Exception) {
                return null;
            }
        }
    }
}
