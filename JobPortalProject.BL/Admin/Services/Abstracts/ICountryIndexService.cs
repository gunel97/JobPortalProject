using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.CountryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface ICountryIndexService
    {
        public Task<CountryPagedIndexViewModel> GetPagedCountryIndexModel(CountryFilterViewModel filter);
    }
}
