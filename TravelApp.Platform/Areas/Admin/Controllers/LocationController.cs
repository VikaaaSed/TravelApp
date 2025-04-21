using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {

        private readonly IAdminLocationService _locationService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IAdminLocationService locationService, ILogger<LocationController> logger)
        {
            _locationService = locationService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Location> locations = await _locationService.GetAllLocationsAsync();
            if (locations.Count < 1) _logger.LogWarning("Получено 0 локаций городов");
            return View(locations);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var information = await _locationService.GetAllLocationInfoAsync(id);
            if (information == null) return NotFound();

            return View("CreateOrEdit", information);
        }
    }
}
