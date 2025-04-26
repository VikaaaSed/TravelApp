using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface IAdminCityService
    {
        public Task<List<City>> GetAllCityAsync();
        public Task<City> GetCityAsync(int cityId);
        public Task UpdateCityAsync(City city);
        public Task DeleteCityAsync(int cityId);
        public Task<City> CreateCityAsync(City city);
        public Task<AllCityInformation> GetAllCityInformationAsync(int id);
        public Task<List<LocationInCity>> GetLocationInCityByCityIdAsync(int cityId);
        public Task<City?> GetCityByPageNameAsync(string pageName);
        public Task<int> DeleteLocationInCityAsync(int idLocation);
    }
}
