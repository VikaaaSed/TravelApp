using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class FeedBackService : IFeedBackUser
    {
        private readonly FeedbackHttpClient _httpClient;

        public FeedBackService(FeedbackHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Feedback>> GetFeedbackByUserId(int UserId)
        {
            var Feedback = await _httpClient.GetAllAsync();
            return Feedback.Where(f => f.IdUser == UserId).ToList();
        }
    }
}
