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

    }
}
