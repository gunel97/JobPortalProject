using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface IOrderIndexService
    {
        public Task<OrderPagedIndexViewModel> GetIndexViewModel(OrderFilterViewModel filter);
    }
}
