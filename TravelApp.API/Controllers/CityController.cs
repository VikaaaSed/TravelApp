using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Repositories.Interfaces;

namespace TravelApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _repository;
        private readonly ICityInHomePageViewRepository _viewRepository;
        private readonly ILogger<CityController> _logger;
        public CityController(ICityRepository repository, ILogger<CityController> logger,
            ICityInHomePageViewRepository viewRepository)
        {
            _repository = repository;
            _logger = logger;
            _viewRepository = viewRepository;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllAsync()
            => Ok(await _repository.GetAllAsync());
        [HttpGet("GetVisibleCity")]
        public async Task<ActionResult<IEnumerable<City>>> GetVisibleCityAsync()
            => Ok(await _repository.GetVisibleCityAsync());
        [HttpGet("GetCityInHomePage")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityInHomePageAsync()
            => Ok(await _viewRepository.GetAllAsync());
        [HttpGet("GetCityByPageName")]
        public async Task<ActionResult<City?>> GetCityByPageNameAsync(string pageName)
            => Ok(await _repository.GetCityByPageNameAsync(pageName));
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<City>> Create([FromBody] City city)
        {
            if (city == null) return BadRequest();
            City createdCity = await _repository.CreateCityAsync(city);
            return CreatedAtAction(nameof(Create), createdCity); ;
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromBody] City city)
        {
            if (city == null) return BadRequest();
            await _repository.UpdateCityAsync(city);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            if (await _repository.GetAsync(id) == null) return NotFound();
            await _repository.DeleteCityAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<City>> Get(int id)
        {
            if (id <= 0) return BadRequest();

            var result = await _repository.GetAsync(id);
            return result == null ? NotFound() : Ok(result);
        }


    }
}
