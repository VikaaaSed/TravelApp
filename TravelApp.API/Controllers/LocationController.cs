using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationInCityViewRepository _repositoryLocationInCity;
        public readonly ILocationInHomePageViewRepository _repositoryLocationInHomePage;
        private readonly ILogger<LocationController> _logger;
        public readonly ILocationRepository _repositoryLocation;

        public LocationController(ILocationInCityViewRepository locationInCityRepository, ILocationRepository repositoryLocation,
            ILocationInHomePageViewRepository locationInHomePageRepository, ILogger<LocationController> logger)
        {
            _repositoryLocationInCity = locationInCityRepository;
            _repositoryLocationInHomePage = locationInHomePageRepository;
            _repositoryLocation = repositoryLocation;
            _logger = logger;
        }

        [HttpGet("GetLocationsViewsByCityId")]
        public async Task<ActionResult<IEnumerable<LocationInCity>>> GetLocationsByCityIdAsync(int cityId)
           => Ok(await _repositoryLocationInCity.GetLocationInCityByCityIdAsync(cityId));
        [HttpGet("GetLocationByPageName")]
        public async Task<ActionResult<LocationInHomePage>> GetLocationByPageNameAsync(string pageName)
            => Ok(await _repositoryLocationInHomePage.GetLocationByPageNameAsync(pageName));

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Location>> GetAsync(int id)
        {
            if (id <= 0) return BadRequest();
            Location? location = await _repositoryLocation.GetAsync(id);
            if (location == null) return NotFound();
            return Ok(location);
        }
        [HttpGet("GetLocationsByCityId/{cityId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocationByCityIdAsync(int cityId)
        {
            if (cityId <= 0) return BadRequest();
            IEnumerable<Location> location = await _repositoryLocation.GetLocationByCityIdAsync(cityId);
            if (location.Count() == 0) return NotFound();
            return Ok(location);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Location>> CreateAsync([FromBody] Location newLocation)
        {
            if (newLocation == null) return BadRequest();
            Location location = await _repositoryLocation.CreateAsync(newLocation);
            return Created($"/api/Location/{location.Id}", location);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromBody] Location location)
        {
            if (location == null) return BadRequest();
            await _repositoryLocation.UpdateAsync(location);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return BadRequest();
            if (await _repositoryLocation.GetAsync(id) == null) return NotFound();
            await _repositoryLocation.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllAsync()
        {
            var locations = await _repositoryLocation.GetAllAsync();
            if (locations == null) return NotFound();
            return Ok(locations);
        }
    }
}
