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

        public async Task<IEnumerable<FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation, bool? accepted = null)
        {
            if (idLocation <= 0)
            {
                _logger.LogWarning("Передан некорректный idLocation: {IdLocation}", idLocation);
                return Enumerable.Empty<FeedbackView>();
            }

            await using var context = await _context.CreateDbContextAsync();

            try
            {
                var query = context.Feedbacks.Where(f => f.IdLocation == idLocation);

                if (accepted.HasValue)
                    query = query.Where(f => f.Accepted == accepted.Value);

                var feedbacks = await query
                    .OrderBy(f => f.Id)
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
