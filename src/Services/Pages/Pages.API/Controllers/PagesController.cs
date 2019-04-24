using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pages.API.Infrastructure.Repositories;
using Pages.API.Model;

namespace Pages.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPageDataRepository _pageDataRepository;

        public PagesController(IPageDataRepository pageDataRepository)
        {
            _pageDataRepository = pageDataRepository;
        }

        // GET api/pages
        [HttpGet]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<PageData>>> Get()
        {
            var pages = await _pageDataRepository.GetAllPagesAsync();
            if (pages is null)
            {
                return Ok();
            }
            return pages.ToList();
        }

        // GET api/pages/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<PageData>> Get(string id)
        {
            var pages = await _pageDataRepository.GetPageAsync(id);
            if (pages is null || !pages.Any())
            {
                return NotFound();            
            }
            return pages.ToList().First();
        }

        // POST api/pages
        [HttpPost]
        public void Post([FromBody] PageData pageData)
        {
            _pageDataRepository.CreatePageAsync(pageData);
        }

        // PUT api/pages/{id}
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] PageData pageData)
        {
            _pageDataRepository.UpdatePageAsync(pageData);
        }

        // DELETE api/values/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(List<PageData>), (int)HttpStatusCode.NotFound)]
        public async Task<StatusCodeResult> Delete(string id)
        {
            var result =  await _pageDataRepository.DeletePageNameAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
