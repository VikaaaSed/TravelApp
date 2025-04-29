using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        public Task<Feedback> CreateAsync(Feedback feedback);
        public Task<Feedback?> GetFeedbackAsync(int id);
        public Task UpdateFeedbackAsync(Feedback feedback);
        public Task AcceptedFeedbackAsync(int id);
    }
}
