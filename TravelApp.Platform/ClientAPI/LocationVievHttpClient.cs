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

        public async Task<IEnumerable<API.Models.LocationInCity>> GetLocationInCitiesByCityIdAsync(int cityId)
        {
            try
            {
                if (cityId <= 0)
                {
                    _logger.LogWarning("Некорректный cityId: {CityId}", cityId);
                    return [];
                }

                string url = $"Location/GetLocationsViewsByCityId?cityId={cityId}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении представления локаций: {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationInCity>>() ?? new List<API.Models.LocationInCity>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationInCitiesByCityIdAsync");
                throw;
            }
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

                string url = $"Location/GetLocationByPageName?pageName={Uri.EscapeDataString(pageName)}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении представления локации: {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<API.Models.LocationInHomePage>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationByPageNameAsync");
                throw;
            }
        }
    }
}
