namespace TravelApp.Platform.Areas.Admin.Models
{
    public class AllLocationInfo
    {
        public Location Location { get; set; }
        public AllLocationInfo(Location location)
        {
            Location = location;
        }
    }
}
