using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateUserAsync(UserRegistration user);
    }
}
