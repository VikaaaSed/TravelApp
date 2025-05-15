using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IUserFollowerRepository
    {
        public Task<UserFollower?> GetAsync(int id);
        public Task<List<UserFollower>> GetAllAsync();
        public Task<List<UserFollower>> GetByUserIdAsync(int idUser);
        public Task<UserFollower> CreateAsync(UserFollower userFollower);
        public Task DeleteAsync(int id);
        public Task<List<UserFollower>> GetByFollowerIdAsync(int idFollower);
    }
}
