using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.Platform.Areas.Admin.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {

        private readonly IAdminLocationService _locationService;
        private readonly ILogger<LocationController> _logger;
        private readonly INotificationService _notifier;

        public LocationController(IAdminLocationService locationService, ILogger<LocationController> logger,
            INotificationService notifier)
        {
            _locationService = locationService;
            _logger = logger;
            _notifier = notifier;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<API.Models.Location> locations = await _locationService.GetAllLocationsAsync();
            if (locations.Count < 1)
            {
                _notifier.Error($"Ой! Произошла ошибка не удалось загрузить не одного города");
                _logger.LogWarning("Получено 0 локаций городов");
            }
            return View(locations);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AllLocationInfo information = await _locationService.GetAllLocationInfoAsync(id);
            if (information == null)
            {
                _notifier.Error($"Ой! Произошла ошибка при получении информации о локации по id={id}");
                return NotFound();
            }
            return View("CreateOrEdit", information);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(AllLocationInfo model)
        {
            if (model?.Location?.Id != 0)
            {
                try
                {
                    await _locationService.UpdateLocation(model?.Location);
                    _notifier.Success($"Локация {model.Location.PageName} успешно обновлена!!!");
                }
                catch
                {
                    _notifier.Error($"Ой! Произошла ошибка при обновлении локации {model.Location.PageName}");
                }
            }
            else
            {
                try
                {
                    var location = await _locationService.CreateLocation(model.Location);
                    foreach (var item in model.Gallery)
                        item.LocationId = location.Id;
                    _notifier.Success($"Локация {model.Location.PageName} успешно создана!!!");
                }
                catch
                {
                    _notifier.Error($"Ой! Произошла ошибка при создании локации {model.Location.PageName}");
                }
            }
            if (model.Gallery.Count != 0)
            {
                try
                {
                    await _locationService.EditGallery(model.Gallery, model.Location.Id);
                }
                catch
                {
                    _notifier.Error($"Ой! Произошла ошибка при получении галереи локации");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create(int idCity)
        {
            var newLocation = new Location();
            newLocation.IdCity = idCity;
            AllLocationInfo model = new(newLocation, []);
            return View("CreateOrEdit", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _locationService.DeleteLocation(id);
                _notifier.Success($"Локация с id = {id} успешно удалена!!!");
            }
            catch
            {
                _notifier.Error($"Ой! Произошла ошибка при удалении локации id={id}");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
