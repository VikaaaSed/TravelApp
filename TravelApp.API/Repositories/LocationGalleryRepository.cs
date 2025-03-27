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
            => await _context.CreateDbContext().Gallery.Where(n => n.LocationId == locationId)
            .OrderBy(n => n.Id).ToListAsync();
    }
}
