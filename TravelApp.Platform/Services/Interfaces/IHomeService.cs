using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IHomeService
    {
        public Task<List<CityInHomePage>> GetAllCityAsync();
        public Task<List<CityInHomePage>> GetVisibleCityAsync();
    }
}
