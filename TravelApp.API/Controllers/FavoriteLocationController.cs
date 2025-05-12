using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteLocationController : ControllerBase
    {
        private readonly IFavoriteLocationRepository _repository;
        private readonly ILogger<FavoriteLocationController> _logger;

        public FavoriteLocationController(IFavoriteLocationRepository repository,
            ILogger<FavoriteLocationController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FavoriteLocation>>> GetAll()
        {
            var favoriteLocations = await _repository.GetAllAsync();
            if (favoriteLocations == null || !favoriteLocations.Any())
            {
                _logger.LogWarning("В системе нет ни одной записи о избранных локациях");
                return NotFound();
            }
            _logger.LogInformation("Список всех записей о избранных локаций получен");
            return Ok(favoriteLocations);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FavoriteLocation>> Get(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка поиска с некорректным id={id}", id);
                return BadRequest();
            }
            var result = await _repository.GetAsync(id);
            if (result == null)
            {
                _logger.LogWarning("Запись с id={id} не найден", id);
                return NotFound();
            }
            _logger.LogInformation("Запись о избранной локации успешно получен");
            return Ok(result);
        }
        [HttpGet("/api/users/{userId}/favorites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FavoriteLocation>>> GetByUserId(int userId)
        {
            if (userId <= 0)
            {
                _logger.LogWarning("Попытка поиска с некорректным id={id}", userId);
                return BadRequest();
            }
            var result = await _repository.GetByUserIdAsync(userId);
            if (result == null)
            {
                _logger.LogWarning("Запись с id={id} не найден", userId);
                return NotFound();
            }
            _logger.LogInformation("Запись о избранной локации успешно получен");
            return Ok(result);
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
                _logger.LogWarning("Запись с id={id} не найдена для удаления", id);
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Запись с id={id} успешно удален", id);
            return NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FavoriteLocation>> Create([FromBody] FavoriteLocation favoriteLocation)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            FavoriteLocation newFavoriteLocation = await _repository.CreateAsync(favoriteLocation);

            _logger.LogInformation("Создана новая запись с id={id}", newFavoriteLocation.Id);
            return Created($"/api/FavoriteLocation/{newFavoriteLocation.Id}", newFavoriteLocation);
        }
    }
}
