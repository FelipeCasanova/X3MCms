using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public class PageDataRepository : IPageDataRepository
    {
        private readonly PageDataContext _context;

        public PageDataRepository(PageDataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PageData>> GetAllPagesAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(m:PageData)")
                  .Return(m => m.As<PageData>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<PageData>> GetPageAsync(string id)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(page:PageData)")
                  .Where((PageData page) => page.Id == id)
                  .Return(page => page.As<PageData>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<PageData>> GetPageByURLAsync(string url)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(page:PageData)")
                  .Where((PageData page) => page.URL == url)
                  .Return(page => page.As<PageData>())
                  .ResultsAsync;
            }
        }

        public void CreatePageAsync(PageData pageData)
        {
            if (string.IsNullOrWhiteSpace(pageData.Id))
            {
                pageData.Id = Guid.NewGuid().ToString();
            }

            using (var client = _context.Factory.Create())
            {
                client.Cypher
                    .Create("(page:PageData {pageData})")
                    .WithParam("pageData", pageData)
                    .ExecuteWithoutResults();
            }
        }

        public void UpdatePageAsync(PageData pageData)
        {
            using (var client = _context.Factory.Create())
            {
                client.Cypher
                    .Match("(page:PageData)")
                    .Where((PageData page) => page.Id == pageData.Id)
                    .Set("page = {pageData}")
                    .WithParam("page", pageData)
                    .ExecuteWithoutResults();
            }
        }

        public void UpdatePageNameAsync(string pageId, string name)
        {
            using (var client = _context.Factory.Create())
            {
                client.Cypher
                    .Match("(page:PageData)")
                    .Where((PageData page) => page.Id == pageId)
                    .Set("page.Name = {Name}")
                    .WithParam("Name", name)
                    .ExecuteWithoutResults();
            }
        }

        public async Task<Boolean> DeletePageNameAsync(string pageId)
        {
            using (var client = _context.Factory.Create())
            {
                var pages = await client.Cypher
                  .Match("(page:PageData)")
                  .Where((PageData page) => page.Id == pageId)
                  .Return(page => page.As<PageData>())
                  .ResultsAsync;
                if (pages is null || !pages.Any())
                {
                    return false;
                }

                await client.Cypher
                    .Match("(page:PageData)")
                    .Where((PageData page) => page.Id == pageId)
                    .Delete("page")
                    .ExecuteWithoutResultsAsync();
                return true;
            }
        }
    }
}
