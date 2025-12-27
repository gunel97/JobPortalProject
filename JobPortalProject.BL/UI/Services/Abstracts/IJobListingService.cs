using JobPortalProject.BL.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.Services.Abstracts
{
    public interface IJobListingService
    {
        //public Task<JobListingViewModel> GetJobListingViewModel();
        public Task<PagedJobListingViewModel> GetPagedJobListingViewModel(int index=0, int size=10);
    }
}
