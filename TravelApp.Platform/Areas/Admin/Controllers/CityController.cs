using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.Platform.Areas.Admin.Services.Interfaces;
using TravelApp.Platform.Models;

namespace TravelApp.Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly IAdminCityService _cityService;
        public CityController(IAdminCityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
            => View(await _cityService.GetAllCityAsync());
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var information = await _cityService.GetAllCityInformationAsync(id);
            if (information == null) return NotFound();

            return View("CreateOrEdit", information);
        }
        [HttpGet]
        public IActionResult Create()
        {
            AllCityInformation model = new(new City { PageName = "", Title = "" }, []);
            return View("CreateOrEdit", model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(City city)
        {
            AllCityInformation model = new(city, city.Id != 0 ? await _cityService.GetLocationInCityByCityIdAsync(city.Id) : []);
            if (!ModelState.IsValid)
            {
                TempData["Error"] = $"Ой! Произошла ошибка не все обязательные поля заполнены";
                return View("CreateOrEdit", model);
            }
            City? c = await _cityService.GetCityByPageNameAsync(city.PageName);
            if (c != null && c.Id != city.Id)
            {
                ModelState.AddModelError(nameof(city.PageName), "Страница с таким именем уже существует");
                TempData["Error"] = $"Ой! Произошла ошибка страница с именем {city.PageName} уже существует";
                return View("CreateOrEdit", model);
            }

            if (city.Id == 0)
            {
                try
                {
                    await _cityService.CreateCityAsync(city);
                    TempData["Success"] = $"Город {city.PageName} успешно добавлен !!!";
                }
                catch
                {
                    TempData["Error"] = $"Ой! Произошла ошибка при добавлении города {city.PageName}";
                }
            }

            else
            {
                try
                {
                    await _cityService.UpdateCityAsync(city);
                    TempData["Success"] = $"Город {city.PageName} успешно изменен !!!";
                }
                catch
                {
                    TempData["Error"] = $"Ой! Произошла ошибка при изменении города {city.PageName}";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _cityService.DeleteCityAsync(id);
                TempData["Success"] = $"Город с id {id} успешно удален !!!";
            }
            catch
            {
                TempData["Error"] = $"Ой! Произошла ошибка при удалении города {id}";
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            int idCity = 0;
            try
            {
                idCity = await _cityService.DeleteLocationInCityAsync(id);
                TempData["Success"] = $"Локация с id {id} успешно удалена !!!";
            }
            catch
            {
                TempData["Error"] = $"Ой! Произошла ошибка при удалении локации {id}";
            }
            return RedirectToAction(nameof(Edit), new { id = idCity });
        }
    }


}
