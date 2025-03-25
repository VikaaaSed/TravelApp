namespace TravelApp.API.Models
{
    public class LocationInCity
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public required string Title { get; set; }
        public required string PageName { get; set; }
        public required string PictureLink { get; set; }
        public double Rating { get; set; }
    }
}
