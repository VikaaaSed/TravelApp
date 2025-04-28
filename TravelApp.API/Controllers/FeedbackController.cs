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

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
        {
            if (feedback == null) return BadRequest();
            Feedback newfeedback = await _feedbackRepository.CreateAsync(feedback);
            return CreatedAtAction(nameof(Create), feedback);
        }

        [HttpGet("GetFeedbackByIdLocation/{idLocation}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FeedbackView>>> GetFeedbackByIdLocationAsync(int idLocation)
        {
            if (idLocation <= 0) return BadRequest();
            var Feedback = await _feedbackViewRepository.GetFeedbackByIdLocationAsync(idLocation);
            if (Feedback.Count() == 0 || Feedback == null) return NotFound();
            return Ok(Feedback);
        }

        [HttpGet("GetAcceptedFeedback/{idLocation}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<FeedbackView>>> GetAcceptedFeedbackByIdLocationAsync(int idLocation)
        {
            if (idLocation <= 0) return BadRequest();
            var Feedback = await _feedbackViewRepository.GetAcceptedFeedbackByIdLocationAsync(idLocation);
            if (Feedback.Count() == 0 || Feedback == null) return NotFound();
            return Ok(Feedback);
        }
    }
}
