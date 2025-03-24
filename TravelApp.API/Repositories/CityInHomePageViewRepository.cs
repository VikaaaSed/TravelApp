using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class CityInHomePageViewRepository : ICityInHomePageViewRepository
    {

        private readonly IDbContextFactory<CityInHomePageContext> _context;
        private readonly ILogger<CityInHomePageViewRepository> _logger;
        public CityInHomePageViewRepository(IDbContextFactory<CityInHomePageContext> context,
            ILogger<CityInHomePageViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<CityInHomePage>> GetAllAsync()
            => await _context.CreateDbContext().Cities.ToListAsync();
    }
}
