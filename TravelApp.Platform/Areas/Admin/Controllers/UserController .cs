using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IAdminUserService _adminUserService;
        private readonly INotificationService _notificationService;
        public UserController(IAdminUserService adminUserService,
            INotificationService notificationService)
        {
            _adminUserService = adminUserService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<User> users = null;
            try
            {
                users = await _adminUserService.GetAll();
            }
            catch
            {
                _notificationService.Warning("Список пользователей пуст");
            }
            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adminUserService.Delete(id);
                _notificationService.Success($"Пользователь с id={id} успешно удален!!!");
            }
            catch
            {
                _notificationService.Error($"Ой! Произошла ошибка при удалении пользователя с id={id}");
            }
            return RedirectToAction("Index", "User", new { area = "Admin" });
        }
    }
}
