using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public interface IPageRepository
    {
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task<IEnumerable<PagePopulatedDTO>> GetAllPagesPopulateAsync();
        Task<IEnumerable<Page>> GetPageAsync(string pageId);
        Task<IEnumerable<PagePopulatedDTO>> GetPagePopulateAsync(string pageId);
        Task<IEnumerable<Page>> GetRootAsync();
        Task<IEnumerable<PagePopulatedDTO>> GetRootPopulateAsync();
        Task<IEnumerable<Page>> GetPageByURLAsync(string url);
        Task<IEnumerable<PagePopulatedDTO>> GetPagePopulateByURLAsync(string url);
        Task<IEnumerable<Page>> GetPageBreadCrumbToTheRootAsync(string url);
        Task<Boolean> CreatePageAsync(Page page = default);
        Task<Boolean> UpdatePageAsync(string pageId, Page page);
        Task<Boolean> UpdatePageNameAsync(string pageId, string name);
        Task<Boolean> DeletePageAsync(string pageId);
    }
}
