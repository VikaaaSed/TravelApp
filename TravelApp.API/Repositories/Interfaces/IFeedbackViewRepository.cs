using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IFeedbackViewRepository
    {
        public Task<IEnumerable<FeedbackView>> GetFeedbackByIdLocationAsync(int idLocation, bool? accepted = null);
    }
}
