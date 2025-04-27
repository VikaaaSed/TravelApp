namespace TravelApp.API.Models
{
    public class FeedbackView
    {
        public int Id { get; set; }
        public int IdLocation { get; set; }
        public required string SenderName { get; set; }
        public required string Text { get; set; }
        public int Ball { get; set; }
        public DateTime DateOfPublication { get; set; }
        public bool Accepted { get; set; }
    }
}
