using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{

    public class PagedJobListingViewModel
    {
        public JobFilterViewModel? Filter { get; set; }
        public PagedResultModel<JobViewModel> Jobs { get; set; } = null!;
        public List<JobCategoryViewModel> JobCategories = [];
        public List<SelectListItem> JobTypes = [];
        public List<SelectListItem> Genders = [];
        public Dictionary<int, int> JobTypeCounts { get; set; } = [];
        public Dictionary<int, int> GenderCounts { get; set; } = [];
        public double MinSalary { get; set; }
        public double MaxSalary { get; set; }
    }
}
