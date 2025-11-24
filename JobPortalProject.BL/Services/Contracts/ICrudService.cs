using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface ICrudService<TEntity, TViewModel, TCreateViewModel, TUpdateViewModel>
    {
        Task<TViewModel?> GetByIdAsync(int id);
        Task<TViewModel> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
                                  IIncludableQueryable<TEntity, object>>? include = null, bool AsNoTracking = false);
        Task<IEnumerable<TViewModel>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                    bool AsNoTracking = false);
        Task<TViewModel> CreateAsync(TCreateViewModel createViewModel);
        Task<bool> UpdateAsync(int id, TUpdateViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
