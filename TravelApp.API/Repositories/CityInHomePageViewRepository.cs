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
        {
            await using var context = await _context.CreateDbContextAsync();
            try
            {
                var result = await context.Cities
                    .OrderBy(n => n.Id)
                    .ToListAsync();

                _logger.LogInformation("Получено {Count} городов для главной страницы.", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении городов для главной страницы.");
                return Enumerable.Empty<CityInHomePage>();
            }
        }
    }
}
