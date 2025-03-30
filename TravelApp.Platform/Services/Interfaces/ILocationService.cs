using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<LocationInHomePage> GetLocationInHomePageByPageNameAsync(string pageName);
        public Task<List<LocationGallery>> GetLocationGalleryByIdLocationAsync(int id);
        public Task<AllLocationInformation> GetAllLocationInformationAsync(string pageName);
        public Task<List<FeedbackView>> GetFeedbackViewByIdLocationAsync(int idLocation);
    }
}
