using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<User?> GetUserByIdAsync(int id);
        public Task UpdateAsync(User user);
        public Task<IEnumerable<User>> GetAllAsync();
    }
}
