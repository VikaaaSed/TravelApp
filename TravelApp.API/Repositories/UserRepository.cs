using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<UserContext> _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IDbContextFactory<UserContext> context,
            ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<User> CreateAsync(User user)
        {
            var context = await _context.CreateDbContextAsync();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
