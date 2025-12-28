using JobPortalProject.BL.UI.ViewModels;
using JobPortalProject.BL.ViewModels.JobViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface IJobListingService
    {
        public Task<PagedJobListingViewModel> GetPagedJobListingViewModel(JobFilterViewModel filter);
    }
}
