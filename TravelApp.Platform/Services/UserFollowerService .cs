using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class UserFollowerService : IUserFollowerService
    {
        private readonly UserFollowerHttpClient _httpClient;

        public UserFollowerService(UserFollowerHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserFollower> CreateAsync(UserFollower Follower)
        {
            return await _httpClient.CreateAsync(Follower);
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync(id);
        }

        public async Task<UserFollower> GetAsync(int id)
        {
            return await _httpClient.GetAsync(id);
        }

        public async Task<List<UserFollower>> GetByUserIdAsync(int id)
        {
            var result = await _httpClient.GetByUserIdAsync(id);
            return result.ToList();
        }
    }
}
