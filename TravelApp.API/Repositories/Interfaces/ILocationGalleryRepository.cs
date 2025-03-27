using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationGalleryRepository
    {
        public Task<IEnumerable<LocationGallery>> GetGalleryByIdLocationAsync(int locationId);
    }
}
