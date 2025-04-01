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
        private readonly IClientIpService _clientIpService;

        public LocationController(ILogger<LocationController> logger, ILocationService cityService, 
            IClientIpService clientIpService)
        {
            _logger = logger;
            _locationService = cityService;
            _clientIpService = clientIpService;
        }
        public async Task<IActionResult> Index(string pageName)
        {
            AllLocationInformation location = await _locationService.GetAllLocationInformationAsync(pageName);

            if (location == null)
                return NotFound();

            return View(location);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(string pageName, Feedback model)
        {
            model.SenderIpAddress = _clientIpService.GetClientIp();
            model.DateTime = DateTime.UtcNow;
            Feedback feedback = await _locationService.CreateFeedbackAsync(model);
            return RedirectToAction("Index", new { pageName });
        }
    }
}
