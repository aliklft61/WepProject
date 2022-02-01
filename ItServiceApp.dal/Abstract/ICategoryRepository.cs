using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItServiceApp.entity;

namespace ItServiceApp.dal.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByIdWithProducts(int categoryId);

        
        void DeleteFromCategory(int productId, int categoryId);
    }
}
