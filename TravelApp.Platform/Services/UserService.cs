﻿using TravelApp.API.Models;
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
        public async Task<User?> AuthorizationUserAsync(UserAuthorization user)
        {
            var result = await GetUserByEmailAsync(user.Email);
            if (result != null && _passwordHasher.VerifyPassword(user.Password, result.PasswordHash))
                return result;
            return null;
        }
    }
}
