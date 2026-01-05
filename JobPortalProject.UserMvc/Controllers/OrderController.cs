using JobPortalProject.BL.Services.Contracts;
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

        public OrderController(IMembershipService membershipService, ICompanyService companyService)
        {
            _membershipService = membershipService;
            _companyService = companyService;
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
    }
}
