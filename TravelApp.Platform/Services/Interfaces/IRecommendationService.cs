using TravelApp.Platform.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IRecommendationService
    {
        public Task<List<RecommendedItem>> GetRecommendedAsync(RecommendationModel model);
    }
}
