using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.DA.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Implementations
{
    public class IndexManager:IIndexService
    {
        private readonly IOrderService _orderService;
        private readonly ILanguageService _languageService;
        private readonly IOrderIndexService _orderIndexService;

        public IndexManager(IOrderService orderService, ILanguageService languageService, IOrderIndexService orderIndexService)
        {
            _orderService = orderService;
            _languageService = languageService;
            _orderIndexService = orderIndexService;
        }

        public async Task<List<OrderIndexViewModel>> GetLatestPayments()
        {
            var payments = await _orderService.GetAllOrderIndexViewModel();
            return payments.OrderByDescending(x => x.CreatedAt).Take(5).ToList();
        }

        public async Task<IndexViewModel> GetIndexViewModel()
        {
            var payments = await GetLatestPayments();
            var languages = await _languageService.GetAllAsync();

            var model = new IndexViewModel
            {
                Orders = payments,
                Languages=languages.ToList()
            };

            return model;
        }
    }
}
