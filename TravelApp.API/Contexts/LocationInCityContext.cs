using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class LocationInCityContext : DbContext
    {
        public LocationInCityContext(DbContextOptions<LocationInCityContext> options)
            : base(options)
        {
        }
        private LocationInCityContext() => Database.EnsureCreated();

        public DbSet<LocationInCity> Locations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationInCity>(entity =>
            {
                entity.ToView("location_in_city");
                entity.Property("Id").HasColumnName("id");
                entity.Property("CityId").HasColumnName("id_city");
                entity.Property("Title").HasColumnName("title");
                entity.Property("PageName").HasColumnName("pagename");
                entity.Property("Rating").HasColumnName("rating");
                entity.Property("PictureLink").HasColumnName("pictureincitylink");
            });
        }
    }
}
