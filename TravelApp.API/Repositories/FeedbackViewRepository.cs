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
            => await _context.CreateDbContext().Feedbacks.Where(n => n.IdLocation == idLocation)
            .OrderBy(n => n.Id).ToListAsync();
    }
}
