using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.DA.DataContext.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class OrderPagedIndexViewModel
    {
        public PagedResultModel<OrderIndexViewModel> Orders { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
        public OrderFilterViewModel? Filter { get; set; }
        public List<SelectListItem> PaymentStatuses { get; set; }
    }

    public class OrderIndexViewModel
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public DateTime CreatedAt { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
    }
}
