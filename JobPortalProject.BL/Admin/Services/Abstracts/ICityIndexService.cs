using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.CityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface ICityIndexService
    {
        public Task<CityPagedIndexViewModel> GetPagedCityIndexModel(CityFilterViewModel filter);
    }
}
