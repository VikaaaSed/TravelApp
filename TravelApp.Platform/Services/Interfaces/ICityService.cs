using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ICityService
    {
        public Task<City> GetCityByPageNameAsync(string pageName);
        public Task<List<LocationInCity>> GetLocationInCityByCityIdAsync(int cityId);
        public Task<AllCityInformation> GetAllCityInformationByPageNameAsync(string pageName);
    }
}
