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

        public LocationController(ILocationInCityViewRepository locationInCityRepository,
            ILocationInHomePageViewRepository locationInHomePageRepository, ILogger<LocationController> logger)
        {
            _repositoryLocationInCity = locationInCityRepository;
            _repositoryLocationInHomePage = locationInHomePageRepository;
            _logger = logger;
        }

        [HttpGet("GetLocationsByCityId")]
        public async Task<ActionResult<IEnumerable<LocationInCity>>> GetLocationsByCityIdAsync(int cityId)
           => Ok(await _repositoryLocationInCity.GetLocationInCityByCityIdAsync(cityId));
        [HttpGet("GetLocationByPageName")]
        public async Task<ActionResult<LocationInHomePage>> GetLocationByPageNameAsync(string pageName)
            => Ok(await _repositoryLocationInHomePage.GetLocationByPageNameAsync(pageName));
    }
}
