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
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPageDataService _pageDataService;

        public PagesController(IPageDataService pageDataService)
        {
            _pageDataService = pageDataService;
        }

        // GET api/pages
        [HttpGet]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PageData>>> Get()
        {
            var pages = await _pageDataService.GetAllPagesAsync();
            if (pages is null)
            {
                return Ok();
            }
            return pages.ToList();
        }

        // GET api/pages/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PageData>> GetAsync(string id)
        {
            var pages = await _pageDataService.GetPageAsync(id);
            if (pages is null || !pages.Any())
            {
                return NotFound();            
            }
            return pages.ToList().First();
        }

        // POST api/pages
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PostAsync([FromBody] PageData pageData)
        {
            if (pageData is null)
            {
                return BadRequest();
            }

            await _pageDataService.CreatePageAsync(pageData);
            return CreatedAtAction(nameof(GetAsync), new { id = pageData.Id }, null);
        }

        // PUT api/pages/{id}
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutAsync(string id, [FromBody] PageData pageData)
        {
            if (pageData is null)
            {
                return BadRequest();
            }

            var result =  await _pageDataService.UpdatePageAsync(id, pageData);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id = pageData.Id }, null);
        }

        // PUT api/pages/{id}/name/{name}
        [HttpPut("{id}/name/{name}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutNameAsync(string id, string name)
        {
            if (name is null)
            {
                return BadRequest();
            }

            var result = await _pageDataService.UpdatePageNameAsync(id, name);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        // DELETE api/pages/{id}
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        public async Task<StatusCodeResult> Delete(string id)
        {
            var result =  await _pageDataService.DeletePageNameAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
