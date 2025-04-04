using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class FeedbackViewRepository : IFeedbackViewRepository
    {
        private readonly IDbContextFactory<FeedbackViewContext> _context;
        private readonly ILogger<FeedbackViewRepository> _logger;
        public FeedbackViewRepository(IDbContextFactory<FeedbackViewContext> context,
            ILogger<FeedbackViewRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation)
        {
            if (idLocation <= 0)
            {
                _logger.LogWarning("Передан некорректный idLocation: {IdLocation}", idLocation);
                return Enumerable.Empty<FeedbackView>();
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {
                var feedbacks = await context.Feedbacks
                    .Where(n => n.IdLocation == idLocation)
                    .OrderBy(n => n.Id)
                    .ToListAsync();

                if (!feedbacks.Any())
                    _logger.LogInformation("Отзывы по локации с id {IdLocation} не найдены.", idLocation);

                return feedbacks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении отзывов по локации с id {IdLocation}", idLocation);
                return Enumerable.Empty<FeedbackView>();
            }
        }
    }
}
