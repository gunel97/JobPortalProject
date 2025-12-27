using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    //public class JobListingViewModel
    //{
    //    public List<JobViewModel> Jobs { get; set; } = [];
    //    public List<JobCategoryViewModel> JobCategories = [];
    //    public List<string> JobTypes = [];
    //    public List<string> Genders = [];
    //}
    public class PagedJobListingViewModel
    {
        public PagedResultModel<JobViewModel> Jobs { get; set; } = null!;
        public List<JobCategoryViewModel> JobCategories = [];
        public List<string> JobTypes = [];
        public List<string> Genders = [];
    }
}
