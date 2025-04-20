using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        public Task<Location?> GetAsync(int id);
        public Task<IEnumerable<Location>> GetLocationByCityIdAsync(int cityId);
        public Task<Location> CreateAsync(Location location);
        public Task UpdateAsync(Location location);
        public Task DeleteAsync(int id);
        public Task<IEnumerable<Location>> GetAllAsync();
    }
}
