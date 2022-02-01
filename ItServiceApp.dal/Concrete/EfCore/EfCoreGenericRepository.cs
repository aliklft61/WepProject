using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItServiceApp.dal.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ItServiceApp.dal.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbContext context;
        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }
        public async Task<List<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        
        public virtual void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public async Task CreateAsync(TEntity entity)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }
        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        //public List<TEntity> GetAll()
        //{
        //    return context.Set<TEntity>().ToList();
        //}

        //public TEntity GetById(int id)
        //{
        //    return context.Set<TEntity>().Find(id);
        //}

       
    }
}