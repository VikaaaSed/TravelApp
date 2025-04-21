using TravelApp.Platform.Areas.Admin.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.ClientAPI;

namespace TravelApp.Platform.Areas.Admin.Services
{
    public class AdminLocationService : IAdminLocationService
    {
        private readonly LocationHttpClient _locationHttpClient;
        public AdminLocationService(LocationHttpClient locationHttpClient)
        {
            _locationHttpClient = locationHttpClient;
        }

        public async Task<AllLocationInfo> GetAllLocationInfoAsync(int Id)
        {
            var location = await GetLocationAsync(Id);
            return new AllLocationInfo(Location.ConvertToUILocation(location));
        }

        public async Task<List<API.Models.Location>> GetAllLocationsAsync()
        {
            var locations = await _locationHttpClient.GetAllAsync();
            return locations.ToList();
        }

        public async Task<API.Models.Location?> GetLocationAsync(int Id)
            => await _locationHttpClient.GetAsync(Id);
    }
}
