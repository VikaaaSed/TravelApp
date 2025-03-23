using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelApp.API.Models;
using TravelApp.Platform.ClientAPI;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CityHttpClient _cityHttpClient;

        public HomeController(ILogger<HomeController> logger, CityHttpClient cityHttpClient)
        {
            _logger = logger;
            _cityHttpClient = cityHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _cityHttpClient.GetAllAsync();
            List<City> cities = result.ToList();
            return View(cities);
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
