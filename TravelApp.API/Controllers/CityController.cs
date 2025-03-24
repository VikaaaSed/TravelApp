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
    }
}
