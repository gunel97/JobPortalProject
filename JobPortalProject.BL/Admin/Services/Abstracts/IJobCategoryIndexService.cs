using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface IJobCategoryIndexService
    {
        public Task<JobCategoryPagedIndexViewModel> GetPagedJobCategoryIndexModel(JobCategoryFilterViewModel filter);
    }
}
