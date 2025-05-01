using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class LocationGalleryHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<LocationGalleryHttpClient> _logger;
        public LocationGalleryHttpClient(HttpClient httpClient,
            ILogger<LocationGalleryHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.LocationGallery>> GetLocationGalleryByIdLocationAsync(int locationId)
        {
            try
            {
                string url = $"LocationGallery/GetGalleryByLocationId?locationId={locationId}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении картинок из галереии локации: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationGallery>>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetLocationGalleryByIdLocationAsync");
                throw;
            }
        }
        public async Task<API.Models.LocationGallery?> GetAsync(int id)
        {
            try
            {
                string url = $"LocationGallery/{id}";

                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении картинки из галереи локации: {response.StatusCode}");
                }
                return await response.Content.ReadFromJsonAsync<API.Models.LocationGallery>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAsync");
                throw;
            }
        }
        public async Task<API.Models.LocationGallery> CreateAsync(API.Models.LocationGallery gallery)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("LocationGallery", gallery);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при создании картинки в галереи локации: {response.StatusCode} - {responseContent}");
                }
                return JsonSerializer.Deserialize<API.Models.LocationGallery>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса CreateAsync");
                throw;
            }
        }
        public async Task UpdateAsync(API.Models.LocationGallery gallery)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("LocationGallery", gallery);
                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);

                    throw new HttpRequestException($"Ошибка при изменении картинки в галереи локации: {response.StatusCode} - {responseContent}");
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
                string url = $"LocationGallery/{id}";
                var response = await _httpClient.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Ошибка удаления картинки из галереи локации. Код: {StatusCode}, Ответ: {Content}",
                        response.StatusCode, content);

                    throw new HttpRequestException($"Ошибка удаления картинки из галереи локации: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteAsync");
                throw;
            }
        }
    }

}
