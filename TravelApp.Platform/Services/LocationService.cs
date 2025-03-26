using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class LocationService : ILocationService
    {
        private readonly LocationViewHttpClient _locationViewHttpClient;
        public LocationService(LocationViewHttpClient locationViewHttpClient)
        {
            _locationViewHttpClient = locationViewHttpClient;
        }
        public async Task<LocationInHomePage> GetLocationInHomePageByPageNameAsync(string pageName)
            => await _locationViewHttpClient.GetLocationByPageNameAsync(pageName);
    }
}
