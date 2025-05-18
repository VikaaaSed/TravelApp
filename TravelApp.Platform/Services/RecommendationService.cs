using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ICityService _cityService;
        private readonly int MaxCountLocation = 5;

        public RecommendationService(ICityService cityService)
        {
            _cityService = cityService;
        }
        public async Task<List<RecommendedItem>> GetRecommendedAsync()
            => await GetRecommendationsOnMaxBall();

        private async Task<List<RecommendedItem>> GetRecommendationsOnMaxBall()
        {
            var location = await _cityService.GetVisibleLocationAsync();
            var result = location.ToList().OrderByDescending(l => l.Rating).
                Take(MaxCountLocation).Select(l => new RecommendedItem(
                 l.Id,
                 l.Title,
                 l.PageName,
                 l.PictureInCityLink,
                 l.Rating
                 ))
                .ToList();

            return result ?? [];
        }
    }
}
