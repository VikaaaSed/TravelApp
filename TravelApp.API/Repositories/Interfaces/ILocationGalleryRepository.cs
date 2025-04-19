using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationGalleryRepository
    {
        public Task<IEnumerable<LocationGallery>> GetGalleryByIdLocationAsync(int locationId);
        public Task<LocationGallery?> GetAsync(int id);
        public Task<LocationGallery> CreateAsync(LocationGallery gallery);
        public Task UpdateAsync(LocationGallery gallery);
        public Task DeleteAsync(int id);
    }
}
