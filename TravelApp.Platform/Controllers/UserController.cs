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
        [HttpGet("[controller]/profile")]
        [HttpGet("[controller]/index")]
        [HttpGet("[controller]")]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["jwt_token"];
            var user = await _userService.GetUserByTokenAsync(token ?? "");
            var fb = await _userService.GetUserFeedbackAsync(user.Id);
            var fl = await _userService.GetFavoriteLocationsAsync(user.Id);
            AllUserInformation userInformation = new AllUserInformation(user, fb, fl);
            return View(userInformation);
        }
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
                return RedirectToAction("profile");
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
                Expires = DateTime.UtcNow.AddMinutes(360)
            });
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Authorization");
        }
    }
}
