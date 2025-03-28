using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<FeedbackController> _logger;
        public FeedbackController(IFeedbackRepository feedbackRepository,
            ILogger<FeedbackController> logger)
        {
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Feedback>> Create([FromBody] Feedback feedback)
        {
            Feedback newfeedback = await _feedbackRepository.CreateAsync(feedback);
            return CreatedAtAction(nameof(Create), feedback);
        }
    }
}
