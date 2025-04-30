using System.Text.Json;


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
        public async Task<API.Models.Feedback> CreateFeedbackAsync(API.Models.Feedback feedback)
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
                return JsonSerializer.Deserialize<API.Models.Feedback>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }
        public async Task<API.Models.Feedback?> GetAsync(int id)
        {
            try
            {
                string url = $"{BaseUrl}/Feedback/Get/{id}";

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении отзыва: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<API.Models.Feedback>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAsync");
                throw;
            }
        }
        public async Task UpdateFeedbackAsync(API.Models.Feedback feedback)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Feedback", feedback);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при изменении отзыва: {response.StatusCode} - {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса UpdateFeedbackAsync");
                throw;
            }
        }

    }
}
