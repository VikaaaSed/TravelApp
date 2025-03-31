namespace TravelApp.Platform.ClientAPI
{
    public class CityViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CityViewHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public CityViewHttpClient(HttpClient httpClient, ILogger<CityViewHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.CityInHomePage>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/City/GetCityInHomePage");
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении всех представлений городов: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.CityInHomePage>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
    }
}
