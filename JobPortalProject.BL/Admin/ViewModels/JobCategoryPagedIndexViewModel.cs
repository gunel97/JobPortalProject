using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class JobCategoryPagedIndexViewModel
    {
        public JobCategoryFilterViewModel? Filter { get; set; }
        public PagedResultModel<JobCategoryViewModel> JobCategories { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
    }
}
