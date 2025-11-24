using JobPortalProject.BL.ViewModels.LanguageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface ICookieService
    {
        Task<LanguageViewModel> GetLanguageAsync();
    }
}
