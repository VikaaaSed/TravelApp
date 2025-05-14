using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class UserFollowerRepository : IUserFollowerRepository
    {
        private readonly IDbContextFactory<UserFollowerContext> _context;
        private readonly ILogger<UserFollowerRepository> _logger;
        public UserFollowerRepository(IDbContextFactory<UserFollowerContext> context, ILogger<UserFollowerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserFollower> CreateAsync(UserFollower userFollower)
        {
            if (userFollower == null)
            {
                _logger.LogWarning("Попытка создания подписчика с пустым объектом.");
                throw new ArgumentNullException(nameof(userFollower));
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                await context.Followers.AddAsync(userFollower);
                await context.SaveChangesAsync();
                _logger.LogInformation("Подписчик успешно добавлен. id={favoriteLocationId}", userFollower.Id);

                return userFollower;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при добавлении подписчика: {favoriteLocation}", userFollower);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при добавлении подписчика.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления подписчика с id={id}", id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var follower = await context.Followers.SingleOrDefaultAsync(fl => fl.Id == id);
                if (follower == null)
                {
                    _logger.LogWarning("Попытка удаления несуществующей записи о подписчике с id={id}", id);
                    return;
                }
                context.Followers.Remove(follower);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при удалении записи о подписчике по id={id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении записи о подписчике по id: {id}", id);
                throw;
            }
        }

        public async Task<List<UserFollower>> GetAllAsync()
        {
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                return await context.Followers.OrderBy(n => n.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка всех записей о подписчиках");
                return [];
            }
        }

        public async Task<UserFollower?> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить запись о подписчике с некорректным od={id}", id);
                return null;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var follower = await context.Followers.FirstOrDefaultAsync(fl => fl.Id == id);

                if (follower == null)
                    _logger.LogInformation("Запись с id '{id}' не найден.", id);
                return follower;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении записи о подписчике по id: {id}", id);
                return null;
            }
        }

        public async Task<List<UserFollower>> GetByUserIdAsync(int idUser)
        {
            if (idUser <= 0)
            {
                _logger.LogWarning("Попытка получить запись о подписчике с некорректным idUser: {idUser}", idUser);
                return [];
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var followers = await context.Followers.Where(fl => fl.IdUser == idUser).ToListAsync() ?? [];


                if (followers.Count == 0)
                    _logger.LogInformation("Список подписчиков для пользователя с id '{id}' не найден.", idUser);

                return followers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка подписчиков для пользователя по id: {id}", idUser);
                return [];
            }
        }
    }
}
