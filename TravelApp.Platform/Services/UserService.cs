using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class UserService : IUserService, IUserSearch
    {
        private readonly UserHttpClient _userHttpClient;
        private readonly IFeedBackUser _feedBackUser;
        private readonly IClientIpService _clientIpService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILocationByUser _location;
        private readonly FavoriteLocationHttpClient _favoriteLocationHttpClient;
        private readonly IUserFollowerService _userFollowerService;
        private readonly IRecommendationService _recommendationService;

       public UserService(UserHttpClient userHttpClient, IClientIpService clientIpService,
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IFeedBackUser feedBackUser,
            ILocationByUser location, FavoriteLocationHttpClient favoriteLocationHttpClient, 
            IUserFollowerService userFollowerService, IRecommendationService recommendationService)
        {
            _userHttpClient = userHttpClient;
            _clientIpService = clientIpService;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _feedBackUser = feedBackUser;
            _location = location;
            _favoriteLocationHttpClient = favoriteLocationHttpClient;
            _userFollowerService = userFollowerService;
            _recommendationService = recommendationService;
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
            var feedbacksTask = _feedBackUser.GetFeedbackByUserId(id);
            var locationsTask = _location.GetAllAsync();

            await Task.WhenAll(feedbacksTask, locationsTask);

            var feedbacksResult = feedbacksTask.Result;
            var locationResult = locationsTask.Result;

            List<UserFeedback> feedbacks = feedbacksResult
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
            var rL = await _location.GetAllAsync();
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
        public async Task<List<RecommendedItem>> GetUserRecommendation(int id)
        {
            var idCitesTask = getUserIdCity(id);

            var followersTask = GetUserFollowersAsync(id);
            await Task.WhenAll(idCitesTask, followersTask);

            var idCites = idCitesTask.Result;
            var followers = followersTask.Result.Select(f => f.id).ToList();

            var followersIdCites = await getFollowersIdCity(followers);

            return await _recommendationService.GetRecommendedAsync(new RecommendationModel(id, idCites, followersIdCites));
        }
        private async Task<List<int>> getUserIdCity(int id)
        {
            List<Feedback> location = await _feedBackUser.GetFeedbackByUserId(id);
            List<int> res = location.DistinctBy(l => l.IdLocation).Select(s => s.IdLocation).ToList();

            List<int> idCites = new List<int>();
            foreach (var item in res)
            {
                var loc = await _location.GetAsync(item);
                if (loc != null && loc.PageVisible && !idCites.Contains(loc.IdCity))
                    idCites.Add(loc.IdCity);
            }
            return idCites;
        }
        private async Task<Dictionary<int, List<int>>> getFollowersIdCity(List<int> followers)
        {
            Dictionary<int, List<int>> result = [];

            foreach (var item in followers)
                result.Add(item, await getUserIdCity(item));

            return result;
        }

        public Task<UserFollower> AddFollowersAsync(UserFollower follower)
            => _userFollowerService.CreateAsync(follower);        

        public Task DeleteFollowersAsync(int id)
            => _userFollowerService.DeleteAsync(id);
        
    }
}
