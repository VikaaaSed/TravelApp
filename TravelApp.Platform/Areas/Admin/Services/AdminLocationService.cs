using TravelApp.Platform.Areas.Admin.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.ClientAPI;

namespace TravelApp.Platform.Areas.Admin.Services
{
    public class AdminLocationService : IAdminLocationService
    {
        private readonly LocationHttpClient _locationHttpClient;
        private readonly LocationGalleryHttpClient _locationGalleryHttpClient;

        public AdminLocationService(LocationHttpClient locationHttpClient, LocationGalleryHttpClient locationGalleryHttpClient)
        {
            _locationHttpClient = locationHttpClient;
            _locationGalleryHttpClient = locationGalleryHttpClient;
        }

        public async Task<AllLocationInfo> GetAllLocationInfoAsync(int Id)
        {
            var location = await GetLocationAsync(Id);
            var gallery = await GetLocationGalleryAsync(Id);
            return new AllLocationInfo(Location.ConvertToUILocation(location), gallery);
        }

        public async Task<List<API.Models.Location>> GetAllLocationsAsync()
        {
            var locations = await _locationHttpClient.GetAllAsync();
            return locations.ToList();
        }

        public async Task<API.Models.Location?> GetLocationAsync(int Id)
            => await _locationHttpClient.GetAsync(Id);

        public async Task<List<API.Models.LocationGallery>> GetLocationGalleryAsync(int Id)
        {
            var gallery = await _locationGalleryHttpClient.GetLocationGalleryByIdLocationAsync(Id);
            return gallery.ToList();
        }
    }
}
