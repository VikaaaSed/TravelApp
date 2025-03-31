using TravelApp.API.Models;

namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public FeedbackHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            var response = await _httpClient.PostAsJsonAsync($"{xPathCity}/Feedback/Create", feedback);
            response.EnsureSuccessStatusCode();
            return feedback;
        }
    }
}
