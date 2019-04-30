using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pages.API.Infrastructure.Repositories;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IPageRepository _pageRepository;

        public ZoneService(IZoneRepository zoneRepository, IPageRepository pageRepository)
        {
            _zoneRepository = zoneRepository;
            _pageRepository = pageRepository;
        }

        public async Task<Boolean> CreateZoneAsync(Zone zone)
        {
            if (!(await PreConditionPageMustExist(zone.PageId)))
            {
                return false;
            }
            return await _zoneRepository.CreateZoneAsync(zone);
        }

        public async Task<IEnumerable<Zone>> GetAllZonesAsync()
        {
            return await _zoneRepository.GetAllZonesAsync();
        }

        public async Task<IEnumerable<Zone>> GetZoneAsync(string id)
        {
            return await _zoneRepository.GetZoneAsync(id);
        }

        public async Task<Boolean> UpdateZoneAsync(string zoneId, Pages.API.Model.Zone zone)
        {
            if (!(await PreConditionPageMustExist(zone.PageId)))
            {
                return false;
            }
            return await _zoneRepository.UpdateZoneAsync(zoneId, zone);
        }

        public async Task<Boolean> UpdateZoneNameAsync(string zoneId, string name)
        {
            return await _zoneRepository.UpdateZoneNameAsync(zoneId, name);
        }

        public async Task<Boolean> UpdateZoneTypeAsync(string zoneId, ZoneEnum type)
        {
            return await _zoneRepository.UpdateZoneTypeAsync(zoneId, type);
        }

        public async Task<Boolean> DeleteZoneAsync(string zoneId)
        {
            return await _zoneRepository.DeleteZoneAsync(zoneId);
        }

        private async Task<Boolean> PreConditionPageMustExist(string pageId)
        {
            if (!string.IsNullOrWhiteSpace(pageId))
            {
                var pages = await _pageRepository.GetPageAsync(pageId);
                return pages != null && pages.Any();
            }
            return true;
        }
    }
}
