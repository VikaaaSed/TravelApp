using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class CityContext : DbContext
    {
        public CityContext(DbContextOptions<CityContext> options)
            : base(options)
        {
        }
        private CityContext() => Database.EnsureCreated();

        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");
                entity.Property("Id").HasColumnName("id");
                entity.Property("Title").HasColumnName("title");
                entity.Property("Description").HasColumnName("description");
                entity.Property("MainPictureLink").HasColumnName("mainpicturelink");
                entity.Property("PictureAtHomeLink").HasColumnName("pictureathomelink");
                entity.Property("PageName").HasColumnName("pagename");
                entity.Property("PageVisible").HasColumnName("pagevisible");
            });
        }
    }
}
