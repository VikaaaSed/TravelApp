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
        private readonly IJwtTokenService _jwtTokenService;

        public LocationController(ILogger<LocationController> logger, ILocationService cityService, 
            IClientIpService clientIpService, IJwtTokenService jwtTokenService)
        {
            _logger = logger;
            _locationService = cityService;
            _clientIpService = clientIpService;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<IActionResult> Index(string pageName)
        {
            AllLocationInformation location = await _locationService.GetAllLocationInformationAsync(pageName);

            if (location == null)
                return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                var token = Request.Cookies["jwt_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    var name = _jwtTokenService.GetNameUserFromToken(token);
                    if (!string.IsNullOrEmpty(name))
                        ViewData["FIO"] = name;
                }
            }
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
            if (User.Identity.IsAuthenticated)
            {
                var token = Request.Cookies["jwt_token"];
                if (!string.IsNullOrEmpty(token))
                {
                    var id = _jwtTokenService.GetUserIdFromToken(token);
                    if (!string.IsNullOrEmpty(id) && int.TryParse(id, out int userId))
                        model.IdUser = userId;
                }
            }
            Feedback feedback = await _locationService.CreateFeedbackAsync(model);
            return RedirectToAction("Index", new { pageName });
        }
    }
}
