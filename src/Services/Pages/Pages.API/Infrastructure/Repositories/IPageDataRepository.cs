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
        void CreatePageAsync(PageData pageData);
        void UpdatePageAsync(PageData pageData);
        void UpdatePageNameAsync(string pageId, string name);
        Task<Boolean> DeletePageNameAsync(string pageId);
    }
}
