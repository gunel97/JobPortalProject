using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.DA.Repositories.Contracts
{
    public interface IRepositoryAsync<T> where T:Entity
    {
        Task<T?> GetByIdAsync(int id);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
                          IIncludableQueryable<T, object>>? include = null,
                          bool AsNoTracking = false);

        Task<PagedResult<T>> GetPagedListAsync(Expression<Func<T, bool>>? predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                        int index = 0, int size = 10, bool enableTracking = false);

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                        bool AsNoTracking = false);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
    }
}
