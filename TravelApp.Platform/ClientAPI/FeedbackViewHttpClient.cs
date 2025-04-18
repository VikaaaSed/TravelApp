﻿namespace TravelApp.Platform.ClientAPI
{
    public class FeedbackViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FeedbackViewHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public FeedbackViewHttpClient(HttpClient httpClient, ILogger<FeedbackViewHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation)
        {
            try
            {
                if (idLocation <= 0)
                {
                    _logger.LogWarning("Некорректный idLocation: {IdLocation}", idLocation);
                    return [];
                }

                string url = $"{BaseUrl}/Feedback/GetFeedbackByIdLocation?idLocation={idLocation}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении отзывов: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.FeedbackView>>() ?? new List<API.Models.FeedbackView>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetFeedbackByIdLocationAsync");
                throw;
            }
        }
    }
}
