using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<LocationInHomePage> GetLocationInHomePageByPageNameAsync(string pageName);
        public Task<List<LocationGallery>> GetLocationGalleryByIdLocationAsync(int idLocation);
        public Task<List<FeedbackView>> GetFeedbackViewByIdLocationAsync(int idLocation);
        public Task<AllLocationInformation> GetAllLocationInformationAsync(string pageName, string token);
        public Task<Feedback> CreateFeedbackAsync(Feedback feedback);
        public Task<Feedback> CreateFeedbackAsync(Feedback feedback, string token);
        public string? GetUserFirstAndLastName(string token);
        public Task ChangeFavoriteLocation(string pageName, string token);
    }
}
