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
        private readonly IJwtTokenService _jwtTokenService;
        public UserService(UserHttpClient userHttpClient, IClientIpService clientIpService,
            IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _userHttpClient = userHttpClient;
            _clientIpService = clientIpService;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<User> CreateUserAsync(UserRegistration userRegistration)
        {
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
            return await _userHttpClient.CreateUserAsync(newUser);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
            => await _userHttpClient.GetUserByEmailAsync(email);

        public async Task<bool> RegistrationUserAsync(UserRegistration user)
        {
            var result = await GetUserByEmailAsync(user.Email);
            if (result != null) return false;
            await CreateUserAsync(user);
            return true;
        }
        public async Task<string?> AuthorizationUserAsync(UserAuthorization user)
        {
            var result = await GetUserByEmailAsync(user.Email);
            if (result != null && _passwordHasher.VerifyPassword(user.Password, result.PasswordHash))
            {
                result.LastIp = _clientIpService.GetClientIp();
                await UpdateUserAsync(result);
                return _jwtTokenService.GenerateToken(result.Email, result.UserType);
            }
            return null;
        }


        public async Task UpdateUserAsync(User user)
            => await _userHttpClient.UpdateUserAsync(user);
    }
}
