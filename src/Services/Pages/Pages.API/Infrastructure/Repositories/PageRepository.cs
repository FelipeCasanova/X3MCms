﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly PagesContext _context;

        public PageRepository(PagesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Page>> GetRootAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.ParentId == String.Empty)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(p:Page)")
                  .Return(p => p.As<Page>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<Page>> GetPageAsync(string pageId)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.Id == pageId)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<Page>> GetPageByURLAsync(string url)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.URL == url)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;
            }
        }

        public async Task<Boolean> CreatePageAsync(Page page = default)
        {
            if (string.IsNullOrWhiteSpace(page?.Id))
            {
                page.Id = Guid.NewGuid().ToString();
            }

            using (var client = _context.Factory.Create())
            {
                var query = client.Cypher;
                if (!string.IsNullOrWhiteSpace(page?.ParentId))
                {// Create relationship with parent
                    query = query.Match("(parent:Page)")
                        .Where((Page parent) => parent.Id == page.ParentId)
                        .CreateUnique("(p:Page {page})-[:CHILD_OF]->(parent)")
                        .CreateUnique("(parent)-[:PARENT_OF]->(p)");
                }
                else
                {
                    query = query.Create("(p:Page {page})");
                }

                await query.WithParam("page", page).ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> UpdatePageAsync(string pageId, Page page)
        {
            using (var client = _context.Factory.Create())
            {
                var pages = await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.Id == pageId)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;

                if (pages is null || !pages.Any())
                {
                    return false;
                }
                
                page.Id = pageId;

                if (!string.IsNullOrWhiteSpace(page?.ParentId))
                {
                    // Delete relationships PARENT_OF and CHILD_OF
                    await client.Cypher
                        .OptionalMatch("()-[r:PARENT_OF]->(p:Page)")
                        .Where((Page p) => p.Id == page.Id)
                        .Delete("r")
                        .ExecuteWithoutResultsAsync();

                    await client.Cypher
                        .OptionalMatch("(p:Page)-[r:CHILD_OF]->()")
                        .Where((Page p) => p.Id == page.Id)
                        .Delete("r")
                        .ExecuteWithoutResultsAsync();


                    await client.Cypher.Match("(p:Page)", "(parent:Page)")
                        .Where((Page p) => p.Id == page.Id)
                        .AndWhere((Page parent) => parent.Id == page.ParentId)
                        .CreateUnique("(p)-[:CHILD_OF]->(parent)")
                        .CreateUnique("(parent)-[:PARENT_OF]->(p)")
                        .ExecuteWithoutResultsAsync();
                }

                await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.Id == page.Id)
                    .Set("p = {page}")
                    .WithParam("page", page)
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> UpdatePageNameAsync(string pageId, string name)
        {
            using (var client = _context.Factory.Create())
            {
                var pages = await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.Id == pageId)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;

                if (pages is null || !pages.Any())
                {
                    return false;
                }

                await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .Set("p.Name = {name}")
                    .WithParam("name", name)
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> DeletePageAsync(string pageId)
        {
            using (var client = _context.Factory.Create())
            {
                var pages = await client.Cypher
                  .Match("(p:Page)")
                  .Where((Page p) => p.Id == pageId)
                  .Return(p => p.As<Page>())
                  .ResultsAsync;

                if (pages is null || !pages.Any())
                {
                    return false;
                }

                await client.Cypher
                    .OptionalMatch("(p:Page)-[r:HAS_ZONE]->()")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("()-[r:ZONE_BELONGS_TO]->(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("()-[r:PARENT_OF]->(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("(p:Page)-[r:PARENT_OF]->()")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("(p:Page)-[r:CHILD_OF]->()")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("()-[r:CHILD_OF]->(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .Delete("p")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<IEnumerable<PagePopulatedDTO>> GetAllPagesPopulateAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                    .Match("(p:Page)")
                    .OptionalMatch("(p:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p)-[:HAS_ZONE]->(z)")
                    .Return((p, c, z) => new PagePopulatedDTO
                     {
                        Page = p.As<Page>(), 
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .ResultsAsync;
            }
        }

        public async Task<IEnumerable<PagePopulatedDTO>> GetPagePopulateAsync(string pageId)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.Id == pageId)
                    .OptionalMatch("(p:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p)-[:HAS_ZONE]->(z)")
                    .Return((p, c, z) => new PagePopulatedDTO
                    {
                        Page = p.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .Union()
                    .Match("(p:Page)-[:PARENT_OF*1]->(c:Page)")
                    .Where((Page p) => p.ParentId == pageId)
                    .OptionalMatch("(p)-[:HAS_ZONE]->(z)")
                    .Return((p, c, z) => new PagePopulatedDTO
                     {
                        Page = p.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .ResultsAsync;
            }
        }

        public async Task<IEnumerable<PagePopulatedDTO>> GetPagePopulateByURLAsync(string url)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.URL == url)
                    .OptionalMatch("(p:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p)-[:HAS_ZONE]->(z)")
                    .Return((p, c, z) => new PagePopulatedDTO 
                    {
                        Page = p.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .Union()
                    .Match("(p:Page)-[:PARENT_OF*1]->(p2:Page)")
                    .Where((Page p) => p.URL == url)
                    .OptionalMatch("(p2:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p2)-[:HAS_ZONE]->(z)")
                    .Return((p2, c, z) => new PagePopulatedDTO
                    {
                        Page = p2.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .ResultsAsync;
            }
        }

        public async Task<IEnumerable<PagePopulatedDTO>> GetRootPopulateAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                    .Match("(p:Page)")
                    .Where((Page p) => p.ParentId == String.Empty)
                    .OptionalMatch("(p:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p)-[:HAS_ZONE]->(z)")
                    .Return((p, c, z) => new PagePopulatedDTO
                     {
                        Page = p.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .Union()
                    .Match("(p:Page)-[:PARENT_OF*1]->(p2:Page)")
                    .Where((Page p) => p.ParentId == String.Empty)
                    .OptionalMatch("(p2:Page)-[:PARENT_OF]->(c:Page)")
                    .OptionalMatch("(p2)-[:HAS_ZONE]->(z)")
                    .Return((p2, c, z) => new PagePopulatedDTO
                    {
                        Page = p2.As<Page>(),
                        Zones = z.CollectAsDistinct<Zone>(),
                        Children = c.CollectAsDistinct<Page>()
                    })
                    .ResultsAsync;
            }
        }

        public async Task<IEnumerable<Page>> GetPageBreadCrumbToTheRootAsync(string url)
        {
            using (var client = _context.Factory.Create())
            {
                var paths = await client.Cypher
                    .Match("p = (currentPage:Page)<-[:PARENT_OF*0..]-()")
                    .Where((Page currentPage) => currentPage.URL == url)
                    .Return((p) => Return.As<IEnumerable<Page>>("nodes(p)"))
                    .OrderByDescending("length(p)")
                    .Limit(1)
                    .ResultsAsync;
                return paths.SingleOrDefault();
            }
        }
    }
}
