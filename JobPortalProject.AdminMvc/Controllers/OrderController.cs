using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderIndexService _orderIndexService;

        public OrderController(IOrderIndexService orderIndexService)
        {
            _orderIndexService = orderIndexService;
        }

        public async Task<IActionResult> Index(OrderFilterViewModel filter)
        {
            var model = await _orderIndexService.GetIndexViewModel(filter);
            return View(model);
        }
    }
}
