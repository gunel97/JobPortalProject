using System.Diagnostics;
using System.Threading.Tasks;
using JobPortalProject.AdminMvc.Models;
using JobPortalProject.BL.Admin.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IIndexService _indexService;

        public HomeController(ILogger<HomeController> logger, IIndexService indexService)
        {
            _logger = logger;
            _indexService = indexService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _indexService.GetIndexViewModel();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
