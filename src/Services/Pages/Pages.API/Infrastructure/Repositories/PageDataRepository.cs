using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
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

        public async Task CreatePageAsync(PageData pageData = default)
        {
            if (string.IsNullOrWhiteSpace(pageData?.Id))
            {
                pageData.Id = Guid.NewGuid().ToString();
            }

            using (var client = _context.Factory.Create())
            {
                var query = client.Cypher;
                if (!string.IsNullOrWhiteSpace(pageData?.ParentId))
                {// Create relationship with parent
                    query = query.Match("(pageParent:PageData)")
                        .Where((PageData pageParent) => pageParent.Id == pageData.ParentId)
                        .CreateUnique("(page:PageData {pageData})-[:CHILD_OF]->(pageParent)")
                        .CreateUnique("(pageParent)-[:PARENT_OF]->(page)");
                }
                else
                {
                    query = query.CreateUnique("(page:PageData {pageData})");
                }

                await query.WithParam("pageData", pageData).ExecuteWithoutResultsAsync();
            }
        }

        public async Task<Boolean> UpdatePageAsync(string pageId, PageData pageData)
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
                
                pageData.Id = pageId;

                if (!string.IsNullOrWhiteSpace(pageData?.ParentId))
                {
                    // Delete relationships PARENT_OF and CHILD_OF
                    await client.Cypher
                    .OptionalMatch("()-[r1:PARENT_OF]->(page:PageData)")
                    .Where((PageData page) => page.Id == pageData.Id)
                    .Delete("r1")
                    .ExecuteWithoutResultsAsync();

                    await client.Cypher
                        .OptionalMatch("(page:PageData)-[r2:CHILD_OF]->()")
                        .Where((PageData page) => page.Id == pageData.Id)
                        .Delete("r2")
                        .ExecuteWithoutResultsAsync();


                    await client.Cypher.Match("(page:PageData)", "(pageParent:PageData)")
                        .Where((PageData page) => page.Id == pageData.Id)
                        .AndWhere((PageData pageParent) => pageParent.Id == pageData.ParentId)
                        .CreateUnique("(page)-[:CHILD_OF]->(pageParent)")
                        .CreateUnique("(pageParent)-[:PARENT_OF]->(page)")
                        .ExecuteWithoutResultsAsync();
                }

                await client.Cypher
                    .Match("(page:PageData)")
                    .Where((PageData page) => page.Id == pageData.Id)
                    .Set("page = {pageData}")
                    .WithParam("pageData", pageData)
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> UpdatePageNameAsync(string pageId, string name)
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
                    .Set("page.Name = {Name}")
                    .WithParam("Name", name)
                    .ExecuteWithoutResultsAsync();

                return true;
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

                //await client.Cypher
                //.OptionalMatch("()-[r1]->(page:PageData)-[r2]->()")
                //.Where((PageData page) => page.Id == pageId)
                //.Delete("r1, r2, page")
                //.ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("()-[r1:PARENT_OF]->(page:PageData)")
                    .Where((PageData page) => page.Id == pageId)
                    .Delete("r1")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("(page:PageData)-[r2:CHILD_OF]->()")
                    .Where((PageData page) => page.Id == pageId)
                    .Delete("r2")
                    .ExecuteWithoutResultsAsync();

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
