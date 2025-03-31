using System.Text.Json;
using TravelApp.API.Models;

namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeedbackHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public FeedbackHttpClient(HttpClient httpClient,
            ILogger<FeedbackHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Feedback/Create", feedback);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при создании отзыва: {response.StatusCode} - {responseContent}");
                }
                return JsonSerializer.Deserialize<Feedback>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }
    }
}
