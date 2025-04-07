using System.Net;
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
        public async Task<API.Models.User?> GetUserByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Пустой или некорректный email");
                    throw new ArgumentException("Email не может быть пустым.", nameof(email));
                }

                string url = $"{BaseUrl}/User/GetUserByEmail?email={Uri.EscapeDataString(email)}";
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Пользователь с email '{Email}' не найден.", email);
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("HTTP {StatusCode} при получении пользователя. Ответ: {Response}",
                        response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при получении пользователя: {response.StatusCode}");
                }

                var user = await response.Content.ReadFromJsonAsync<API.Models.User>();

                return user;
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Ошибка десериализации JSON при получении пользователя.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при выполнении запроса GetUserByEmailAsync.");
                throw;
            }
        }
        public async Task UpdateUserAsync(API.Models.User user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/User/Update", user);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Ошибка HTTP {StatusCode}: {ResponseContent}", response.StatusCode, responseContent);
                    throw new HttpRequestException($"Ошибка при обновлении пользователя: {response.StatusCode} - {responseContent}");
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }

    }
}
