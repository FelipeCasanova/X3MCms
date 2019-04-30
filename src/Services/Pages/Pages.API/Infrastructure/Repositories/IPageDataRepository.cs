using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pages.API.Model;

namespace Pages.API.Infrastructure.Repositories
{
    public interface IPageDataRepository
    {
        Task<IEnumerable<PageData>> GetAllPagesAsync();
        Task<IEnumerable<PageData>> GetPageAsync(string id);
        Task<IEnumerable<PageData>> GetPageByURLAsync(string url);
        Task CreatePageAsync(PageData pageData = default);
        Task<Boolean> UpdatePageAsync(string pageId, PageData pageData);
        Task<Boolean> UpdatePageNameAsync(string pageId, string name);
        Task<Boolean> DeletePageNameAsync(string pageId);
    }
}
