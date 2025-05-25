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
        private const int MaxCountLocation = 7;

        public RecommendationService(ICityService cityService)
        {
            _cityService = cityService;
        }
        public async Task<List<RecommendedItem>> GetRecommendedAsync(RecommendationModel model)
        {
            var maxBallRecTask = GetRecommendationsOnMaxBall();
            var cityRecTask = GetRecommendationsOnCity(model.MyCityId);

            await Task.WhenAll(maxBallRecTask, cityRecTask);

            List<RecommendedItem> maxBall = maxBallRecTask.Result.DistinctBy(i => i.Id).Take(MaxCountLocationOnMax).ToList();
            List<RecommendedItem> cityRec = cityRecTask.Result;
            var a = cityRec.Where(item => !maxBall.Any(n => n.Id == item.Id))
                           .Distinct()
                           .Take(MaxCountLocationOnCity)
                           .ToList();
            List<RecommendedItem> items = maxBall.Concat(a).Take(MaxCountLocation).ToList();
            return items;
        }

        private async Task<List<RecommendedItem>> GetRecommendationsOnMaxBall()
        {
            var locations = await _cityService.GetVisibleLocationAsync();

            return GetRecommendations(locations).OrderByDescending(l => l.Rating).ToList();
        }
        private async Task<List<RecommendedItem>> GetRecommendationsOnCity(List<int> idCity)
        {
            var locations = await GetLocationList(idCity);
            return GetRecommendations(locations).OrderBy(l => l.Title).ToList();
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
            List<RecommendedItem> result = locations.DistinctBy(l => l.Id).Select(l => new RecommendedItem(
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
