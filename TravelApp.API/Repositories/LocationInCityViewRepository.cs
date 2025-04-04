using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class LocationInCityViewRepository : ILocationInCityViewRepository
    {
        private readonly IDbContextFactory<LocationInCityContext> _context;
        private readonly ILogger<LocationInCityViewRepository> _logger;
        public LocationInCityViewRepository(IDbContextFactory<LocationInCityContext> context,
            ILogger<LocationInCityViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<LocationInCity>> GetLocationInCityByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                _logger.LogWarning("Передан некорректный cityId: {CityId}", cityId);
                return Enumerable.Empty<LocationInCity>();
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {

                var locations = await context.Locations
                    .Where(n => n.CityId == cityId)
                    .OrderBy(n => n.Id)
                    .ToListAsync();

                if (!locations.Any())
                    _logger.LogInformation("Локации для города с id {CityId} не найдены.", cityId);

                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении локаций для города с id {CityId}", cityId);
                return Enumerable.Empty<LocationInCity>();
            }
        }
    }
}
