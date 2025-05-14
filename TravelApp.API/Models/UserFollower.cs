namespace TravelApp.API.Models
{
    public class UserFollower
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdFollower { get; set; }
    }
}
