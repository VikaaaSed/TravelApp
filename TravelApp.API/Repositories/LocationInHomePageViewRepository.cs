using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class LocationInHomePageViewRepository
        : ILocationInHomePageViewRepository
    {
        private readonly IDbContextFactory<LocationInHomePageContext> _context;
        private readonly ILogger<LocationInHomePageViewRepository> _logger;
        public LocationInHomePageViewRepository(IDbContextFactory<LocationInHomePageContext> context,
            ILogger<LocationInHomePageViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<LocationInHomePage> GetLocationByPageNameAsync(string pageName)
            => await _context.CreateDbContext().Locations.FirstAsync(n => n.PageName == pageName);
    }
}
