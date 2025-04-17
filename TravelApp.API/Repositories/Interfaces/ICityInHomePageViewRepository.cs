using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ICityInHomePageViewRepository
    {
        Task<IEnumerable<CityInHomePage>> GetAllAsync();
        Task<IEnumerable<CityInHomePage>> GetVisibleAsync();
    }
}
