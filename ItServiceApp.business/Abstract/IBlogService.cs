using ItServiceApp.entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItServiceApp.business.Abstract
{
    public interface IBlogService
    {
        bool Create(Blog entity);
        Task<Blog> CreateAsync(Blog entity);
        void Delete(Blog entity);
        Task DeleteAsync(Blog entity);
        Task<List<Blog>> GetAll();
        Blog GetBlogDetails(string url);
        Task<Blog> GetById(int id);
        Task<Blog> GetByName(string name);
        List<Blog> GetHomePageBlog();
        List<Blog> GetSearchResult(string searchString);
        void Update(Blog entity);
        Task UpdateAsync(Blog entityToUpdate, Blog entity);
    }
}