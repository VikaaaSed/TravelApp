using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class UserFollowerContext : DbContext
    {
        public UserFollowerContext(DbContextOptions<UserFollowerContext> options)
            : base(options)
        {
        }
        private UserFollowerContext() => Database.EnsureCreated();
        public DbSet<UserFollower> Followers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.ToTable("user_follower");
                entity.Property("Id").HasColumnName("id");
                entity.Property("IdUser").HasColumnName("id_user");
                entity.Property("IdFollower").HasColumnName("id_follower");
            });
        }
    }
}
