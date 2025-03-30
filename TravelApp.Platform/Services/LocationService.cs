using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class LocationService : ILocationService
    {
        private readonly LocationViewHttpClient _locationViewHttpClient;
        private readonly LocationGalleryHttpClient _locationGalleryHttpClient;
        private readonly FeedbackViewHttpClient _feedbackViewHttpClient;
        public LocationService(LocationViewHttpClient locationViewHttpClient,
            LocationGalleryHttpClient locationGalleryHttpClient,
            FeedbackViewHttpClient feedbackViewHttpClient)
        {
            _locationViewHttpClient = locationViewHttpClient;
            _locationGalleryHttpClient = locationGalleryHttpClient;
            _feedbackViewHttpClient = feedbackViewHttpClient;
        }

        public async Task<AllLocationInformation> GetAllLocationInformationAsync(string pageName)
        {
            LocationInHomePage location = await GetLocationInHomePageByPageNameAsync(pageName);
            List<LocationGallery> gallery = await GetLocationGalleryByIdLocationAsync(location.Id);
            List<FeedbackView> feedbacks = await GetFeedbackViewByIdLocationAsync(location.Id);
            return new AllLocationInformation(location, gallery, feedbacks);
        }

        public async Task<List<FeedbackView>> GetFeedbackViewByIdLocationAsync(int idLocation)
        {
            var result = await _feedbackViewHttpClient.GetFeedbackByIdLocationAsync(idLocation);
            return result.ToList();
        }

        public async Task<List<LocationGallery>> GetLocationGalleryByIdLocationAsync(int idLocation)
        {
            var result = await _locationGalleryHttpClient.GetLocationGalleryByIdLocationAsync(idLocation);
            return result.ToList();
        }

        public async Task<LocationInHomePage> GetLocationInHomePageByPageNameAsync(string pageName)
            => await _locationViewHttpClient.GetLocationByPageNameAsync(pageName);
    }
}
