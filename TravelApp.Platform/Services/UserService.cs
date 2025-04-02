using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class UserService : IUserService
    {
        private readonly UserHttpClient _userHttpClient;
        private readonly IClientIpService _clientIpService;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(UserHttpClient userHttpClient, IClientIpService clientIpService,
            IPasswordHasher passwordHasher)
        {
            _userHttpClient = userHttpClient;
            _clientIpService = clientIpService;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> CreateUserAsync(UserRegistration userRegistration)
        {
            if (userRegistration.Email == "" || userRegistration.Password != userRegistration.RepeatPassword)
                return false;
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
            var result = await _userHttpClient.CreateUserAsync(newUser);
            if (result == null)
                return false;
            return true;
        }
    }
}
