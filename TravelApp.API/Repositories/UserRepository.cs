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
            user.Email = user.Email.Trim().ToLowerInvariant();
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                _logger.LogInformation("Пользователь создан: {Email}", user.Email);

                return user;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении пользователя с email: {Email}", user.Email);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании пользователя: {Email}", user.Email);
                throw;
            }
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            string normalizedEmail = email.Trim().ToLowerInvariant();
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя по email: {Email}", email);
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя по id: {Id}", id);
                return null;
            }
        }
        public async Task UpdateAsync(User user)
        {
            user.Email = user.Email.Trim().ToLowerInvariant();
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var oldUser = await context.Users.SingleOrDefaultAsync(u => u.Id == user.Id);
                if (oldUser != null)
                {
                    oldUser.FirstName = user.FirstName;
                    oldUser.LastName = user.LastName;
                    oldUser.UserType = user.UserType;
                    oldUser.Age = user.Age;
                    oldUser.PasswordHash = user.PasswordHash;
                    oldUser.Email = user.Email;
                    oldUser.LastIp = user.LastIp;
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений пользователя с id: {id}", user.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных о пользователе по id: {id}", user.Id);
                throw;
            }
        }

    }
}
