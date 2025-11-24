using JobPortalProject.BL.UI.Services.Abstracts;
using JobPortalProject.UserMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobPortalProject.UserMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = await _homeService.GetHomeViewModelAsync();

            return View(homeViewModel);
        }

       
    }
}
