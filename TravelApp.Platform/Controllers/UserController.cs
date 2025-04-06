using Microsoft.AspNetCore.Authorization;
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
        [Authorize]        
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
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");
            ModelState.Clear();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Authorization(UserAuthorization user)
        {
            if (!ModelState.IsValid)
                return View(user);
            var token = await _userService.AuthorizationUserAsync(user);
            if (token == null)
            {
                ModelState.AddModelError("", "Пользователь не найден. Возможно неверно указан логин или пароль.");
                return View(user);
            }
            Response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            return RedirectToAction("Index");
        }
    }
}
