using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class FavoriteLocationRepository : IFavoriteLocationRepository
    {
        private readonly IDbContextFactory<FavoriteLocationContext> _context;
        private readonly ILogger<FavoriteLocationRepository> _logger;

        public FavoriteLocationRepository(IDbContextFactory<FavoriteLocationContext> context,
            ILogger<FavoriteLocationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<FavoriteLocation> CreateAsync(FavoriteLocation favoriteLocation)
        {
            if (favoriteLocation == null)
            {
                _logger.LogWarning("Попытка создания записи о избранной локации с пустым объектом.");
                throw new ArgumentNullException(nameof(favoriteLocation));
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                await context.FavoriteLocations.AddAsync(favoriteLocation);
                await context.SaveChangesAsync();
                _logger.LogInformation("Запись о избранной локации успешно создана. ID: {favoriteLocationId}", favoriteLocation.Id);

                return favoriteLocation;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении записи о избранной локации: {favoriteLocation}", favoriteLocation);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании записи о избранной локации.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления записи о избранной локации с некорректным Id: {id}", id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var favoriteLocation = await context.FavoriteLocations.SingleOrDefaultAsync(fl => fl.Id == id);
                if (favoriteLocation == null)
                {
                    _logger.LogWarning("Попытка удаления несуществующей записи о избранной локации с id: {id}", id);
                    return;
                }
                context.FavoriteLocations.Remove(favoriteLocation);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при удалении записи о избранной локации с id: {id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении записи о избранной локации по id: {id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<FavoriteLocation>> GetAllAsync()
        {
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                return await context.FavoriteLocations.OrderBy(n => n.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка всех избранных локаций");
                return [];
            }
        }

        public async Task<FavoriteLocation> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить запись о избранной локации с некорректным Id: {id}", id);
                return null;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var favoriteLocation = await context.FavoriteLocations.FirstOrDefaultAsync(fl => fl.Id == id);

                if (favoriteLocation == null)
                    _logger.LogInformation("Запись с id '{id}' не найден.", id);
                return favoriteLocation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении записи о избранной локации по id: {id}", id);
                return null;
            }
        }


        public async Task<IEnumerable<FavoriteLocation>> GetByUserIdAsync(int idUser)
        {
            if (idUser <= 0)
            {
                _logger.LogWarning("Попытка получить запись о избранной локации с некорректным idUser: {idUser}", idUser);
                return [];
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var favoriteLocation = await context.FavoriteLocations.Where(fl => fl.IdUser == idUser).ToListAsync() ?? [];

                if (favoriteLocation.Count == 0)
                    _logger.LogInformation("Избранные локации для пользователя с id '{id}' не найден.", idUser);

                return favoriteLocation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении избранных локаций для пользователя по id: {id}", idUser);
                return [];
            }
        }

    }
}
