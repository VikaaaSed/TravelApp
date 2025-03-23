namespace TravelApp.API.Models
{
    public class City
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? MainPictureLink { get; set; }
        public string? PictureAtHomeLink { get; set; }
        public required string PageName { get; set; }
        public bool PageVisible { get; set; }
    }
}
