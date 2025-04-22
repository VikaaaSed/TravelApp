namespace TravelApp.Platform.Areas.Admin.Models
{
    public class AllLocationInfo
    {
        public Location? Location { get; set; }
        public List<API.Models.LocationGallery> Gallery { get; set; }
        public AllLocationInfo(Location location, List<API.Models.LocationGallery> gallery)
        {
            Location = location;
            Gallery = gallery;
        }
        public AllLocationInfo()
        {
            Location = null;
            Gallery = [];
        }
    }
}
