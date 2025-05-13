using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class FavoriteLocationHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FavoriteLocationHttpClient> _logger;

        public FavoriteLocationHttpClient(HttpClient httpClient, ILogger<FavoriteLocationHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<IEnumerable<API.Models.FavoriteLocation>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("FavoriteLocation");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                await EnsureSuccessAsync(response, "получения списка избранных локаций");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.FavoriteLocation>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
                throw;
            }
        }

        public async Task<API.Models.FavoriteLocation> GetAsync(int id)
        {
            try
            {
                string url = $"FavoriteLocation/{id}";
                var response = await _httpClient.GetAsync(url);

                await EnsureSuccessAsync(response, $"получения избранной локации по id={id}");
                return await response.Content.ReadFromJsonAsync<API.Models.FavoriteLocation>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAsync");
                throw;
            }
        }

        public async Task<IEnumerable<API.Models.FavoriteLocation>> GetByUserIdAsync(int idUser)
        {
            try
            {
                string url = $"/api/users/{idUser}/favorites";

                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];

                var responseContent = await response.Content.ReadAsStringAsync();

                await EnsureSuccessAsync(response, $"получения списка избранных локаций пользователем с id={idUser}");
                if (string.IsNullOrWhiteSpace(responseContent)) return [];

                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.FavoriteLocation>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetByUserIdAsync");
                throw;
            }
        }

        public async Task<API.Models.FavoriteLocation> CreateAsync(API.Models.FavoriteLocation favoriteLocation)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"FavoriteLocation", favoriteLocation);
                var responseContent = await response.Content.ReadAsStringAsync();
                await EnsureSuccessAsync(response, $"создания избранной локации");
                return JsonSerializer.Deserialize<API.Models.FavoriteLocation>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса CreateAsync");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                string url = $"FavoriteLocation/{id}";
                var response = await _httpClient.DeleteAsync(url);

                await EnsureSuccessAsync(response, $"удаления избранной локации по id={id}");
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
