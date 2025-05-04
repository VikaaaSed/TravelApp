using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class CityService : ICityService
    {
        private readonly CityHttpClient _cityHttpClient;
        private readonly LocationHttpClient _locationHttpClient;
        public CityService(CityHttpClient cityHttpClient, LocationHttpClient locationHttpClient)
        {
            _cityHttpClient = cityHttpClient;
            _locationHttpClient = locationHttpClient;
        }
        public async Task<AllCityInformation> GetAllCityInformationByPageNameAsync(string pageName)
        {
            City city = await GetCityByPageNameAsync(pageName);
            List<Location> locations = await GetVisibleLocationAsync(city.Id);
            return new AllCityInformation(city, locations);
        }
        public async Task<City> GetCityByPageNameAsync(string pageName)
            => await _cityHttpClient.GetCityByPageNameAsync(pageName);
        public async Task<List<Location>> GetLocationByCityIdAsync(int cityId)
        {
            var result = await _locationHttpClient.GetLocationByCityIdAsync(cityId);
            return result.ToList();
        }
        public async Task<List<Location>> GetVisibleLocationAsync(int cityId)
        {
            var result = await _locationHttpClient.GetVisibleLocationByCityIdAsync(cityId);
            return result.ToList();
        }
    }
}
