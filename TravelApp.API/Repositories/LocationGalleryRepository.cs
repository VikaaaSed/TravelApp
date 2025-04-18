using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class LocationGalleryRepository : ILocationGalleryRepository
    {
        private readonly IDbContextFactory<LocationGalleryContext> _context;
        private readonly ILogger<LocationGalleryRepository> _logger;

        public LocationGalleryRepository(IDbContextFactory<LocationGalleryContext> context,
            ILogger<LocationGalleryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<LocationGallery>> GetGalleryByIdLocationAsync(int locationId)
        {
            if (locationId <= 0)
            {
                _logger.LogWarning("Передан некорректный locationId: {LocationId}", locationId);
                return Enumerable.Empty<LocationGallery>();
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {
                var galleryItems = await context.Gallery
                    .Where(n => n.LocationId == locationId)
                    .OrderBy(n => n.Id)
                    .ToListAsync();

                if (!galleryItems.Any())
                    _logger.LogInformation("Галерея для локации с id {LocationId} не найдена.", locationId);

                return galleryItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении галереи для локации с id {LocationId}", locationId);
                return Enumerable.Empty<LocationGallery>();
            }
        }
        public async Task<LocationGallery?> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка получить информацию о картинке из галереи локации с некорректным Id: {id}", id);
                return null;
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var gallery = await context.Gallery.FirstOrDefaultAsync(c => c.Id == id);

                if (gallery == null)
                    _logger.LogInformation("Картинке из галереи локации с id '{id}' не найден.", id);
                return gallery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении картинки из галереи локации по id: {id}", id);
                return null;
            }
        }
    }
}
