namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string xPathCity = "https://localhost:7040/api";
        public FeedbackViewHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<API.Models.FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation)
        {
            string url = $"{xPathCity}/Feedback/GetFeedbackByIdLocation";

            if (idLocation != 0)
                url += $"?idLocation={idLocation}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.FeedbackView>>();
        }
    }
}
