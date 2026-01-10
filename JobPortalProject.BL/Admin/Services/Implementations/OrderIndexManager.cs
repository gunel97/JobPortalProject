using AutoMapper;
using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.Services.Implementations;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class OrderIndexManager : IOrderIndexService
    {
        private readonly ILanguageService _languageService;
        private readonly IOrderService _orderService;
        private readonly IEnumService _enumService;

        public OrderIndexManager(IOrderService orderService, ILanguageService languageService, IEnumService enumService)
        {
            _orderService = orderService;
            _languageService = languageService;
            _enumService = enumService;
        }

        public async Task<OrderPagedIndexViewModel> GetIndexViewModel(OrderFilterViewModel filter)
        {
            var languages = await _languageService.GetAllAsync();

            filter ??= new OrderFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;
            var orders = await _orderService.GetPagedOrdersAdminAsync(filter);

            var model = new OrderPagedIndexViewModel
            {
                Languages = languages.ToList(),
                Orders = orders,
                Filter = filter,
                PaymentStatuses = _enumService.GetPaymentStatusListItems()
            };

            return model;
        }

    }
}
