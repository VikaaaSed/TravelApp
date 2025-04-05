using Microsoft.AspNetCore.Mvc;
using TravelApp.Platform.Models;
using TravelApp.Platform.Services.Interfaces;

namespace TravelApp.Platform.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index() => View();
        [HttpGet]
        public IActionResult Registration()
        {
            ModelState.Clear();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistration user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            if (!await _userService.RegistrationUserAsync(user))
            {
                ModelState.AddModelError("", "Ошибка при регистрации. Возможно, пароли не совпадают или email уже занят.");
                return View(user);
            }
            TempData["SuccessMessage"] = "Регистрация прошла успешно!";
            return View();
        }
        [HttpGet]
        public IActionResult Authorization()
        {
            ModelState.Clear();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(UserAuthorization user)
        {
            if (!ModelState.IsValid)
                return View(user);
            if (await _userService.AuthorizationUserAsync(user) == null)
            {
                ModelState.AddModelError("", "Пользователь не найден. Возможно неверно указан логин или пароль.");
                return View(user);
            }
            return RedirectToAction("Index");
        }
    }
}
