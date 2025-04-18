﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LocationGallery>> CreateAsync([FromBody] LocationGallery newGallery)
        {
            if (newGallery == null) return BadRequest();
            LocationGallery gallery = await _repository.CreateAsync(newGallery);
            return Created($"/api/LocationGallery/{gallery.Id}", gallery);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateAsync([FromBody] LocationGallery gallery)
        {
            if (gallery == null) return BadRequest();
            await _repository.UpdateAsync(gallery);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return BadRequest();
            if (await _repository.GetAsync(id) == null) return NotFound();
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
