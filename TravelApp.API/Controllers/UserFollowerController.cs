using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFollowerController : ControllerBase
    {
        private readonly IUserFollowerRepository _repository;
        private readonly ILogger<UserFollowerController> _logger;
        public UserFollowerController(IUserFollowerRepository repository,
            ILogger<UserFollowerController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserFollower>>> GetAll()
        {
            var followers = await _repository.GetAllAsync();
            if (followers == null || !followers.Any())
            {
                _logger.LogWarning("В системе нет ни одной записи о подписчиках");
                return NotFound();
            }
            _logger.LogInformation("Список всех записей о подписчиках получен");
            return Ok(followers);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserFollower>> Get(int id)
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
            _logger.LogInformation("Запись о подписчике успешно получен");
            return Ok(result);
        }

        [HttpGet("/api/users/{userId}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserFollower>>> GetByUserId(int userId)
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
            _logger.LogInformation("Записи о подписчиках успешно получены");
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
                _logger.LogWarning("Попытка удаления с некорректным id={id}", id);
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
        public async Task<ActionResult<UserFollower>> Create([FromBody] UserFollower follower)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            UserFollower newfollower = await _repository.CreateAsync(follower);

            _logger.LogInformation("Создана новая запись с id={id}", newfollower.Id);
            return Created($"/api/UserFollower/{newfollower.Id}", newfollower);
        }
    }
}
