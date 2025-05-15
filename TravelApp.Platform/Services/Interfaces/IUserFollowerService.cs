using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IUserFollowerService
    {
        public Task<UserFollower> GetAsync(int id);
        public Task<List<UserFollower>> GetByUserIdAsync(int id);
        public Task<UserFollower> CreateAsync(UserFollower Follower);
        public Task DeleteAsync(int id);
        public Task<List<UserFollower>> GetByFollowerIdAsync(int id);
    }
}
