using JobPortalProject.DA.DataContext;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Pagination;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.Repositories
{
    public class EfCoreRepositoryAsync<T> : IRepositoryAsync<T> where T : Entity
    {
        protected readonly AppDbContext DbContext;

        public EfCoreRepositoryAsync(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async virtual Task<T> AddAsync(T entity)
        {
          var entityEntry = await DbContext.Set<T>().AddAsync(entity);
          await DbContext.SaveChangesAsync();

          return entityEntry.Entity;
        }

        public async virtual Task<T> DeleteAsync(T entity)
        {
            var entityEntry = DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }

        public async virtual Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool AsNoTracking = false)
        {

            IQueryable<T> queryable = DbContext.Set<T>();

            if (AsNoTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include(queryable);

            if (predicate != null) queryable = queryable.Where(predicate);

            if (orderBy != null) queryable = orderBy(queryable);


            return await queryable.ToListAsync();
        }

        public async virtual Task<T?> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async virtual Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool AsNoTracking = false)
        {
            var query = DbContext.Set<T>().AsQueryable();

            if (AsNoTracking) query = query.AsNoTracking();
            if (include != null) query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async virtual Task<PagedResult<T>> GetPagedListAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, 
                                                      IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>,
                                                      IIncludableQueryable<T, object>>? include = null, 
                                                      int index = 0, int size = 10, bool enableTracking = true)
        {
            IQueryable<T> queryable = DbContext.Set<T>();

            if (!enableTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include(queryable);

            if (predicate != null) queryable = queryable.Where(predicate);

            if (orderBy != null) queryable = orderBy(queryable);

            return await queryable.ToPaginateAsync(index, size);
        }

        public async virtual Task<T> UpdateAsync(T entity)
        {
            var entityEntry = DbContext.Set<T>().Update(entity);
            await DbContext.SaveChangesAsync();

            return entityEntry.Entity;
        }
    }
}
