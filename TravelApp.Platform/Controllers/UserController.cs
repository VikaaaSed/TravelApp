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
            var feedbackTask = _userService.GetUserFeedbackAsync(user.Id);
            var recommendationsTask = _userService.GetUserRecommendation();
            var favoriteLocationsTask = _userService.GetFavoriteLocationsAsync(user.Id);
            var subscriptionsTask = _userService.GetUserSubscriptionsAsync(user.Id);
            var followersTask = _userService.GetUserFollowersAsync(user.Id);

            await Task.WhenAll(feedbackTask, favoriteLocationsTask, subscriptionsTask, followersTask);
            await Task.WhenAll(feedbackTask, favoriteLocationsTask, subscriptionsTask, followersTask, recommendationsTask);

            var feedback = feedbackTask.Result;
            var favoriteLocations = favoriteLocationsTask.Result;
            var followers = followersTask.Result;
            var recommendations = recommendationsTask.Result;
            var subscriptions = subscriptionsTask.Result;

            AllUserInformation userInformation =
            new AllUserInformation(user, feedback, favoriteLocations, subscriptions, followers, recommendations);
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
        [Authorize]
        [HttpGet("[controller]/profile/{id}")]
        [HttpGet("[controller]/index/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var userTask = _userService.GetAsync(id);
            var feedbackTask = _userService.GetUserFeedbackAsync(id);
            var favoriteLocationsTask = _userService.GetFavoriteLocationsAsync(id);
            var subscriptionsTask = _userService.GetUserSubscriptionsAsync(id);
            var followersTask = _userService.GetUserFollowersAsync(id);

            await Task.WhenAll(userTask, feedbackTask, favoriteLocationsTask, subscriptionsTask, followersTask);

            var user = userTask.Result;
            var feedback = feedbackTask.Result;
            var favoriteLocations = favoriteLocationsTask.Result;
            var subscriptions = subscriptionsTask.Result;
            var followers = followersTask.Result;
            if (user == null) return NotFound();
            AllUserInformation userInformation = new AllUserInformation(user, feedback, favoriteLocations,
                subscriptions, followers);
            return View(userInformation);
        }
    }
}
