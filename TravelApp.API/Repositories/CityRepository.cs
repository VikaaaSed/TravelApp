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
            => await _context.CreateDbContext().Cities.ToListAsync();
        public async Task<IEnumerable<City>> GetVisibleCityAsync()
            => await _context.CreateDbContext().Cities.Where(n => n.PageVisible).ToListAsync();
    }
}
