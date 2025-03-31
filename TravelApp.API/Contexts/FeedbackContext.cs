using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class FeedbackContext : DbContext
    {
        public FeedbackContext(DbContextOptions<FeedbackContext> options)
            : base(options)
        {
        }
        private FeedbackContext() => Database.EnsureCreated();

        public DbSet<Feedback> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("feedback");
                entity.Property("Id").HasColumnName("id");
                entity.Property("IdLocation").HasColumnName("id_location");
                entity.Property("NameSender").HasColumnName("namesender");
                entity.Property("Text").HasColumnName("textfeedback");
                entity.Property("Ball").HasColumnName("ball");
                entity.Property("IsAccepted").HasColumnName("accepted");
                entity.Property("DateTime").HasColumnName("datetime");
                entity.Property("SenderIpAddress").HasColumnName("senderipaddress");
                entity.Property("IdUser").HasColumnName("id_user");
            });
        }
    }
}
