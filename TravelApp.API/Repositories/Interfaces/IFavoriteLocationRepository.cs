using TravelApp.API.Models;

namespace TravelApp.API.Repositories.Interfaces
{
    public interface IFavoriteLocationRepository
    {
        public Task<FavoriteLocation> GetAsync(int id);
        public Task<IEnumerable<FavoriteLocation>> GetAllAsync();
        public Task<IEnumerable<FavoriteLocation>> GetByUserIdAsync(int idUser);
        public Task<FavoriteLocation> CreateAsync(FavoriteLocation favoriteLocation);
        public Task DeleteAsync(int id);
    }
}
