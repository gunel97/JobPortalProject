using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class JobCategoryIndexViewModel
    {
        public List<JobCategoryViewModel> JobCategories { get; set; } = [];
        public List<LanguageViewModel> Languages { get; set; } = [];
        public List<int> SelectedIdsToDelete { get; set; } = [];
    }
}
