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
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            User newuser = await _userRepository.CreateAsync(user);
            return CreatedAtAction(nameof(Create), user);
        }
        [HttpGet("GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User?>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation("Пользователь с email {Email} не найден", email);
                return NotFound($"Пользователь с email {email} не найден");
            }
            return Ok(user);
        }
        [HttpGet("GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogInformation("Пользователь с id {id} не найден", id);
                return NotFound($"Пользователь с id {id} не найден");
            }
            return Ok(user);
        }
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update([FromBody] User user)
        {
            if (await _userRepository.GetUserByIdAsync(user.Id) == null)
            {
                _logger.LogInformation("Пользователь с id {id} не найден", user.Id);
                return NotFound($"Пользователь с id {user.Id} не найден");
            }
            await _userRepository.UpdateAsync(user);
            return Ok(user);
        }
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null || !users.Any()) return NotFound();
            return Ok(users);
        }
    }
}
