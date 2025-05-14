using TravelApp.API.Models;

namespace TravelApp.Platform.Services.Interfaces
{
    public interface IFavoriteLocationService
    {
        public Task<List<FavoriteLocation>> GetAllAsync();
        public Task<FavoriteLocation> GetAsync(int id);
        public Task<List<FavoriteLocation>> GetByUserIdAsync(int id);
        public Task<FavoriteLocation> CreateAsync(FavoriteLocation favoriteLocation);
        public Task DeleteAsync(int id);

    }
}
