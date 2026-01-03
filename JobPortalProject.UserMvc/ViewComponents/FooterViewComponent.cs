using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
