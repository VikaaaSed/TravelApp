namespace TravelApp.API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string WorkSchedule { get; set; }
        public string? TicketLink { get; set; }
        public required string PictureInCityLink { get; set; }
        public required string PicturePageLink { get; set; }
        public required string PageName { get; set; }
        public double Rating { get; set; }
        public bool PageVisible { get; set; }
    }

}
