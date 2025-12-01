using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class JobListingViewModel
    {
        public List<JobViewModel> Jobs { get; set; } = [];
        public List<JobCategoryViewModel> JobCategories = [];
        public List<string> JobTypes = [];
        public List<string> Genders = [];
    }
}
