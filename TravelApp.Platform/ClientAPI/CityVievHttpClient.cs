namespace TravelApp.Platform.ClientAPI
{
    public class CityViewHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CityViewHttpClient> _logger;
        public CityViewHttpClient(HttpClient httpClient, ILogger<CityViewHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.CityInHomePage>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("City/in-home-page");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                await EnsureSuccessAsync(response, "получения списка города для домашней страницы");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.CityInHomePage>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.CityInHomePage>> GetVisibleAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("City/in-home-page/visible");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                await EnsureSuccessAsync(response, "получения списка города для отображения на домашней страницы");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.CityInHomePage>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleAsync");
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
