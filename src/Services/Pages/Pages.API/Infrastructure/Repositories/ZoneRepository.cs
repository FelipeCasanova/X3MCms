using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Pages.API.Infrastructure;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly PagesContext _context;

        public ZoneRepository(PagesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Zone>> GetAllZonesAsync()
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(z:Zone)")
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;
            }
        }

        public async Task<IEnumerable<Zone>> GetZoneAsync(string id)
        {
            using (var client = _context.Factory.Create())
            {
                return await client.Cypher
                  .Match("(z:Zone)")
                  .Where((Zone z) => z.Id == id)
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;
            }
        }

        public async Task<Boolean> CreateZoneAsync(Zone zone = default)
        {

            if (string.IsNullOrWhiteSpace(zone?.Id))
            {
                zone.Id = Guid.NewGuid().ToString();
            }

            using (var client = _context.Factory.Create())
            {
                var query = client.Cypher;
                if (!string.IsNullOrWhiteSpace(zone?.PageId))
                {// Create relationship with parent
                    query = query.Match("(p:Page)")
                        .Where((Page p) => p.Id == zone.PageId)
                        .CreateUnique("(z:Zone {zone})-[:ZONE_BELONGS_TO]->(p)")
                        .CreateUnique("(p)-[:HAS_ZONE]->(z)");
                }
                else
                {
                    query = query.Create("(z:Zone {zone})");
                }

                await query.WithParam("zone", zone).ExecuteWithoutResultsAsync();
                return true;
            }
        }

        public async Task<Boolean> UpdateZoneAsync(string zoneId, Zone zone)
        {
            using (var client = _context.Factory.Create())
            {
                var zones = await client.Cypher
                  .Match("(z:Zone)")
                  .Where((Zone z) => z.Id == zoneId)
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;

                if (zones is null || !zones.Any())
                {
                    return false;
                }
                
                zone.Id = zoneId;

                if (!string.IsNullOrWhiteSpace(zone?.PageId))
                {
                    // Delete relationships HAS_ZONE and CHILD_OF
                    await client.Cypher
                        .OptionalMatch("()-[r:HAS_ZONE]->(z:Zone)")
                        .Where((Zone z) => z.Id == zone.Id)
                        .Delete("r")
                        .ExecuteWithoutResultsAsync();

                    await client.Cypher
                        .OptionalMatch("(z:Zone)-[r:ZONE_BELONGS_TO]->()")
                        .Where((Zone z) => z.Id == zone.Id)
                        .Delete("r")
                        .ExecuteWithoutResultsAsync();


                    await client.Cypher.Match("(z:Zone)", "(p:Page)")
                        .Where((Zone z) => z.Id == zone.Id)
                        .AndWhere((Page p) => p.Id == zone.PageId)
                        .CreateUnique("(z)-[:ZONE_BELONGS_TO]->(p)")
                        .CreateUnique("(p)-[:HAS_ZONE]->(z)")
                        .ExecuteWithoutResultsAsync();
                }

                await client.Cypher
                    .Match("(z:Zone)")
                    .Where((Zone z) => z.Id == zone.Id)
                    .Set("z = {zone}")
                    .WithParam("zone", zone)
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> UpdateZoneNameAsync(string zoneId, string name)
        {
            using (var client = _context.Factory.Create())
            {
                var zones = await client.Cypher
                  .Match("(z:Zone)")
                  .Where((Zone z) => z.Id == zoneId)
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;

                if (zones is null || !zones.Any())
                {
                    return false;
                }

                await client.Cypher
                    .Match("(z:Zone)")
                    .Where((Zone z) => z.Id == zoneId)
                    .Set("z.Name = {name}")
                    .WithParam("name", name)
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> UpdateZoneTypeAsync(string zoneId, ZoneEnum type)
        {
            using (var client = _context.Factory.Create())
            {
                var zones = await client.Cypher
                  .Match("(z:Zone)")
                  .Where((Zone z) => z.Id == zoneId)
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;

                if (zones is null || !zones.Any())
                {
                    return false;
                }

                await client.Cypher
                    .Match("(z:Zone)")
                    .Where((Zone z) => z.Id == zoneId)
                    .Set("z.Type = {type}")
                    .WithParam("type", type.ToString())
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        public async Task<Boolean> DeleteZoneAsync(string zoneId)
        {
            using (var client = _context.Factory.Create())
            {
                var zones = await client.Cypher
                  .Match("(z:Zone)")
                  .Where((Zone z) => z.Id == zoneId)
                  .Return(z => z.As<Zone>())
                  .ResultsAsync;

                if (zones is null || !zones.Any())
                {
                    return false;
                }

                await client.Cypher
                    .OptionalMatch("()-[r:HAS_ZONE]->(z:Zone)")
                    .Where((Zone z) => z.Id == zoneId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();

                await client.Cypher
                    .OptionalMatch("(z:Zone)-[r:ZONE_BELONGS_TO]->()")
                    .Where((Zone z) => z.Id == zoneId)
                    .Delete("r")
                    .ExecuteWithoutResultsAsync();


                await client.Cypher
                    .Match("(z:Zone)")
                    .Where((Zone z) => z.Id == zoneId)
                    .Delete("z")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }
    }
}
