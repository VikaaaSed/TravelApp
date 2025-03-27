namespace TravelApp.API.Models
{
    public class LocationGallery
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public required string Title { get; set; }
        public required string PictureLink { get; set; }
    }
}
