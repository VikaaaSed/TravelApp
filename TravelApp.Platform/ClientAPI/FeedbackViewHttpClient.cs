namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeedbackViewHttpClient> _logger;
        public FeedbackViewHttpClient(HttpClient httpClient, ILogger<FeedbackViewHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation, bool? accepted = null)
        {
            try
            {
                if (idLocation <= 0)
                {
                    _logger.LogWarning("Некорректный idLocation: {IdLocation}", idLocation);
                    return [];
                }

                string url = $"/api/locations/{idLocation}/feedbacks";
                if (accepted is not null)
                    url += $"?accepted={accepted.Value.ToString().ToLower()}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];
                await EnsureSuccessAsync(response, $"при получении списка отзывов по idLocation {idLocation}");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.FeedbackView>>() ?? new List<API.Models.FeedbackView>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetFeedbackByIdLocationAsync");
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
