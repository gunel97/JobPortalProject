using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
