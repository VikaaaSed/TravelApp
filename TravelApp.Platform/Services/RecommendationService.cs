using TravelApp.API.Models;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ICityService _cityService;
        private const int MaxCountLocationOnMax = 3;
        private const int MaxCountLocationOnCity = 4;
        private const int MaxCountLocation = 5;

        public RecommendationService(ICityService cityService)
        {
            _cityService = cityService;
        }
        public async Task<List<RecommendedItem>> GetRecommendedAsync(List<int> idCity)
        {
            var maxBallRecTask = GetRecommendationsOnMaxBall();
            var cityRecTask = GetRecommendationsOnCity(idCity);

            await Task.WhenAll(maxBallRecTask, cityRecTask);

            List<RecommendedItem> result = maxBallRecTask.Result;
            result.AddRange(cityRecTask.Result);

            return result.DistinctBy(r => r.Id).Take(MaxCountLocation).ToList();
        }

        private async Task<List<RecommendedItem>> GetRecommendationsOnMaxBall()
        {
            var locations = await _cityService.GetVisibleLocationAsync();

            return GetRecommendations(locations).OrderByDescending(l => l.Rating).Take(MaxCountLocationOnMax).ToList();
        }
        private async Task<List<RecommendedItem>> GetRecommendationsOnCity(List<int> idCity)
        {
            var locations = await GetLocationList(idCity);
            return GetRecommendations(locations).OrderBy(l => l.Title).Take(MaxCountLocationOnCity).ToList();
        }
        private async Task<List<Location>> GetLocationList(List<int> idCites)
        {
            List<Location> locationList = new List<Location>();

            foreach (var id in idCites)
                locationList.AddRange(await _cityService.GetVisibleLocationAsync(id));
            return locationList;
        }
        private List<RecommendedItem> GetRecommendations(List<Location> locations)
        {
            List<RecommendedItem> result = locations.ToList().Select(l => new RecommendedItem(
                 l.Id,
                 l.Title,
                 l.PageName,
                 l.PictureInCityLink,
                 l.Rating
                 ))
                .ToList() ?? [];

            return result;
        }
    }
}
