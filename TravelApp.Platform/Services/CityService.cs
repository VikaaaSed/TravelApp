using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class CityService : ICityService
    {
        private readonly CityHttpClient _cityHttpClient;
        private readonly LocationViewHttpClient _locationViewHttpClient;
        public CityService(CityHttpClient cityHttpClient, LocationViewHttpClient locationViewHttpClient)
        {
            _cityHttpClient = cityHttpClient;
            _locationViewHttpClient = locationViewHttpClient;
        }
        public async Task<AllCityInformation> GetAllCityInformationByPageNameAsync(string pageName)
        {
            City city = await GetCityByPageNameAsync(pageName);
            List<LocationInCity> locations = await GetLocationInCityByCityIdAsync(city.Id);
            return new AllCityInformation(city, locations);
        }
        public async Task<City> GetCityByPageNameAsync(string pageName)
            => await _cityHttpClient.GetCityByPageNameAsync(pageName);
        public async Task<List<LocationInCity>> GetLocationInCityByCityIdAsync(int cityId)
        {
            var result = await _locationViewHttpClient.GetLocationInCitiesByCityIdAsync(cityId);
            return result.ToList();
        }
    }
}
