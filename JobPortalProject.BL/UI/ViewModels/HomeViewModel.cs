using JobPortalProject.BL.ViewModels.AddressViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using JobPortalProject.BL.ViewModels.JobCategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.UI.ViewModels
{
    public class HomeViewModel
    {
        public List<JobCategoryViewModel>? JobCategories { get; set; }
        public List<AddressViewModel>? Addresses { get; set; }
    }
}
