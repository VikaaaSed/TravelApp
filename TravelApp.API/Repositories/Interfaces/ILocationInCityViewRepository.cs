using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationInCityViewRepository
    {
        Task<IEnumerable<LocationInCity>> GetLocationInCityByCityIdAsync(int cityId);
    }
}
