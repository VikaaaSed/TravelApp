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
        private readonly IUserFollowerService _userFollowerService;

        public UserService(UserHttpClient userHttpClient, IClientIpService clientIpService,
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, FeedbackHttpClient feedbackHttpClient, 
            LocationHttpClient locationHttpClient, FavoriteLocationHttpClient favoriteLocationHttpClient, IUserFollowerService userFollowerService)
        {
            _userHttpClient = userHttpClient;
            _clientIpService = clientIpService;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _feedbackHttpClient = feedbackHttpClient;
            _locationHttpClient = locationHttpClient;
            _favoriteLocationHttpClient = favoriteLocationHttpClient;
            _userFollowerService = userFollowerService;
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

        public async Task<List<User>> GetAllAsync()
        {
            var result = await _userHttpClient.GetAllAsync();
            return result.ToList();
        }

        public async Task<List<Follower>> GetUserSubscriptionsAsync(int id)
        {
            var userTask = GetAllAsync();
            var subscriptionTask = _userFollowerService.GetByUserIdAsync(id);

            await Task.WhenAll(userTask, subscriptionTask);

            var userResult = userTask.Result;
            var subscriptionResult = subscriptionTask.Result;

            var result = subscriptionResult.Join(userResult, f => f.IdFollower, u => u.Id, (subscription, user)
                => new Follower(
                    subscription.Id,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.AvatarLink)
                ).ToList();
            return result;
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _userHttpClient.GetUserAsync(id);
        }

        public async Task<List<Follower>> GetUserFollowersAsync(int id)
        {
            var userTask = GetAllAsync();
            var followerTask = _userFollowerService.GetByFollowerIdAsync(id);

            await Task.WhenAll(userTask, followerTask);

            var userResult = userTask.Result;
            var followerResult = followerTask.Result;

            var result = followerResult.Join(userResult, f => f.IdUser, u => u.Id, (follower, user)
                => new Follower(
                    follower.Id,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.AvatarLink)
                ).ToList();
            return result;
        }
    }
}
