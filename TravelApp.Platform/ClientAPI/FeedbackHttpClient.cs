using System.Text;
using System.Text.Json;


namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeedbackHttpClient> _logger;
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
                var response = await _httpClient.PostAsJsonAsync("Feedback", feedback);
                var responseContent = await response.Content.ReadAsStringAsync();
                await EnsureSuccessAsync(response, $"создание отзыва");
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
                string url = $"Feedback/{id}";

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                await EnsureSuccessAsync(response, $"получения отзыва по id={id}");
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
                var response = await _httpClient.PutAsJsonAsync($"Feedback/{feedback.Id}", feedback);
                await EnsureSuccessAsync(response, $"обновление отзыва");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса UpdateFeedbackAsync");
                throw;
            }
        }
        public async Task AcceptedFeedbackAsync(int id)
        {
            try
            {
                string url = $"Feedback/{id}/accept";
                var response = await _httpClient.PatchAsync(url, new StringContent("{}", Encoding.UTF8, "application/json"));
                await EnsureSuccessAsync(response, $"подтверждении отзыва");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса AcceptedFeedbackAsync");
                throw;
            }
        }
        public async Task DeleteFeedbackAsync(int id)
        {
            try
            {
                string url = $"Feedback/{id}";
                var response = await _httpClient.DeleteAsync(url);
                await EnsureSuccessAsync(response, $"удаления отзыва");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteFeedbackAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Feedback>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("Feedback");
                await EnsureSuccessAsync(response, $"при получении всех отзывов");

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Feedback>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
        private async Task EnsureSuccessAsync(HttpResponseMessage response, string context)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("Ошибка при {Context}. Код: {StatusCode}, Ответ: {Content}", context, response.StatusCode, content);
                throw new HttpRequestException($"Ошибка {context}: {response.StatusCode} - {content}");
            }
        }
    }
}
