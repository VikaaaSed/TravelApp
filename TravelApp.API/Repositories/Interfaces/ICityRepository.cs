using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAllAsync();
        Task<IEnumerable<City>> GetVisibleCityAsync();
    }
}
