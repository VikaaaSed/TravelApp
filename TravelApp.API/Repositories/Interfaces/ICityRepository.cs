using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ICityRepository
    {
        public Task<IEnumerable<City>> GetAllAsync();
        public Task<IEnumerable<City>> GetVisibleCityAsync();
        public Task<City?> GetCityByPageNameAsync(string PageName);
        public Task<City> CreateCityAsync(City city);
        public Task UpdateCityAsync(City city);
    }
}
