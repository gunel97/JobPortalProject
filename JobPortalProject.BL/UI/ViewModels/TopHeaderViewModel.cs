using JobPortalProject.BL.ViewModels.LanguageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class TopHeaderViewModel
    {
        public List<LanguageViewModel>? Languages { get; set; }
        public LanguageViewModel? SelectedLanguage { get; set; }
    }
}
