using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        public Task<Location?> GetAsync(int id);
        Task<IEnumerable<Location>> GetLocationByCityId(int cityId);

    }
}
