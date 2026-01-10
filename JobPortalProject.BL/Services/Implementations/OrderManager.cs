using AutoMapper;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using JobPortalProject.DA.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobPortalProject.BL.Services.Implementations
{
    public class OrderManager : CrudManager<Order, OrderViewModel, OrderCreateViewModel, OrderUpdateViewModel>
 , IOrderService
    {
        private readonly ILanguageService _languageService;
        private readonly ICookieService _cookieService;

        public OrderManager(IRepositoryAsync<Order> repository, IMapper mapper, ILanguageService languageService, ICookieService cookieService) : base(repository, mapper)
        {
            _languageService = languageService;
            _cookieService = cookieService;
        }

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

        public async Task<PagedResultModel<OrderIndexViewModel>> GetPagedOrdersAdminAsync(OrderFilterViewModel filter)
        {
            var language = await _cookieService.GetLanguageAsync();
            var languages = await _languageService.GetAllAsync();

            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = BuildOrderByAdmin(filter);
            var predicate = BuildPredicateAdmin(filter);

            var pagedOrders = await Repository.GetPagedListAsync(
                predicate: predicate,
                orderBy: orderBy,
                include: x=>x.Include(x=>x.Company).ThenInclude(x=>x.CompanyTranslations),
                index: filter.Index, 
                size: filter.Size);

            var orderViewModels = new List<OrderIndexViewModel>();
            foreach (var item in pagedOrders.Items)
            {
                var model = new OrderIndexViewModel
                {
                    Id = item.Id,
                    CreatedAt = item.CreatedAt,
                    Status = item.Status,
                    Amount = item.Amount,
                };
                if (item.Company != null)
                {
                    var translation = item.Company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == language.Id);
                    if (translation != null)
                        model.CompanyName = translation.Name;
                    else
                    {
                        translation = item.Company.CompanyTranslations.FirstOrDefault();
                        if(translation!=null)
                            model.CompanyName=translation.Name;
                    }
                }
                orderViewModels.Add(model);
            }

            var pagedOrderModels = new PagedResultModel<OrderIndexViewModel>
            {
                Items = orderViewModels,
                Index = pagedOrders.Index,
                Size = pagedOrders.Size,
                Count = pagedOrders.Count,
                Pages = pagedOrders.Pages,
            };

            return pagedOrderModels;
        }

        public async Task<List<OrderIndexViewModel>> GetAllOrderIndexViewModel()
        {
            var orders = await Repository.GetAllAsync(include: x => x.Include(x => x.Company).ThenInclude(x => x.CompanyTranslations));
            var models = new List<OrderIndexViewModel>();

            foreach(var order in orders)
            {
                if (order.Company != null)
                {
                    var model = await MapToOrderIndexViewModel(order);
                    models.Add(model);
                }
            }

            return models;
        }

        public async Task<OrderIndexViewModel> MapToOrderIndexViewModel(Order item)
        {
            var language = await _cookieService.GetLanguageAsync();
            var model = new OrderIndexViewModel
            {
                Id = item.Id,
                CreatedAt = item.CreatedAt,
                Status = item.Status,
                Amount = item.Amount,
            };
            if (item.Company != null)
            {
                var translation = item.Company.CompanyTranslations.FirstOrDefault(x => x.LanguageId == language.Id);
                if (translation != null)
                    model.CompanyName = translation.Name;
                else
                {
                    translation = item.Company.CompanyTranslations.FirstOrDefault();
                    if (translation != null)
                        model.CompanyName = translation.Name;
                }
            }

            return model;
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

        private Func<IQueryable<Order>, IOrderedQueryable<Order>> BuildOrderByAdmin(OrderFilterViewModel filter)
        {
            var sortBy = filter.SortBy?.ToLower() ?? "createdat";
            var sortOrder = filter.SortOrder?.ToLower() ?? "desc";

            return query =>
            {
                return (sortBy, sortOrder) switch
                {
                    ("amount", "asc") => query.OrderBy(x => x.Amount),
                    ("amount", "desc") => query.OrderByDescending(x => x.Amount),

                    ("companyname", "asc") => query.OrderBy(x => x.Company.CompanyTranslations.FirstOrDefault().Name),
                    ("companyname", "desc") => query.OrderByDescending(x => x.Company.CompanyTranslations.FirstOrDefault().Name),

                    ("status", "asc") => query.OrderBy(x => x.Status),
                    ("status", "desc") => query.OrderByDescending(x => x.Status),

                    _ => sortOrder == "asc" ? query.OrderBy(x => x.CreatedAt) : query.OrderByDescending(x => x.CreatedAt)
                };
            };
        }

        private Expression<Func<Order, bool>> BuildPredicateAdmin(OrderFilterViewModel filter)
        {
            var term = filter.SearchTerm?.Trim().ToLower();
            var hasSearch = !string.IsNullOrWhiteSpace(term);

            var status = filter.Status;
            var hasStatus = status.HasValue;

            return x =>
                (
                    !hasSearch
                    || x.Company.CompanyTranslations.Any(t => t.Name.ToLower().Contains(term))
                )
                &&
                (
                    !hasStatus
                    || x.Status == status
                );
        }

    }
}
