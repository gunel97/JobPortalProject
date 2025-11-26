using JobPortalProject.BL.Services.Contracts;
using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortalProject.UserMvc.Controllers
{
    public class Company : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly ICookieService _cookieService;

        public Company(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public Task<IActionResult> Details(string id)
        //{
        //    int companyId = int.Parse(id.Split('-').Last());

        //    var language = _cookieService.GetLanguageAsync();

        //    var company = _companyService.GetAsync(
        //                           predicate: x => x.Id == companyId && !x.IsDeleted,
        //                           include: x => x.
        //                           Include(ct=>ct.CompanyTranslations.Where(c=>c.LanguageId==language.Id)).
        //                           Include(c=>c.WorkingFields).ThenInclude(ct=>ct.
        //                           );
        //    return View();
        //}
    }
}
