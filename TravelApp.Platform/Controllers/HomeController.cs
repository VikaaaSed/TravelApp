using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "AdminPanel", new { area = "Admin" });
            return View(await _homeService.GetAllCityAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
