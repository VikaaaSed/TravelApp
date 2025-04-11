using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.ClientAPI;

namespace TravelApp.Platform.Areas.Admin.Services
{
    public class AdminCityService : IAdminCityService
    {
        private readonly CityHttpClient _cityHttpClient;
        public AdminCityService(CityHttpClient cityHttpClient)
        {
            _cityHttpClient = cityHttpClient;
        }
        public async Task<List<City>> GetAllCityAsync()
        {
            var result = await _cityHttpClient.GetAllAsync();
            return result.ToList();
        }
    }
}
