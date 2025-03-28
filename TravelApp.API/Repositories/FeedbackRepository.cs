using Microsoft.EntityFrameworkCore;
using TravelApp.API.Contexts;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IDbContextFactory<FeedbackContext> _context;
        private readonly ILogger<FeedbackRepository> _logger;
        public FeedbackRepository(IDbContextFactory<FeedbackContext> context, ILogger<FeedbackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Feedback> CreateAsync(Feedback feedback)
        {
            var context = await _context.CreateDbContextAsync();
            await context.Feedbacks.AddAsync(feedback);
            await context.SaveChangesAsync();
            return feedback;
        }
    }
}
