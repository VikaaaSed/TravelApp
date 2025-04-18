using Microsoft.AspNetCore.Http;
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

        [HttpGet("GetGalleryByLocationId")]
        public async Task<ActionResult<LocationInHomePage>> GetGalleryByLocationIdAsync(int locationId)
            => Ok(await _repository.GetGalleryByIdLocationAsync(locationId));
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationGallery>> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();
            LocationGallery? location = await _repository.GetAsync(id);
            if (location == null) return NotFound();
            return Ok(location);
        }
    }
}
