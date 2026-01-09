using JobPortalProject.BL.Admin.Services.Abstracts;
using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyAdminService _companyAdminService;
        private readonly ICookieService _cookieService;

        public CompanyController(ICompanyAdminService companyAdminService, ICookieService cookieService)
        {
            _companyAdminService = companyAdminService;
            _cookieService = cookieService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string id, int langId, int page, string listName, string role)
        {
            if (langId == 0)
                langId = (await _cookieService.GetLanguageAsync()).Id;
            var model = await _companyAdminService.GetDetailsAdminViewModel(id, langId);
            if (model == null)
                return NotFound();
            ViewBag.ListName = listName;
            ViewBag.Index = page;
            ViewBag.Role = role;
            return View(model);
        }

    }
}
