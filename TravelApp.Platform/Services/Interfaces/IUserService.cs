using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> RegistrationUserAsync(UserRegistration user);
        public Task<User> CreateUserAsync(UserRegistration user);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<string?> AuthorizationUserAsync(UserAuthorization user);
        public Task UpdateUserAsync(User user);
        public Task<User?> GetUserByTokenAsync(string token);
        public Task<List<UserFeedback>> GetUserFeedbackAsync(int id);
        public Task<List<FavoriteLocationItem>> GetFavoriteLocationsAsync(int id);
        public Task<List<User>> GetAllAsync();
        public Task<List<Follower>> GetUserFollowerAsync(int id);
    }
}
