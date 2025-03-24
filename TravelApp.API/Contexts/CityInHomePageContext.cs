using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class CityInHomePageContext : DbContext
    {
        public CityInHomePageContext(DbContextOptions<CityInHomePageContext> options)
            : base(options)
        {
        }
        private CityInHomePageContext() => Database.EnsureCreated();

        public DbSet<CityInHomePage> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityInHomePage>(entity =>
            {
                entity.ToView("city_in_home_page_view");
                entity.Property("Id").HasColumnName("id");
                entity.Property("Title").HasColumnName("title");
                entity.Property("PictureLink").HasColumnName("picturelink");
                entity.Property("PageName").HasColumnName("pagename");
            });
        }
    }
}
