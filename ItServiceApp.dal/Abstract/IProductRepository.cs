using ItServiceApp.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItServiceApp.dal.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByName(string name);
        Blog GetProductDetails(string url);
        Blog GetByIdWithCategories(int id);
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        List<Product> GetSearchResult(string searchString);
        List<Product> GetHomePageProducts();
        int GetCountByCategory(string category);
        void Update(Product entity, int[] categoryIds);
    }
}
