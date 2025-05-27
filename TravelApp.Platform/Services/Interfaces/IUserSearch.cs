using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IUserSearch
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByTokenAsync(string token);
        public Task<UserFollower> AddFollowersAsync(UserFollower follower);
        public Task DeleteFollowersAsync(int id);
        public Task<List<User>> GetAllAsync();
    }
}
