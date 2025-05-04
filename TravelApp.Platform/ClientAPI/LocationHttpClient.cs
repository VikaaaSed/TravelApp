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

                await EnsureSuccessAsync(response, $"локации по id={id}");
                return await response.Content.ReadFromJsonAsync<API.Models.Location>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetAllAsync()
        {
            try
            {
                string url = "Location";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return Enumerable.Empty<API.Models.Location>();

                await EnsureSuccessAsync(response, $"получения списка локаций");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
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
                string url = "Location/visible";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return Enumerable.Empty<API.Models.Location>();

                await EnsureSuccessAsync(response, $"получения списка локаций для отображения");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetLocationByCityIdAsync(int id)
        {
            try
            {
                string url = $"Location/by-city/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return Enumerable.Empty<API.Models.Location>();

                await EnsureSuccessAsync(response, $"получения списка локаций по id города id={id}");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationByCityIdAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.Location>> GetVisibleLocationByCityIdAsync(int id)
        {
            try
            {
                string url = $"Location/visible/by-city/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return Enumerable.Empty<API.Models.Location>();

                await EnsureSuccessAsync(response, $"получения списка локаций для отображения по id города id={id}");


                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.Location>>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetVisibleLocationByCityIdAsync");
                throw;
            }
        }
        public async Task<API.Models.Location> CreateAsync(API.Models.Location location)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Location", location);
                await EnsureSuccessAsync(response, $"создания локации");

                return await response.Content.ReadFromJsonAsync<API.Models.Location>()
                    ?? throw new JsonException("Ошибка десериализации ответа");
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
                var response = await _httpClient.PutAsJsonAsync($"Location/{location.Id}", location);

                await EnsureSuccessAsync(response, $"обновления локации с id={location.Id}");
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
                await EnsureSuccessAsync(response, $"удаления локации с id={id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteAsync");
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
