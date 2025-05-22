using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ILocationByUser
    {
        public Task<List<Location>> GetAllAsync();
    }
}
