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
        private readonly INotificationService _notificationService;
        public FeedbackController(IAdminFeedbackService adminFeedbackService, INotificationService notificationService)
        {
            _adminFeedbackService = adminFeedbackService;
            _notificationService = notificationService;
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
            try
            {
                await _adminFeedbackService.AcceptedFeedbacksAsync(id);
                _notificationService.Success($"Отзыв с id={id} успешно подтвержден!!!");
            }
            catch
            {
                _notificationService.Error($"Ой! произошла ошибка при подтверждении отзыва c id={id}");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adminFeedbackService.DeleteFeedbackAsync(id);
                _notificationService.Success($"Отзыв c id={id} успешно удален!!!");
            }
            catch
            {
                _notificationService.Error("Ой! Произошла ошибка при удалении отзыва");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Feedback? feedback = null;
            try
            {
                feedback = await _adminFeedbackService.GetFeedbackByIdAsync(id);
            }
            catch
            {
                _notificationService.Error($"Ой! Произошла ошибка при открытии отзыва id={id} для редактирования");
            }
            return View(feedback);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(Feedback feedback)
        {
            try
            {
                await _adminFeedbackService.UpdateFeedbackAsync(feedback);
                _notificationService.Success($"Отзыв с id={feedback.Id} успешно отредактирован!!!");
            }
            catch
            {
                _notificationService.Error("Ой! Произошла ошибка попытке редактирования отзыва");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
