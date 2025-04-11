using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly IAdminCityService _cityService;
        public CityController(IAdminCityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await _cityService.GetAllCityAsync());
    }
}
