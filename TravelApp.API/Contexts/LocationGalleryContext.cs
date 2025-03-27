using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class LocationGalleryContext : DbContext
    {
        public LocationGalleryContext(DbContextOptions<LocationGalleryContext> options)
            : base(options)
        {
        }
        private LocationGalleryContext() => Database.EnsureCreated();

        public DbSet<LocationGallery> Gallery { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationGallery>(entity =>
            {
                entity.ToTable("location_gallery");
                entity.Property("Id").HasColumnName("id");
                entity.Property("LocationId").HasColumnName("id_location");
                entity.Property("Title").HasColumnName("title");
                entity.Property("PictureLink").HasColumnName("picturelink");
            });
        }
    }
}
