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
        public async Task<LocationGallery> CreateAsync(LocationGallery gallery)
        {
            if (gallery == null)
            {
                _logger.LogWarning("Попытка создания картинки в галереи локации с пустым объектом.");
                throw new ArgumentNullException(nameof(gallery));
            }
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                await context.Gallery.AddAsync(gallery);
                await context.SaveChangesAsync();
                _logger.LogInformation("Картинка в галереи локации успешно создан. ID: {id}", gallery.Id);

                return gallery;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении картинки в галереи локации: {Gallery}", gallery);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при создании картинки в галереи локации.");
                throw;
            }
        }
        public async Task UpdateAsync(LocationGallery gallery)
        {
            if (gallery.Id <= 0)
            {
                _logger.LogWarning("Попытка обновить картинку с некорректным Id: {id}", gallery.Id);
                return;
            }
            await using var context = await _context.CreateDbContextAsync();
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {

                var oldGallery = await context.Gallery.SingleOrDefaultAsync(l => l.Id == gallery.Id);
                if (oldGallery == null)
                {
                    _logger.LogWarning("Попытка обновить несуществующую картинку с id: {id}", gallery.Id);
                    return;
                }
                oldGallery.LocationId = gallery.LocationId;
                oldGallery.Title = gallery.Title;
                oldGallery.PictureLink = gallery.PictureLink;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Ошибка при сохранении изменений картинки в галерею локации с id: {id}", gallery.Id);
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при обновления данных о картинке в галереи локации по id: {id}", gallery.Id);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
