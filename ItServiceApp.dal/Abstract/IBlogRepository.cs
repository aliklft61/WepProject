using ItServiceApp.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItServiceApp.dal.Abstract
{
    public interface IBlogRepository : IRepository<Blog>
    {
        Task<Blog> GetByName(string name);
        Blog GetBlogDetails(string url);
        List<Blog> GetSearchResult(string searchString);
        List<Blog> GetHomePagePBlogs();
        void Update(Blog entity, int[] categoryIds);
    }
}
