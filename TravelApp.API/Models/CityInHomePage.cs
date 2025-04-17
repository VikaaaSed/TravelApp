namespace TravelApp.API.Models
{
    public class CityInHomePage
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string PictureLink { get; set; }
        public required string PageName { get; set; }
        public bool VisiblePage { get; set; }
    }
}
