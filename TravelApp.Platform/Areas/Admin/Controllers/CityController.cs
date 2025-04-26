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
            AllCityInformation model = new(city, await _cityService.GetLocationInCityByCityIdAsync(city.Id));
            if (!ModelState.IsValid) return View("CreateOrEdit", model);

            City c = await _cityService.GetCityByPageNameAsync(city.PageName);
            if (c != null && c.Id != city.Id)
            {
                ModelState.AddModelError(nameof(city.PageName), "Страница с таким именем уже существует");
                return View("CreateOrEdit", model);
            }

            if (city.Id == 0)
                await _cityService.CreateCityAsync(city);
            else
                await _cityService.UpdateCityAsync(city);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _cityService.DeleteCityAsync(id);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            int idCity = await _cityService.DeleteLocationInCityAsync(id);
            return RedirectToAction(nameof(Edit), new { id = idCity });
        }
    }


}
