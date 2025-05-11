namespace TravelApp.Platform.Models
{
    public class UserFeedback
    {
        public string? Location { get; set; }
        public string? PageName { get; set; }
        public string? Text { get; set; }
        public int Ball { get; set; }
        public UserFeedback(string? location, string? pageName, string? text, int ball)
        {
            Location = location;
            PageName = pageName;
            Text = text;
            Ball = ball;
        }
    }
}
