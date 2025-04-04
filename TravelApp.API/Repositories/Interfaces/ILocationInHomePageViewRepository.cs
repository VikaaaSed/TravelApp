using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface ILocationInHomePageViewRepository
    {
        public Task<LocationInHomePage?> GetLocationByPageNameAsync(string pageName);
    }
}
