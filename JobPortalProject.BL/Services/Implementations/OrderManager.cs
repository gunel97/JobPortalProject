using AutoMapper;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.BL.Services.Implementations
{
    public class OrderManager : CrudManager<Order, OrderViewModel, OrderCreateViewModel, OrderUpdateViewModel>
 , IOrderService
    {
        public OrderManager(IRepositoryAsync<Order> repository, IMapper mapper) : base(repository, mapper) { }

        public async Task<PagedResultModel<OrderViewModel>> GetPagedOrdersOfCompanyAsync(OrderFilterViewModel filter, int companyId)
        {
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = BuildOrderBy(filter);
            var pagedOrders = await Repository.GetPagedListAsync(predicate: x => x.CompanyId==companyId,
                orderBy: orderBy, index: filter.Index, size: filter.Size);

            var orderViewModels = new List<OrderViewModel>();
            foreach (var item in pagedOrders.Items)
            {
                var model = Mapper.Map<OrderViewModel>(item);
                orderViewModels.Add(model);
            }

            var pagedOrderModels = new PagedResultModel<OrderViewModel>
            {
                Items = orderViewModels,
                Index = pagedOrders.Index,
                Size = pagedOrders.Size,
                Count = pagedOrders.Count,
                Pages = pagedOrders.Pages,
            };

            return pagedOrderModels;
        }

        public async Task<PagedResultModel<OrderViewModel>> GetPagedOrdersAsync(OrderFilterViewModel filter)
        {
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = BuildOrderBy(filter);
            var pagedOrders = await Repository.GetPagedListAsync(orderBy: orderBy, index: filter.Index, size: filter.Size);

            var orderViewModels = new List<OrderViewModel>();
            foreach (var item in pagedOrders.Items)
            {
                var model = Mapper.Map<OrderViewModel>(item);
                orderViewModels.Add(model);
            }

            var pagedOrderModels = new PagedResultModel<OrderViewModel>
            {
                Items = orderViewModels,
                Index = pagedOrders.Index,
                Size = pagedOrders.Size,
                Count = pagedOrders.Count,
                Pages = pagedOrders.Pages,
            };

            return pagedOrderModels;
        }

        private Func<IQueryable<Order>, IOrderedQueryable<Order>> BuildOrderBy(OrderFilterViewModel filter)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "createdat";
            var sortOrder = "desc"; // default

            // Handle compound sort values (e.g., "Title_asc")
            if (sortBy.Contains('_'))
            {
                var parts = sortBy.Split('_');
                sortBy = parts[0];
                sortOrder = parts[1];
            }
            else if (!string.IsNullOrEmpty(filter.SortOrder))
            {
                sortOrder = filter.SortOrder.ToLower();
            }

            return queryable =>
            {
                IOrderedQueryable<Order> ordered = sortBy switch
                {

                    // Default: sort by posted date
                    _ => sortOrder == "asc"
                        ? queryable.OrderBy(x => x.CreatedAt)
                        : queryable.OrderByDescending(x => x.CreatedAt)
                };

                return ordered;
            };
        }
    }
}
