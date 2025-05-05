using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class CityHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CityHttpClient> _logger;
        public CityHttpClient(HttpClient httpClient, ILogger<CityHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<API.Models.City>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("City");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                await EnsureSuccessAsync(response, "получения списка города");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.City>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }
        public async Task<API.Models.City?> GetCityByPageNameAsync(string pageName)
        {
            try
            {
                string url = "City/by-page";
                if (!string.IsNullOrWhiteSpace(pageName))
                    url += $"?pageName={Uri.EscapeDataString(pageName)}";

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                var responseContent = await response.Content.ReadAsStringAsync();

                await EnsureSuccessAsync(response, $"получения города по названию страницы pageName={pageName}");
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
                string url = $"City/{id}";
                var response = await _httpClient.GetAsync(url);

                await EnsureSuccessAsync(response, $"получения города по id={id}");
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
                string url = $"City/{id}";
                var response = await _httpClient.DeleteAsync(url);


                await EnsureSuccessAsync(response, $"удаления города по id={id}");
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
                var response = await _httpClient.PutAsJsonAsync($"City/{city.Id}", city);
                await EnsureSuccessAsync(response, $"обновления города по id={city.Id}");
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
                var response = await _httpClient.PostAsJsonAsync($"City", city);
                var responseContent = await response.Content.ReadAsStringAsync();
                await EnsureSuccessAsync(response, $"создания города");
                return JsonSerializer.Deserialize<API.Models.City>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса CreateCityAsync");
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
