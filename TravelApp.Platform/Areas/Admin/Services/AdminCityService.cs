using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Areas.Admin.Services
{
    public class AdminCityService : IAdminCityService
    {
        private readonly CityHttpClient _cityHttpClient;
        private readonly LocationHttpClient _locationHttpClient;
        private readonly IAdminLocationService _adminLocationService;
        public AdminCityService(CityHttpClient cityHttpClient, LocationHttpClient locationHttpClient,
            IAdminLocationService adminLocationService)
        {
            _cityHttpClient = cityHttpClient;
            _locationHttpClient = locationHttpClient;
            _adminLocationService = adminLocationService;
        }
        public async Task<City?> GetCityByPageNameAsync(string pageName)
            => await _cityHttpClient.GetCityByPageNameAsync(pageName);
        public async Task<City> CreateCityAsync(City city)
            => await _cityHttpClient.CreateCityAsync(city);
        public async Task DeleteCityAsync(int cityId)
            => await _cityHttpClient.DeleteCityAsync(cityId);
        public async Task<List<City>> GetAllCityAsync()
        {
            var result = await _cityHttpClient.GetAllAsync();
            return result.ToList();
        }
        public async Task<AllCityInformation> GetAllCityInformationAsync(int id)
        {
            City city = await GetCityAsync(id);
            List<Location> locations = await GetLocationInCityByCityIdAsync(city.Id);
            return new AllCityInformation(city, locations);
        }
        public async Task<City> GetCityAsync(int cityId)
            => await _cityHttpClient.GetCityAsync(cityId);
        public async Task<List<Location>> GetLocationInCityByCityIdAsync(int cityId)
        {
            var result = await _locationHttpClient.GetLocationByCityIdAsync(cityId);
            return result.ToList();
        }
        public async Task UpdateCityAsync(City city)
            => await _cityHttpClient.UpdateCityAsync(city);

        public async Task<int> DeleteLocationInCityAsync(int idLocation)
        {
            var location = await _adminLocationService.GetLocationAsync(idLocation);
            int idCity = location.IdCity;
            await _adminLocationService.DeleteLocation(idLocation);
            return idCity;
        }
    }
}
