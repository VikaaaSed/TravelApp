using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class HomeService : IHomeService
    {
        private readonly CityViewHttpClient _cityViewHttpClient;
        public HomeService(CityViewHttpClient cityHttpClient)
        {
            _cityViewHttpClient = cityHttpClient;
        }
        public async Task<List<CityInHomePage>> GetAllCityAsync()
        {
            var result = await _cityViewHttpClient.GetAllAsync();
            return result.ToList();
        }
    }
}
