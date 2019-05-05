using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pages.API.Infrastructure.Repositories;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
        }

        public async Task<Boolean> CreatePageAsync(Page page)
        {
            if(!(await PreConditionParentMustExist(page)))
            {
                return false;
            }
            return await _pageRepository.CreatePageAsync(page);
        }

        public async Task<IEnumerable<Page>> GetAllPagesAsync()
        {
            return await _pageRepository.GetAllPagesAsync();
        }

        public async Task<IEnumerable<Page>> GetPageAsync(string pageId)
        {
            return await _pageRepository.GetPageAsync(pageId);
        }

        public async Task<IEnumerable<Page>> GetPageByURLAsync(string url)
        {
            return await _pageRepository.GetPageByURLAsync(url);
        }

        public async Task<IEnumerable<Page>> GetRootAsync()
        {
            return await _pageRepository.GetRootAsync();
        }

        public async Task<IEnumerable<dynamic>> GetAllPagesPopulateAsync()
        {
            return await _pageRepository.GetAllPagesPopulateAsync();
        }

        public async Task<IEnumerable<dynamic>> GetPagePopulateAsync(string pageId)
        {
            return await _pageRepository.GetPagePopulateAsync(pageId);
        }

        public async Task<IEnumerable<dynamic>> GetPagePopulateByURLAsync(string url)
        {
            return await _pageRepository.GetPagePopulateByURLAsync(url);
        }

        public async Task<IEnumerable<dynamic>> GetRootPopulateAsync()
        {
            return await _pageRepository.GetRootPopulateAsync();
        }

        public async Task<Boolean> UpdatePageAsync(string pageId, Page page)
        {
            if (!(await PreConditionParentMustExist(page)))
            {
                return false;
            }
            return await _pageRepository.UpdatePageAsync(pageId, page);
        }

        public async Task<Boolean> UpdatePageNameAsync(string pageId, string name)
        {
            return await _pageRepository.UpdatePageNameAsync(pageId, name);
        }

        public async Task<Boolean> DeletePageAsync(string pageId)
        {
            return await _pageRepository.DeletePageAsync(pageId);
        }

        private async Task<Boolean> PreConditionParentMustExist(Page page)
        {
            if (!string.IsNullOrWhiteSpace(page?.ParentId))
            {
                var parents = await _pageRepository.GetPageAsync(page.ParentId);
                return parents != null && parents.Any();
            }
            return true;
        }

        public async Task<IEnumerable<Page>> GetPageBreadCrumbToTheRootAsync(string url)
        {
            return await _pageRepository.GetPageBreadCrumbToTheRootAsync(url);
        }
    }
}
