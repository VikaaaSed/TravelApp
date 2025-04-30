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
        public async Task<IActionResult> Index(bool? isAccepted)
        {
            List<Feedback> feedbacks = await _adminFeedbackService.GetFeedbacksAsync();
            if (isAccepted.HasValue)
                feedbacks = feedbacks.Where(f => f.IsAccepted == isAccepted.Value).ToList();
            return View(feedbacks);

        }
        [HttpPost]
        public async Task<IActionResult> Accepted(int id)
        {
            await _adminFeedbackService.AcceptedFeedbacksAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _adminFeedbackService.DeleteFeedbackAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Feedback? feedback = await _adminFeedbackService.GetFeedbackByIdAsync(id);
            return View(feedback);
        }
    }
}
