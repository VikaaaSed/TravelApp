using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class CityHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CityHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public CityHttpClient(HttpClient httpClient, ILogger<CityHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<API.Models.City>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/City/GetAll");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении всех городов: {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.City>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.City>> GetVisibleCityAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/City/GetVisibleCity");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении видимых городов: {response.StatusCode}");
                }

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.City>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleCityAsync");
                throw;
            }
        }
        public async Task<API.Models.City?> GetCityByPageNameAsync(string pageName)
        {
            try
            {
                string url = $"{BaseUrl}/City/GetCityByPageName";
                if (!string.IsNullOrWhiteSpace(pageName))
                    url += $"?pageName={Uri.EscapeDataString(pageName)}";

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении города: {response.StatusCode}");
                }
                if (string.IsNullOrWhiteSpace(responseContent)) return null;
                return JsonSerializer.Deserialize<API.Models.City>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetCityByPageNameAsync");
                throw;
            }
        }

        public async Task<API.Models.City> GetCityAsync(int id)
        {
            try
            {
                string url = $"{BaseUrl}/City/{id}";
                var response = await _httpClient.GetAsync(url);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении города: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<API.Models.City>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetCityAsync");
                throw;
            }
        }
        public async Task DeleteCityAsync(int id)
        {
            try
            {
                string url = $"{BaseUrl}/City/{id}";
                var response = await _httpClient.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Ошибка удаления города. Код: {StatusCode}, Ответ: {Content}",
                        response.StatusCode, content);

                    throw new HttpRequestException($"Ошибка удаления города: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteAsync");
                throw;
            }
        }
        public async Task UpdateCityAsync(API.Models.City city)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/City", city);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при изменении города: {response.StatusCode} - {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса UpdateCityAsync");
                throw;
            }
        }
        public async Task<API.Models.City> CreateCityAsync(API.Models.City city)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/City", city);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при создании Города: {response.StatusCode} - {responseContent}");
                }
                return JsonSerializer.Deserialize<API.Models.City>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса CreateCityAsync");
                throw;
            }
        }

    }
}
