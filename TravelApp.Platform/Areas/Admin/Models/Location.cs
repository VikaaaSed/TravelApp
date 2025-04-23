using System.ComponentModel.DataAnnotations;

namespace TravelApp.Platform.Areas.Admin.Models
{
    public class Location
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required string WorkSchedule { get; set; }
        public string? TicketLink { get; set; }
        public required string PictureInCityLink { get; set; }
        public required string PicturePageLink { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public required string PageName { get; set; }
        public bool PageVisible { get; set; }
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
        public static Location ConvertToUILocation(API.Models.Location location)
        {
            return new Location
            {
                Id = location.Id,
                IdCity = location.IdCity,
                Title = location.Title,
                Description = location.Description,
                Address = location.Address,
                WorkSchedule = location.WorkSchedule,
                TicketLink = location.TicketLink,
                PictureInCityLink = location.PictureInCityLink,
                PicturePageLink = location.PicturePageLink,
                PageVisible = location.PageVisible,
                PageName = location.PageName,
            };
        }
    }
}
