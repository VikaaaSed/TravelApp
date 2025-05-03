using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class LocationGalleryHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LocationGalleryHttpClient> _logger;

        public LocationGalleryHttpClient(HttpClient httpClient, ILogger<LocationGalleryHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.LocationGallery>> GetLocationGalleryByIdLocationAsync(int locationId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"LocationGallery/by-location/{locationId}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                await EnsureSuccessAsync(response, $"получении галереи по locationId={locationId}");

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.LocationGallery>>()
                       ?? throw new JsonException("Ошибка десериализации ответа");
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
                var response = await _httpClient.GetAsync($"LocationGallery/{id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

                await EnsureSuccessAsync(response, $"получении картинки по id={id}");

                return await response.Content.ReadFromJsonAsync<API.Models.LocationGallery>()
                       ?? throw new JsonException("Ошибка десериализации ответа");
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
                await EnsureSuccessAsync(response, "создании картинки");

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<API.Models.LocationGallery>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при вызове CreateAsync");
                throw;
            }
        }
        public async Task UpdateAsync(API.Models.LocationGallery gallery)
        {
            try
            {
                if (gallery.Id <= 0)
                    throw new ArgumentException("Id не может быть нулевым или отрицательным для обновления.");

                var response = await _httpClient.PutAsJsonAsync($"LocationGallery/{gallery.Id}", gallery);
                await EnsureSuccessAsync(response, $"обновлении картинки с id={gallery.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при вызове UpdateAsync");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"LocationGallery/{id}");
                await EnsureSuccessAsync(response, $"удалении картинки с id={id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при вызове DeleteAsync");
                throw;
            }
        }
        private async Task EnsureSuccessAsync(HttpResponseMessage response, string context)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogError("Ошибка при {Context}. Код: {StatusCode}, Ответ: {Content}", context, response.StatusCode, content);
                throw new HttpRequestException($"Ошибка при {context}: {response.StatusCode} - {content}");
            }
        }
    }
}
