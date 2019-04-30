using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Services
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAllPagesAsync();
        Task<IEnumerable<dynamic>> GetAllPagesPopulateAsync();
        Task<IEnumerable<Page>> GetPageAsync(string pageId);
        Task<IEnumerable<dynamic>> GetPagePopulateAsync(string pageId);
        Task<IEnumerable<Page>> GetPageByURLAsync(string url);
        Task<Boolean> CreatePageAsync(Page page);
        Task<Boolean> UpdatePageAsync(string pageId, Page page);
        Task<Boolean> UpdatePageNameAsync(string pageId, string name);
        Task<Boolean> DeletePageAsync(string pageId);
    }
}
