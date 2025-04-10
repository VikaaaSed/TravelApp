using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelApp.Platform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
