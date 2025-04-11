using TravelApp.API.Models;

namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface IAdminCityService
    {
        public Task<List<City>> GetAllCityAsync();
    }
}
