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
        private const int MaxCountLocationOnFollowers = 4;

        public RecommendationService(ICityService cityService)
        {
            _cityService = cityService;
        }
        public async Task<List<RecommendedItem>> GetRecommendedAsync(RecommendationModel model)
        {
            var maxBallRecTask = GetRecommendationsOnMaxBall();
            var cityRecTask = GetRecommendationsOnCity(model.MyCityId);
            var followersRecTask = GetRecommendationsOnFollowers(model.MyFriendIdCity);

            await Task.WhenAll(maxBallRecTask, cityRecTask, followersRecTask);

            List<RecommendedItem> maxBall = maxBallRecTask.Result.DistinctBy(i => i.Id).Take(MaxCountLocationOnMax).ToList();
            List<RecommendedItem> cityRec = cityRecTask.Result;
            List<RecommendedItem> followersRec = followersRecTask.Result;
            cityRec = containsList(cityRec, maxBall, MaxCountLocationOnCity);

            followersRec = containsList(followersRec, [.. maxBall, .. cityRec], MaxCountLocationOnFollowers);

            return followersRec.Concat([.. maxBall, .. cityRec]).Take(MaxCountLocation).ToList();
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
        private async Task<List<RecommendedItem>> GetRecommendationsOnFollowers(Dictionary<int, List<int>> followersIdCites)
        {
            followersIdCites = followersIdCites.OrderBy(f => f.Value.Count).Take(5).ToDictionary();

            List<Location> location = [];

            foreach (var id in followersIdCites)
            {
                location.AddRange(await GetLocationList(id.Value));
                location.DistinctBy(l => l.Id).ToList();
            }
            return GetRecommendations(location);
        }
        private List<RecommendedItem> containsList(List<RecommendedItem> a, List<RecommendedItem> b, int count)
            => a.Where(item => !b.Any(n => n.Id == item.Id))
                   .Distinct()
                   .Take(count)
                   .ToList();
    }
}
