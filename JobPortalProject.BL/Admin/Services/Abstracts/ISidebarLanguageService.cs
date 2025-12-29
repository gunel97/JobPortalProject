using JobPortalProject.BL.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface ISidebarLanguageService
    {
        public Task<TopHeaderViewModel> GetSidebarLanguageModelAsync();
    }
}
