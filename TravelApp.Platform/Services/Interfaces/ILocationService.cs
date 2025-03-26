using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<LocationInHomePage> GetLocationInHomePageByPageNameAsync(string pageName);
    }
}
