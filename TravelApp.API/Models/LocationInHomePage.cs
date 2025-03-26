namespace TravelApp.API.Models
{
    public class LocationInHomePage
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string WorkSchedule { get; set; }
        public bool FreeVisit { get; set; }
        public required string PictureLink { get; set; }
        public required string PageName { get; set; }

    }
}
