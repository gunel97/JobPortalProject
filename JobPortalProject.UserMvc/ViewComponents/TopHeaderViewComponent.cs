using JobPortalProject.BL.UI.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalProject.UserMvc.ViewComponents
{
    public class TopHeaderViewComponent:ViewComponent
    {
        private readonly ITopHeaderService _topHeaderService;

        public TopHeaderViewComponent(ITopHeaderService topHeaderService)
        {
            _topHeaderService = topHeaderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _topHeaderService.GetTopHeaderViewModelAsync();

            return View(model);
        }
    }
}
