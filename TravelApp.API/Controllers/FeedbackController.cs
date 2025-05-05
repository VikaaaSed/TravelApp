using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IFeedbackViewRepository _feedbackViewRepository;
        private readonly ILogger<FeedbackController> _logger;
        public FeedbackController(IFeedbackRepository feedbackRepository,
            ILogger<FeedbackController> logger,
            IFeedbackViewRepository feedbackViewRepository)
        {
            _feedbackRepository = feedbackRepository;
            _logger = logger;
            _feedbackViewRepository = feedbackViewRepository;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            Feedback newFeedback = await _feedbackRepository.CreateAsync(feedback);
            _logger.LogInformation("Создан новый отзыв с id={id}", newFeedback.Id);
            return Created($"/api/Feedback/{newFeedback.Id}", newFeedback);
        }

        [HttpGet("/api/locations/{idLocation}/feedbacks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FeedbackView>>> GetFeedbackByIdLocationAsync(int idLocation, [FromQuery] bool? accepted)
        {
            if (idLocation <= 0)
            {
                _logger.LogWarning("Попытка получения списка отзывов по некорректному idLocation={id}", idLocation);
                return BadRequest();
            }
            var feedbacks = await _feedbackViewRepository.GetFeedbackByIdLocationAsync(idLocation);
            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogWarning("Не удалось найти список отзывов по idLocation={id}", idLocation);
                return NotFound();
            }
            if (accepted != null) feedbacks = feedbacks.Where(x => x.Accepted).ToList();
            _logger.LogInformation("Получен список отзывов idLocation={id}", idLocation);
            return Ok(feedbacks);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Feedback?>> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка поиска с некорректным id={id}", id);
                return BadRequest();
            }
            var feedback = await _feedbackRepository.GetFeedbackAsync(id);
            if (feedback == null)
            {
                _logger.LogWarning("Отзыв с id={id} не найден", id);
                return NotFound();
            }
            _logger.LogInformation("Отзыв успешно получен");
            return Ok(feedback);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Feedback?>> Update(int id, [FromBody] Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Не прошла валидация данных пользователя при создании");
                return BadRequest();
            }
            if (await _feedbackRepository.GetFeedbackAsync(id) == null)
            {
                _logger.LogWarning("Отзыв с id={id} не найдена для обновления", id);
                return NotFound();
            }
            await _feedbackRepository.UpdateFeedbackAsync(feedback);
            _logger.LogInformation("Отзыв с id={id} успешно обновлён", id);
            return NoContent();
        }
        [HttpPatch("{id}/accept")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> AcceptedFeedback(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Попытка подтверждения отзыва с некорректным id={id}", id);
                return BadRequest();
            }
            var feedback = await _feedbackRepository.GetFeedbackAsync(id);
            if (feedback == null)
            {
                _logger.LogWarning("Отзыв с id={id} не найден для подтверждения", id);
                return NotFound();
            }
            await _feedbackRepository.AcceptedFeedbackAsync(id);
            _logger.LogInformation("Отзыв с id={id} успешно подтвержден", id);
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
            if (await _feedbackRepository.GetFeedbackAsync(id) == null)
            {
                _logger.LogWarning("Отзыв с id={id} не найдена для удаления", id);
                return NotFound();
            }
            await _feedbackRepository.DeleteFeedbackAsync(id);

            _logger.LogInformation("Отзыв с id={id} успешно удален", id);
            return NoContent();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetAllAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllAsync();
            if (feedbacks == null || !feedbacks.Any())
            {
                _logger.LogWarning("В системе нет ни одного отзыва");
                return NotFound();
            }
            _logger.LogInformation("Получен список всех отзывов");
            return Ok(feedbacks);
        }
    }
}
