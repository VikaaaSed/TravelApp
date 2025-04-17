using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IDbContextFactory<LocationContext> _context;
        private readonly ILogger<LocationRepository> _logger;
        public LocationRepository(IDbContextFactory<LocationContext> context, ILogger<LocationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Location?> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить локацию с некорректным Id: {id}", id);
                return null;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var city = await context.Locations.FirstOrDefaultAsync(c => c.Id == id);

                if (city == null)
                    _logger.LogInformation("Локацию с id '{id}' не найден.", id);
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении Локацию по id: {id}", id);
                return null;
            }
        }
    }

}
