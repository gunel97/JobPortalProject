using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
