using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class LocationViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LocationViewHttpClient> _logger;

        public LocationViewHttpClient(HttpClient httpClient, ILogger<LocationViewHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<API.Models.LocationInHomePage> GetLocationByPageNameAsync(string pageName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageName))
                {
                    _logger.LogWarning("Пустой или некорректный pageName");
                    throw new ArgumentException("pageName не может быть пустым");
                }

                string url = $"Location/by-page?pageName={Uri.EscapeDataString(pageName)}";
                var response = await _httpClient.GetAsync(url);
                await EnsureSuccessAsync(response, "получении локации по pageName");

                return await response.Content.ReadFromJsonAsync<API.Models.LocationInHomePage>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationByPageNameAsync");
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
