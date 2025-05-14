using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class UserService : IUserService
    {
        private readonly UserHttpClient _userHttpClient;
        private readonly FeedbackHttpClient _feedbackHttpClient;
        private readonly IClientIpService _clientIpService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly LocationHttpClient _locationHttpClient;
        private readonly FavoriteLocationHttpClient _favoriteLocationHttpClient;

        public UserService(UserHttpClient userHttpClient, IClientIpService clientIpService,
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, FeedbackHttpClient feedbackHttpClient, 
            LocationHttpClient locationHttpClient, FavoriteLocationHttpClient favoriteLocationHttpClient)
        {
            _userHttpClient = userHttpClient;
            _clientIpService = clientIpService;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _feedbackHttpClient = feedbackHttpClient;
            _locationHttpClient = locationHttpClient;
            _favoriteLocationHttpClient = favoriteLocationHttpClient;
        }

        public async Task<string?> AuthorizationUserAsync(UserAuthorization user)
        {
            var result = await GetUserByEmailAsync(user.Email);
            if (result != null && _passwordHasher.VerifyPassword(user.Password, result.PasswordHash))
            {
                result.LastIp = _clientIpService.GetClientIp();
                await UpdateUserAsync(result);
                return _jwtTokenService.GenerateToken(result.Email, result.UserType, result.Id, $"{result.FirstName} {result.LastName}");
            }
            return null;
        }

        public async Task<User> CreateUserAsync(UserRegistration userRegistration)
        {
            var newUser = new User
            {
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                Email = userRegistration.Email,
                Age = userRegistration.Age,
                PasswordHash = _passwordHasher.HashPassword(userRegistration.Password),
                RegistrationIp = _clientIpService.GetClientIp(),
                LastIp = _clientIpService.GetClientIp()
            };
            return await _userHttpClient.CreateUserAsync(newUser);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _userHttpClient.GetUserByEmailAsync(email);

        public async Task<User?> GetUserByTokenAsync(string token)
        {
            string email = _jwtTokenService.GetUserEmailFromToken(token) ?? "";
            if (!string.IsNullOrEmpty(email))
                return await _userHttpClient.GetUserByEmailAsync(email) ?? null;
            return null;
        }

        public async Task<bool> RegistrationUserAsync(UserRegistration user)
        {
            var result = await GetUserByEmailAsync(user.Email);
            if (result != null) return false;
            await CreateUserAsync(user);
            return true;
        }

        public async Task UpdateUserAsync(User user)
            => await _userHttpClient.UpdateUserAsync(user);

        public async Task<List<UserFeedback>> GetUserFeedbackAsync(int id)
        {
            var feedbacksTask = _feedbackHttpClient.GetAllAsync();
            var locationsTask = _locationHttpClient.GetAllAsync();

            await Task.WhenAll(feedbacksTask, locationsTask);

            var feedbacksResult = feedbacksTask.Result;
            var locationResult = locationsTask.Result;

            List<UserFeedback> feedbacks = feedbacksResult.Where(f => f.IdUser == id)
                .Join(locationResult, f => f.IdLocation, l => l.Id, (feedback, location)
                => new UserFeedback(
                    location.Title,
                    location.PageName,
                    feedback.Text,
                    feedback.Ball
                )).ToList();
            return feedbacks;
        }

        public async Task<List<FavoriteLocationItem>> GetFavoriteLocationsAsync(int id)
        {
            var rFL = await _favoriteLocationHttpClient.GetByUserIdAsync(id);
            var rL = await _locationHttpClient.GetAllAsync();
            var result = rFL.Join(rL, f => f.IdLocation, l => l.Id, (favorite, location) 
                => new FavoriteLocationItem(favorite.Id, location.Id, location.PageName, location.Title));
            return result.ToList();
        }
    }
}
