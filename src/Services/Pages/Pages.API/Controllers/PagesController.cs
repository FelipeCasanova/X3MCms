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
    public class PagesController : ControllerBase
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }

        // GET api/pages/{format?}
        [HttpGet("{format?}")]
        [ProducesResponseType(typeof(IEnumerable<Page>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Page>>> Get()
        {
            var pages = await _pageService.GetAllPagesAsync();
            if (pages is null)
            {
                return Ok();
            }
            return pages.ToList();
        }

        // GET api/pages/populated/{format?}
        [HttpGet("populated/{format?}")]
        [ProducesResponseType(typeof(IEnumerable<PagePopulatedDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PagePopulatedDTO>>> GetPopulated()
        {
            var pages = await _pageService.GetAllPagesPopulateAsync();
            if (pages is null)
            {
                return Ok();
            }
            return pages.ToList();
        }

        // GET api/pages/{id:guid}.{format?}
        [HttpGet("{id:guid}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Page), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Page>> GetAsync(string id)
        {
            var pages = await _pageService.GetPageAsync(id);
            if (pages is null || !pages.Any())
            {
                return NotFound();            
            }
            return pages.ToList().First();
        }

        // GET api/pages/populated/{id:guid}.{format?}
        [HttpGet("populated/{id:guid}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<PagePopulatedDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PagePopulatedDTO>>> GetPopulatedAsync(string id)
        {
            var pages = await _pageService.GetPagePopulateAsync(id);
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList();
        }

        // GET api/pages/root
        [HttpGet("root/{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Page), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Page>> GetRootAsync(string id)
        {
            var pages = await _pageService.GetRootAsync();
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList().First();
        }

        // GET api/pages/populated/root/{format?}
        [HttpGet("populated/root/{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<PagePopulatedDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PagePopulatedDTO>>> GetPopulatedRootAsync(string id)
        {
            var pages = await _pageService.GetRootPopulateAsync();
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList();
        }

        // GET api/pages/url/{url}.{format?}
        [HttpGet("url/{url}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Page), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Page>> GetByURLAsync(string url)
        {
            var pages = await _pageService.GetPageByURLAsync(url);
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList().First();
        }

        // GET api/pages/populated/url/{url}.{format?}
        [HttpGet("populated/url/{url}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<PagePopulatedDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PagePopulatedDTO>>> GetPopulatedByURLAsync(string url)
        {
            var pages = await _pageService.GetPagePopulateByURLAsync(url);
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList();
        }

        // GET api/pages/breadcrumb/url/{url}.{format?}
        [HttpGet("breadcrumb/url/{url}.{format?}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Page>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Page>>> GetBreadCrumbToTheRootByURLAsync(string url)
        {
            var pages = await _pageService.GetPageBreadCrumbToTheRootAsync(url);
            if (pages is null || !pages.Any())
            {
                return NotFound();
            }
            return pages.ToList();
        }


        // POST api/pages
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PostAsync([FromBody] Page page)
        {
            if (page is null)
            {
                return BadRequest();
            }

            var result = await _pageService.CreatePageAsync(page);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id = page.Id }, page);
        }

        // PUT api/pages/{id:guid}
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> PutAsync(string id, [FromBody] Page page)
        {
            if (page is null)
            {
                return BadRequest();
            }

            var result =  await _pageService.UpdatePageAsync(id, page);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id = page.Id }, page);
        }

        // PUT api/pages/{id:guid}/name/{name}
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

            var result = await _pageService.UpdatePageNameAsync(id, name);
            if (!result)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        // DELETE api/pages/{id:guid}
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

            var result =  await _pageService.DeletePageAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
