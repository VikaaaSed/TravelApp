using System.ComponentModel.DataAnnotations;

namespace TravelApp.Platform.Areas.Admin.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? WorkSchedule { get; set; }
        public string? TicketLink { get; set; }
        public string? PictureInCityLink { get; set; }
        public string? PicturePageLink { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string? PageName { get; set; }
        public bool PageVisible { get; set; }
        public Location() { }
        public static API.Models.Location ConvertToDBLocation(Location location)
        {
            return new API.Models.Location
            {
                Id = location.Id,
                IdCity = location.IdCity,
                Title = location.Title ?? "",
                Description = location.Description ?? "",
                Address = location.Address ?? "",
                WorkSchedule = location.WorkSchedule ?? "",
                TicketLink = location.TicketLink ?? "",
                PictureInCityLink = location.PictureInCityLink ?? "",
                PicturePageLink = location.PicturePageLink ?? "",
                PageVisible = location.PageVisible,
                PageName = location.PageName ?? "",
            };
        }
        public static Location? ConvertToUILocation(API.Models.Location location)
        {
            if (location == null) return new Location();
            return new Location
            {
                Id = location.Id,
                IdCity = location.IdCity,
                Title = location.Title,
                Description = location.Description ?? "",
                Address = location.Address ?? "",
                WorkSchedule = location.WorkSchedule ?? "",
                TicketLink = location.TicketLink ?? "",
                PictureInCityLink = location.PictureInCityLink ?? "",
                PicturePageLink = location.PicturePageLink ?? "",
                PageVisible = location.PageVisible,
                PageName = location.PageName ?? "",
            };
        }
    }
}
