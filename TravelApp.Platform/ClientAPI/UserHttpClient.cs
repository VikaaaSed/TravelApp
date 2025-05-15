using System.Net;
using System.Text.Json;

namespace TravelApp.Platform.ClientAPI
{
    public class UserHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserHttpClient> _logger;
        public UserHttpClient(HttpClient httpClient, ILogger<UserHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<API.Models.User> CreateUserAsync(API.Models.User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("User", user);
                var responseContent = await response.Content.ReadAsStringAsync();
                await EnsureSuccessAsync(response, "создании пользователя");
                return JsonSerializer.Deserialize<API.Models.User>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }
        public async Task UpdateUserAsync(API.Models.User user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"User/{user.Id}", user);
                var responseContent = await response.Content.ReadAsStringAsync();
                await EnsureSuccessAsync(response, "обновлении пользователя");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при отправке запроса");
                throw;
            }
        }
        public async Task DeleteUserAsync(int id)
        {
            try
            {
                string url = $"User/{id}";
                var response = await _httpClient.DeleteAsync(url);

                await EnsureSuccessAsync(response, $"удаление пользователя по id={id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса DeleteAsync");
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

                string url = $"User/by-email?email={Uri.EscapeDataString(email)}";
                var response = await _httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Пользователь с email '{Email}' не найден.", email);
                    return null;
                }

                await EnsureSuccessAsync(response, "получении пользователя по email пользователя");

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
        public async Task<API.Models.User?> GetUserAsync(int id)
        {
            try
            {
                string url = $"User/{id}";
                var response = await _httpClient.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Пользователь с id '{id}' не найден.", id);
                    return null;
                }
                await EnsureSuccessAsync(response, $"получение пользователя по id={id}");
                return await response.Content.ReadFromJsonAsync<API.Models.User?>() ?? throw new JsonException("Ошибка десериализации ответа");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetUserAsync");
                throw;
            }
        }
        public async Task<IEnumerable<API.Models.User>> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("User");
                if (response.StatusCode == HttpStatusCode.NotFound) return [];


                await EnsureSuccessAsync(response, "получение списка пользователей");
                return await response.Content.ReadFromJsonAsync<IEnumerable<API.Models.User>>() ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении запроса GetAllAsync");
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
