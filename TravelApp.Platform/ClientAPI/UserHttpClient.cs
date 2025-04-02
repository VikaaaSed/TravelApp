using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class UserHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserHttpClient> _logger;
        private readonly string BaseUrl = "https://localhost:7040/api";
        public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<API.Models.User> CreateUserAsync(API.Models.User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/User/Create", user);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при создании пользователя: {response.StatusCode} - {responseContent}");
                }
                return JsonSerializer.Deserialize<API.Models.User>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }
    }
}
