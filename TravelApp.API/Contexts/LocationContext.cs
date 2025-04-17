using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options)
            : base(options)
        {
        }
        private LocationContext() => Database.EnsureCreated();
        public DbSet<Location> Locations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");
                entity.Property("Id").HasColumnName("id");
                entity.Property("IdCity").HasColumnName("id_city");
                entity.Property("Title").HasColumnName("title");
                entity.Property("Description").HasColumnName("description");
                entity.Property("Address").HasColumnName("address");
                entity.Property("WorkSchedule").HasColumnName("workschedule");
                entity.Property("TicketLink").HasColumnName("ticketlink");
                entity.Property("PictureInCityLink").HasColumnName("pictureincitylink");
                entity.Property("PicturePageLink").HasColumnName("picturepagelink");
                entity.Property("PageName").HasColumnName("pagename");
                entity.Property("Rating").HasColumnName("rating");
                entity.Property("PageVisible").HasColumnName("pagevisible");
            });
        }
    }

}
