using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelApp.API.Models;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationService _locationService;

        public LocationController(ILogger<LocationController> logger, ILocationService cityService)
        {
            _logger = logger;
            _locationService = cityService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string pageName)
        {
            var token = Request.Cookies["jwt_token"];
            var name = _locationService.GetUserFirstAndLastName(token ?? "");
            AllLocationInformation locationInformation = await _locationService.GetAllLocationInformationAsync(pageName, token ?? "");

            if (locationInformation == null)
                return NotFound();

            if (!string.IsNullOrEmpty(name))
                ViewData["FIO"] = name;

            return View(locationInformation);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(string pageName, Feedback model)
        {
            var token = Request.Cookies["jwt_token"];
            await _locationService.CreateFeedbackAsync(model, token ?? "");
            return RedirectToAction("Index", new { pageName });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeFavoriteLocation(string pageName)
        {
            var token = Request.Cookies["jwt_token"];
            await _locationService.ChangeFavoriteLocation(pageName, token);
            return RedirectToAction("Index", new { pageName });
        }
    }
}
