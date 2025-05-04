using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationInHomePageViewRepository _repositoryLocationInHomePage;
        private readonly ILocationRepository _repositoryLocation;
        private readonly ILogger<LocationController> _logger;

        public LocationController(
            ILocationRepository repositoryLocation,
            ILocationInHomePageViewRepository locationInHomePageRepository,
            ILogger<LocationController> logger)
        {
            _repositoryLocationInHomePage = locationInHomePageRepository;
            _repositoryLocation = repositoryLocation;
            _logger = logger;
        }
        [HttpGet("visible/by-city/{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Location>>> GetVisibleLocationsByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", cityId);
                return BadRequest("Некорректный идентификатор города.");
            }

            var locations = await _repositoryLocation.GetVisibleLocationByCityIdAsync(cityId);
            if (locations == null || !locations.Any())
            {
                _logger.LogWarning("В системе нет видимых локаций для города с id {id}", cityId);
                return NotFound("Не найдены локации для данного города.");
            }

            _logger.LogInformation("Список видимых локаций для города с id {id} успешно получен", cityId);
            return Ok(locations);
        }
        [HttpGet("by-page")]
        public async Task<ActionResult<LocationInHomePage>> GetByPageNameAsync(string pageName)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                _logger.LogWarning("Попытка запроса информации о локации с пустым pageName");
                return BadRequest();
            }
            var location = await _repositoryLocationInHomePage.GetLocationByPageNameAsync(pageName);
            if (location == null)
            {
                _logger.LogWarning("Не удалось получить локацию по указанному названию странице {pageName}", pageName);
                return NotFound();
            }
            _logger.LogInformation("Локация успешно получена");
            return Ok(location);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Location>> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", id);
                return BadRequest();
            }
            Location? location = await _repositoryLocation.GetAsync(id);
            if (location == null)
            {
                _logger.LogWarning("Не удалось найти локацию по указанному id {id}", id);
                return NotFound();
            }
            _logger.LogInformation("Локация успешно получена");
            return Ok(location);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Location>> CreateAsync([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest(ModelState);
            }
            Location newLocation = await _repositoryLocation.CreateAsync(location);
            _logger.LogInformation("Создана новая локация с id {id}", newLocation.Id);
            return Created($"/api/Location/{newLocation.Id}", newLocation);
        }


        [HttpGet("by-city/{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Location>>> GetByCityIdAsync(int cityId)
        {
            if (cityId <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", cityId);
                return BadRequest();
            }
            IEnumerable<Location> location = await _repositoryLocation.GetLocationByCityIdAsync(cityId);
            if (!location.Any())
            {
                _logger.LogWarning("В системе нет локаций по указанному городу {id}", cityId);
                return NotFound();
            }
            _logger.LogInformation("Список локаций для города успешно получена");
            return Ok(location);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest(ModelState);
            }
            if (await _repositoryLocation.GetAsync(id) == null)
            {
                _logger.LogWarning("Локация с id {id} не найденa для обновления", id);
                return NotFound();
            }
            await _repositoryLocation.UpdateAsync(location);
            _logger.LogInformation("Локация с id {id} успешно обновлена", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления с некорректным id {id}", id);
                return BadRequest();
            }
            if (await _repositoryLocation.GetAsync(id) == null)
            {
                _logger.LogWarning("Локация с id {id} не найдена для удаления", id);
                return NotFound();
            }
            await _repositoryLocation.DeleteAsync(id);
            _logger.LogInformation("Локация с id {id} успешно удалена", id);

            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllAsync()
        {
            var locations = await _repositoryLocation.GetAllAsync();
            if (locations == null)
            {
                _logger.LogWarning("В системе нет ни одной локации");
                return NotFound();
            }
            _logger.LogInformation("Список локаций успешно получен");

            return Ok(locations);
        }
        [HttpGet("visible")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Location>>> GetVisible()
        {
            var locations = await _repositoryLocation.GetVisibleAsync();
            if (locations == null || !locations.Any())
            {
                _logger.LogWarning("В системе нет ни одной локации");
                return NotFound();
            }
            _logger.LogInformation("Список локаций успешно получен");

            return Ok(locations);
        }
    }
}
