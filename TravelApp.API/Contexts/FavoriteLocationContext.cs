using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class FavoriteLocationContext : DbContext
    {
        public FavoriteLocationContext(DbContextOptions<FavoriteLocationContext> options)
            : base(options) { }

        private FavoriteLocationContext() => Database.EnsureCreated();

        public DbSet<FavoriteLocation> FavoriteLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteLocation>(entity =>
            {
                entity.ToTable("favorite_location");
                entity.Property("Id").HasColumnName("id");
                entity.Property("IdLocation").HasColumnName("id_location");
                entity.Property("IdUser").HasColumnName("id_user");
            });
        }
    }
}
