using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Pages.API.Model.Zone>> GetAllZonesAsync();
        Task<IEnumerable<Pages.API.Model.Zone>> GetZoneAsync(string id);
        Task<Boolean> CreateZoneAsync(Pages.API.Model.Zone zone = default);
        Task<Boolean> UpdateZoneAsync(string zoneId, Pages.API.Model.Zone zone);
        Task<Boolean> UpdateZoneNameAsync(string zoneId, string name);
        Task<Boolean> UpdateZoneTypeAsync(string zoneId, ZoneEnum type);
        Task<Boolean> DeleteZoneAsync(string zoneId);
    }
}
