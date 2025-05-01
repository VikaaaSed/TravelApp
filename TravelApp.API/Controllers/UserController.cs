using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> CreateAsync([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest(ModelState);
            }

            var newUser = await _userRepository.CreateAsync(user);
            _logger.LogInformation("Создан новый пользователь с id {id}", newUser.Id);

            return Created($"api/User/{newUser.Id}", newUser);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid || user.Id != id)
            {
                _logger.LogWarning("Неверные данные для обновления пользователя с id {id}", id);
                return BadRequest();
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                _logger.LogWarning("Пользователь с id {id} не найден для обновления", id);
                return NotFound();
            }

            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("Пользователь с id {id} успешно обновлён", id);

            return Ok(user);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", id);
                return BadRequest();
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с id {id} не найден", id);
                return NotFound();
            }

            _logger.LogInformation("Пользователь с id {id} найден", id);
            return Ok(user);
        }

        [HttpGet("by-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetByEmailAsync([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Попытка запроса пользователя с пустым email");
                return BadRequest();
            }

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с email {email} не найден", email);
                return NotFound();
            }

            _logger.LogInformation("Пользователь с email {email} найден", email);
            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null || !users.Any())
            {
                _logger.LogInformation("В системе нет зарегистрированных пользователей");
                return NotFound();
            }

            _logger.LogInformation("Список пользователей успешно получен");
            return Ok(users);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка удаления с некорректным id {id}", id);
                return BadRequest();
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Пользователь с id {id} не найден для удаления", id);
                return NotFound();
            }

            await _userRepository.DeleteAsync(id);
            _logger.LogInformation("Пользователь с id {id} успешно удалён", id);

            return NoContent();
        }
    }
}

