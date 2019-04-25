using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pages.API.Infrastructure.Repositories;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Services
{
    public class PageDataService : IPageDataService
    {
        private readonly IPageDataRepository _pageDataRepository;

        public PageDataService(IPageDataRepository pageDataRepository)
        {
            _pageDataRepository = pageDataRepository;
        }

        public async Task CreatePageAsync(PageData pageData)
        {
            await _pageDataRepository.CreatePageAsync(pageData);
        }

        public async Task<bool> DeletePageNameAsync(string pageId)
        {
            return await _pageDataRepository.DeletePageNameAsync(pageId);
        }

        public async Task<IEnumerable<PageData>> GetAllPagesAsync()
        {
            return await _pageDataRepository.GetAllPagesAsync();
        }

        public async Task<IEnumerable<PageData>> GetPageAsync(string id)
        {
            return await _pageDataRepository.GetPageAsync(id);
        }

        public async Task<IEnumerable<PageData>> GetPageByURLAsync(string url)
        {
            return await _pageDataRepository.GetPageByURLAsync(url);
        }

        public async Task<bool> UpdatePageAsync(string pageId, PageData pageData)
        {
            return await _pageDataRepository.UpdatePageAsync(pageId, pageData);
        }

        public async Task<bool> UpdatePageNameAsync(string pageId, string name)
        {
            return await _pageDataRepository.UpdatePageNameAsync(pageId, name);
        }
    }
}
