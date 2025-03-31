namespace TravelApp.API.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int IdLocation { get; set; }
        public int? IdUser { get; set; }
        public int Ball { get; set; }
        public required string NameSender { get; set; }
        public required string SenderIpAddress { get; set; }
        public required string Text { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime DateTime { get; set; }
    }
}
