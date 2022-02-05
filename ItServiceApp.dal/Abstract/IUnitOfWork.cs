using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItServiceApp.dal.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        ICartRepository Carts { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IBlogRepository Blogs { get; }
        void Save();
        Task<int> SaveAsync();
    }
}
