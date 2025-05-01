using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class LocationHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LocationHttpClient> _logger;
        public LocationHttpClient(HttpClient httpClient, ILogger<LocationHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<API.Models.Location?> GetAsync(int id)
        {
            try
            {
                string url = $"Location/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении локации: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<API.Models.Location>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetLocationByCityIdAsync(int id)
        {
            try
            {
                string url = $"Location/GetLocationsByCityId/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении локации: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationByCityIdAsync");
                throw;
            }
        }
        public async Task<API.Models.Location> CreateAsync(API.Models.Location location)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Location", location);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при создании локации: {response.StatusCode} - {responseContent}");
                }
                return JsonSerializer.Deserialize<API.Models.Location>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса CreateAsync");
                throw;
            }
        }
        public async Task UpdateAsync(API.Models.Location location)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("Location", location);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при изменении локации: {response.StatusCode} - {responseContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса UpdateAsync");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                string url = $"Location/{id}";
                var response = await _httpClient.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Ошибка удаления локации. Код: {StatusCode}, Ответ: {Content}",
                        response.StatusCode, content);

                    throw new HttpRequestException($"Ошибка удаления локации: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetAllAsync()
        {
            try
            {
                string url = "Location/GetAll";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении локаций: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetVisibleAsync()
        {
            try
            {
                string url = "Location/GetVisibleLocations";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении локаций: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetVisibleLocationByCityIdAsync(int id)
        {
            try
            {
                string url = $"Location/GetVisibleLocationsByCityId/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении локации: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleLocationByCityIdAsync");
                throw;
            }
        }
    }
}
