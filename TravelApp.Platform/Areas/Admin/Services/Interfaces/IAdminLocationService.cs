using TravelApp.API.Models;

namespace TravelApp.Platform.Areas.Admin.Services.Interfaces
{
    public interface IAdminLocationService
    {
        public Task<List<Location>> GetAllLocationsAsync();
    }
}
