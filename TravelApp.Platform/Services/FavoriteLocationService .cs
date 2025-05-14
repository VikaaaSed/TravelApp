using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Services
{
    public class FavoriteLocationService : IFavoriteLocationService
    {
        private readonly FavoriteLocationHttpClient _httpClient;
        public FavoriteLocationService(FavoriteLocationHttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<FavoriteLocation> CreateAsync(FavoriteLocation favoriteLocation)
        {
            return await _httpClient.CreateAsync(favoriteLocation);
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClient.DeleteAsync(id);
        }

        public async Task<List<FavoriteLocation>> GetAllAsync()
        {
            var result = await _httpClient.GetAllAsync();
            return result.ToList();
        }

        public async Task<FavoriteLocation> GetAsync(int id)
        {
            return await _httpClient.GetAsync(id);
        }

        public async Task<List<FavoriteLocation>> GetByUserIdAsync(int id)
        {
            var result = await _httpClient.GetByUserIdAsync(id);
            return result.ToList();
        }
    }
}
