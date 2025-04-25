using TravelApp.Platform.Areas.Admin.Models;

namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface IAdminLocationService
    {
        public Task<List<API.Models.Location>> GetAllLocationsAsync();
        public Task<API.Models.Location?> GetLocationAsync(int Id);
        public Task<AllLocationInfo> GetAllLocationInfoAsync(int Id);
        public Task<List<API.Models.LocationGallery>> GetLocationGalleryAsync(int Id);
        public Task UpdateLocation(Location location);
        public Task EditGallery(List<API.Models.LocationGallery> galleries, int idLocation);
        public Task<API.Models.Location> CreateLocation(Location location);
        public Task DeleteLocation(int id);
    }
}
