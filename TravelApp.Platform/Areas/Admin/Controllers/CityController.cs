using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var information = await _cityService.GetAllCityInformationAsync(id);
            if (information == null) return NotFound();

            return View(information);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(City city)
        {
            await _cityService.UpdateCityAsync(city);
            return RedirectToAction(nameof(Edit), new { id = city.Id });
        }
    }

}
