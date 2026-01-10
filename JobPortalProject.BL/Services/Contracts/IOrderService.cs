using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;

namespace JobPortalProject.BL.Services.Contracts
{
    public interface IOrderService : ICrudService<Order, OrderViewModel, OrderCreateViewModel, OrderUpdateViewModel>
    {
        public Task<PagedResultModel<OrderViewModel>> GetPagedOrdersOfCompanyAsync(OrderFilterViewModel filter, int companyId);
        public Task<PagedResultModel<OrderViewModel>> GetPagedOrdersAsync(OrderFilterViewModel filter);
        public Task<PagedResultModel<OrderIndexViewModel>> GetPagedOrdersAdminAsync(OrderFilterViewModel filter);
    }
}
