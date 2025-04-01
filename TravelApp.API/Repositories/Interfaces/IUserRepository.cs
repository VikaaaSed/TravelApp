using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);
    }
}
