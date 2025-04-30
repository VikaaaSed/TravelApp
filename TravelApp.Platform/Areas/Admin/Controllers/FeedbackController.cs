using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FeedbackController : Controller
    {
        private readonly IAdminFeedbackService _adminFeedbackService;
        public FeedbackController(IAdminFeedbackService adminFeedbackService)
        {
            _adminFeedbackService = adminFeedbackService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Feedback> result = await _adminFeedbackService.GetFeedbacksAsync();
            return View(result);
        }
    }
}
