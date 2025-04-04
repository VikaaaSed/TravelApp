using TravelApp.API.Models;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> RegistrationUserAsync(UserRegistration user);
        public Task<User> CreateUserAsync(UserRegistration user);
        public Task<User?> GetUserByEmailAsync(string email);
    }
}
