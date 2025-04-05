﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetUserByEmailAndPasswordHash")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User?>> GetUserByEmailAndPasswordHashAsync(string email, string passwordHash)
        {
            var user = await _userRepository.GetUserByEmailAndHashPassAsync(email, passwordHash);
            if (user == null)
            {
                _logger.LogInformation("Пользователь с указанными данными не найден");
                return NotFound($"Пользователь не найден");
            }
            return Ok(user);
        }
    }
}
