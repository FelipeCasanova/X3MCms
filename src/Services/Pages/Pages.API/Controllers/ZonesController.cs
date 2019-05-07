using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pages.API.Infrastructure.Services;
using Pages.API.Model;

namespace Pages.API.Controllers
{
    [Route("api/v1/[controller]")]
    [FormatFilter]
    [ApiController]
    public class ZonesController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZonesController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }

        // GET api/zones/{format?}
        [HttpGet("{format?}")]
        [ProducesResponseType(typeof(List<Zone>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Zone>>> Get()
        {
            var zones = await _zoneService.GetAllZonesAsync();
            if (zones is null)
            {
                return Ok();
            }
            return zones.ToList();
        }

        // GET api/zones/{id:guid}.{format?}
        [HttpGet("{id:guid}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Zone), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Zone>> GetAsync(string id)
        {
            var zones = await _zoneService.GetZoneAsync(id);
            if (zones is null || !zones.Any())
            {
                return NotFound();            
            }
            return zones.ToList().First();
        }

        // POST api/zones
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PostAsync([FromBody] Zone zone)
        {
            if (zone is null)
            {
                return BadRequest();
            }

            var result = await _zoneService.CreateZoneAsync(zone);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id = zone.Id }, zone);
        }

        // PUT api/zones/{id:guid}
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutAsync(string id, [FromBody] Zone zone)
        {
            if (string.IsNullOrWhiteSpace(id) || zone is null)
            {
                return BadRequest();
            }

            var result =  await _zoneService.UpdateZoneAsync(id, zone);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id = zone.Id }, zone);
        }

        // PUT api/zones/{id}/name/{name}
        [HttpPut("{id:guid}/name/{name}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutNameAsync(string id, string name)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var result = await _zoneService.UpdateZoneNameAsync(id, name);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        // PUT api/zones/{id}/type/{type}
        [HttpPut("{id:guid}/type/{type}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutTypeAsync(string id, ZoneEnum type)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var result = await _zoneService.UpdateZoneTypeAsync(id, type);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        // DELETE api/zones/{id}
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<StatusCodeResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var result =  await _zoneService.DeleteZoneAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
