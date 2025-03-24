using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ICityService
    {
        public Task<City> GetCityByPageNameAsync(string pageName);
    }
}
