using JobPortalProject.BL.Admin.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.AdminMvc.ViewComponents
{
    public class SidebarLanguageViewComponent : ViewComponent
    {
        private readonly ISidebarLanguageService _sidebarLanguageService;

        public SidebarLanguageViewComponent(ISidebarLanguageService sidebarLanguageService)
        {
            _sidebarLanguageService = sidebarLanguageService;
        }

        public async Task< IViewComponentResult> InvokeAsync()
        {
            var model = await _sidebarLanguageService.GetSidebarLanguageModelAsync();

            return View(model);
        }
    }
}
