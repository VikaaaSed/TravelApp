using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class CityService : ICityService
    {
        private readonly CityHttpClient _cityHttpClient;
        public CityService(CityHttpClient cityHttpClient)
        {
            _cityHttpClient = cityHttpClient;
        }
        public async Task<City> GetCityByPageNameAsync(string pageName)
            => await _cityHttpClient.GetCityByPageNameAsync(pageName);
    }
}
