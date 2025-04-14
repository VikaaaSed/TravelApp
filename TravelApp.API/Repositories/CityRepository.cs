using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IDbContextFactory<CityContext> _context;
        private readonly ILogger<CityRepository> _logger;

        public CityRepository(IDbContextFactory<CityContext> context, ILogger<CityRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                return await context.Cities.OrderBy(n => n.Id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка всех городов");
                return Enumerable.Empty<City>();
            }
        }

        public async Task<City?> GetCityByPageNameAsync(string pageName)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                _logger.LogWarning("Попытка получить город с пустым PageName.");
                return null;
            }

            string normalizedPageName = pageName.Trim().ToLowerInvariant();
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                var city = await context.Cities.FirstOrDefaultAsync(n => n.PageName.ToLower() == normalizedPageName);

                if (city == null)
                    _logger.LogInformation("Город с PageName '{PageName}' не найден.", pageName);

                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении города по PageName: {PageName}", pageName);
                return null;
            }
        }

        public async Task<IEnumerable<City>> GetVisibleCityAsync()
        {
            await using var context = await _context.CreateDbContextAsync();

            try
            {
                return await context.Cities
                    .Where(n => n.PageVisible)
                    .OrderBy(n => n.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении видимых городов.");
                return Enumerable.Empty<City>();
            }
        }
        public async Task<City> CreateCityAsync(City city)
        {
            if (city == null)
            {
                _logger.LogWarning("Попытка создания города с пустым объектом.");
                throw new ArgumentNullException(nameof(city));
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                await context.Cities.AddAsync(city);
                await context.SaveChangesAsync();
                _logger.LogInformation("Город успешно создан. ID: {cityId}", city.Id);

                return city;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении города: {City}", city);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании города.");
                throw;
            }
        }
        public async Task UpdateCityAsync(City city)
        {
            if (city.Id <= 0)
            {
                _logger.LogWarning("Попытка обновить город с некорректным Id: {id}", city.Id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                var oldCity = await context.Cities.SingleOrDefaultAsync(u => u.Id == city.Id);
                if (oldCity == null)
                {
                    _logger.LogWarning("Попытка обновить несуществующий город с id: {id}", city.Id);
                    return;
                }
                oldCity.Title = city.Title;
                oldCity.Description = city.Description;
                oldCity.MainPictureLink = city.MainPictureLink;
                oldCity.PictureAtHomeLink = city.PictureAtHomeLink;
                oldCity.PageName = city.PageName;
                oldCity.PageVisible = city.PageVisible;
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений города с id: {id}", city.Id);
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных о городе по id: {id}", city.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteCityAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удалении город с некорректным Id: {id}", id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var city = await context.Cities.SingleOrDefaultAsync(c => c.Id == id);
                if (city == null)
                {
                    _logger.LogWarning("Попытка удаления несуществующего города с id: {id}", id);
                    return;
                }
                context.Cities.Remove(city);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при удалении города с id: {id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при удалении города по id: {id}", id);
                throw;
            }
        }
        public async Task<City?> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить город с некорректным Id: {id}", id);
                return null;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var city = await context.Cities.FirstOrDefaultAsync(c => c.Id == id);

                if (city == null)
                    _logger.LogInformation("Город с id '{id}' не найден.", id);
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении города по id: {id}", id);
                return null;
            }
        }

    }
}
