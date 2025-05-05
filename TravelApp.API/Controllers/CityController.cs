using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _repository;
        private readonly ICityInHomePageViewRepository _viewRepository;
        private readonly ILogger<CityController> _logger;
        public CityController(ICityRepository repository,
            ILogger<CityController> logger,
            ICityInHomePageViewRepository viewRepository)
        {
            _repository = repository;
            _logger = logger;
            _viewRepository = viewRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<City>>> GetAllAsync()
        {
            var cities = await _repository.GetAllAsync();
            if (cities == null || !cities.Any())
            {
                _logger.LogWarning("В системе нет ни одного города");
                return NotFound();
            }
            _logger.LogInformation("Список всех городов получен");
            return Ok(cities);
        }

        [HttpGet("in-home-page/visible")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CityInHomePage>>> GetVisibleCityAsync()
        {
            var cities = await _viewRepository.GetVisibleAsync();
            if (cities == null || !cities.Any())
            {
                _logger.LogWarning("В системе нет ни одного города для отображения (представление для домашней страницы)");
                return NotFound();
            }
            _logger.LogInformation("Список всех городов для отображения получен (представление для домашней страницы)");
            return Ok(cities);
        }

        [HttpGet("in-home-page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CityInHomePage>>> GetCityInHomePageAsync()
        {
            var cities = await _viewRepository.GetAllAsync();
            if (cities == null || !cities.Any())
            {
                _logger.LogWarning("В системе нет ни одного города (представление для домашней страницы)");
                return NotFound();
            }
            _logger.LogInformation("Список всех городов получен (представление для домашней страницы)");
            return Ok(cities);
        }

        [HttpGet("by-page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<City?>> GetCityByPageNameAsync(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                _logger.LogWarning("Попытка запроса информации о городе с пустым pageName");
                return BadRequest();
            }
            var city = await _repository.GetCityByPageNameAsync(pageName);
            if (city == null)
            {
                _logger.LogWarning("Не удалось найти город по указанному pageName={pageName}", pageName);
                return NotFound();
            }
            _logger.LogInformation("Информация о городе успешно получена по pageName={pageName}", pageName);
            return Ok(city);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<City>> Create([FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            City newCity = await _repository.CreateCityAsync(city);

            _logger.LogInformation("Создан новый город с id={id}", newCity.Id);
            return Created($"/api/City/{newCity.Id}", newCity);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            if (await _repository.GetAsync(id) == null)
            {
                _logger.LogWarning("Город с id={id} не найдена для обновления", id);
                return NotFound();
            }
            await _repository.UpdateCityAsync(city);
            _logger.LogInformation("Город с id={id} успешно обновлён", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления с некорректным id {id}", id);
                return BadRequest();
            }
            if (await _repository.GetAsync(id) == null)
            {
                _logger.LogWarning("Город с id={id} не найдена для удаления", id);
                return NotFound();
            }
            await _repository.DeleteCityAsync(id);
            _logger.LogInformation("Город с id={id} успешно удален", id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<City>> Get(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка поиска с некорректным id={id}", id);
                return BadRequest();
            }
            var result = await _repository.GetAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Город с id={id} не найден", id);
                return NotFound();
            }
            _logger.LogInformation("Город успешно получен");
            return Ok(result);
        }
    }
}
