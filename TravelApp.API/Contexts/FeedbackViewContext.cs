using Microsoft.EntityFrameworkCore;
using TravelApp.API.Models;

namespace TravelApp.API.Contexts
{
    public class FeedbackViewContext : DbContext
    {
        public FeedbackViewContext(DbContextOptions<FeedbackViewContext> options)
            : base(options)
        {
        }
        private FeedbackViewContext() => Database.EnsureCreated();

        public DbSet<FeedbackView> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackView>(entity =>
            {
                entity.ToView("feedback_view");
                entity.Property("Id").HasColumnName("id");
                entity.Property("IdLocation").HasColumnName("id_location");
                entity.Property("SenderName").HasColumnName("namesender");
                entity.Property("Text").HasColumnName("textfeedback");
                entity.Property("Ball").HasColumnName("ball");
                entity.Property("DateOfPublication").HasColumnName("datetime");
            });
        }
    }
}
