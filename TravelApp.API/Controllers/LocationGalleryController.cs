using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationGalleryController : ControllerBase
    {
        private readonly ILocationGalleryRepository _repository;
        private readonly ILogger<LocationGalleryController> _logger;
        public LocationGalleryController(ILocationGalleryRepository repository,
            ILogger<LocationGalleryController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("by-location/{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LocationGallery>>> GetGalleryByLocationIdAsync(int locationId)
        {
            if (locationId <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", locationId);
                return BadRequest();
            }
            var gallery = await _repository.GetGalleryByIdLocationAsync(locationId);
            if (gallery == null || !gallery.Any())
            {
                _logger.LogWarning("В системе нет галереи для указанной локации {Id}", locationId);
                return NotFound();
            }
            _logger.LogInformation("Галерея картинок для локации успешно получена");
            return Ok(gallery);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationGallery>> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Получен некорректный id {id}", id);
                return BadRequest();
            }
            LocationGallery? picture = await _repository.GetAsync(id);
            if (picture == null)
            {
                _logger.LogWarning("Не удалось найти картинку из галереи локации по указанному id {id}", id);
                return NotFound();
            }
            _logger.LogInformation("Картинка с id {id} найден", id);
            return Ok(picture);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationGallery>> CreateAsync([FromBody] LocationGallery newPicture)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            LocationGallery createdPicture = await _repository.CreateAsync(newPicture);


            _logger.LogInformation("Добавлена новая картинка с id {id} в галерею", createdPicture.Id);
            return Created($"/api/LocationGallery/{createdPicture.Id}", createdPicture);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateAsync(int id, [FromBody] LocationGallery picture)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Неверные данные для обновления картинки с id {id} в галереи", id);
                return BadRequest();
            }
            LocationGallery? existingPicture = await _repository.GetAsync(id);
            if (existingPicture == null)
            {
                _logger.LogWarning("Картинка с id {id} не найден для обновления", id);
                return NotFound();
            }
            await _repository.UpdateAsync(picture);
            _logger.LogInformation("Картинка с id {id} успешно обновлена", id);
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
                _logger.LogWarning("Получен некорректный id {id}", id);
                return BadRequest();
            }
            if (await _repository.GetAsync(id) == null)
            {
                _logger.LogWarning("Картинка с id {id} не найден для удаления", id);
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Картинка с id {id} успешно удаления", id);
            return NoContent();
        }
    }
}