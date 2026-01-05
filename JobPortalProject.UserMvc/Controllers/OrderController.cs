using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.UserMvc.Controllers
{
    [Authorize(Roles = "Company")]
    public class OrderController : Controller
    {
        private readonly IMembershipService _membershipService;
        private readonly ICompanyService _companyService;
        private readonly IOrderService _orderService;

        public OrderController(IMembershipService membershipService, ICompanyService companyService, IOrderService orderService)
        {
            _membershipService = membershipService;
            _companyService = companyService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        
        {
            var model = await _companyService.GetCheckoutViewModelAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy()
        {
            var id =await _companyService.GetCompanyIdOfUser();
            string stripeUrl = await _membershipService.CreateRenewalCheckoutSessionAsync(id);

            Response.Headers.Add("Location", stripeUrl);
            return Redirect(stripeUrl);
        }

        public async Task<IActionResult> Success(string session_id)
        {
            bool result = await _membershipService.ProcessPaymentSuccessAsync(session_id);

            return result ? View("Success") : View("Error");
        }

        [Authorize(Roles ="Company")]
        public async Task<IActionResult> Payments(OrderFilterViewModel filter)
        {
            var companyId = await _companyService.GetCompanyIdOfUser();

            filter ??= new OrderFilterViewModel();
            if (filter.Index < 0) filter.Index = 0;
            if (filter.Size <= 0) filter.Size = 10;
            var orders = await _orderService.GetPagedOrdersOfCompanyAsync(filter, companyId);

            var model = new OrderPagedViewModel
            {
                Filter = filter,
                Orders = orders
            };

            return View(model);
        }
    }
}
