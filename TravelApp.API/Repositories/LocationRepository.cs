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
        public async Task<Location> CreateAsync(Location location)
        {
            if (location == null)
            {
                _logger.LogWarning("Попытка создания локации с пустым объектом.");
                throw new ArgumentNullException(nameof(location));
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                await context.Locations.AddAsync(location);
                await context.SaveChangesAsync();
                _logger.LogInformation("Локация успешно создан. ID: {cityId}", location.Id);

                return location;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении локации: {City}", location);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании локации.");
                throw;
            }
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
        public async Task<IEnumerable<Location>> GetLocationByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                _logger.LogWarning("Попытка получить список локаций с некорректным Id города: {id}", cityId);
                return [];
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                IEnumerable<Location> city = await context.Locations.Where(l => l.IdCity == cityId).OrderBy(l => l.Id).ToListAsync() ?? [];

                if (city.Count() == 0)
                    _logger.LogInformation("Не удалось найти локации по указанному id города {cityId}", cityId);
                return city;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении Локацию по id города: {id}", cityId);
                return [];
            }
        }
        public async Task UpdateAsync(Location location)
        {
            if (location.Id <= 0)
            {
                _logger.LogWarning("Попытка обновить локацию. с некорректным Id: {id}", location.Id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                var oldLocation = await context.Locations.SingleOrDefaultAsync(l => l.Id == location.Id);
                if (oldLocation == null)
                {
                    _logger.LogWarning("Попытка обновить несуществующую локацию с id: {id}", location.Id);
                    return;
                }
                oldLocation.IdCity = location.IdCity;
                oldLocation.Title = location.Title;
                oldLocation.Description = location.Description;
                oldLocation.Address = location.Address;
                oldLocation.WorkSchedule = location.WorkSchedule;
                oldLocation.TicketLink = location.TicketLink;
                oldLocation.PictureInCityLink = location.PictureInCityLink;
                oldLocation.PicturePageLink = location.PicturePageLink;
                oldLocation.PageName = location.PageName;
                oldLocation.Rating = location.Rating;
                oldLocation.PageVisible = location.PageVisible;
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений локации с id: {id}", location.Id);
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных о локации по id: {id}", location.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
