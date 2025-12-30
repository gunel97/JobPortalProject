using JobPortalProject.BL.Admin.ViewModels;
using JobPortalProject.BL.ViewModels.CompanyTypeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface ICompanyTypeIndexService
    {
        public Task<CompanyTypePagedIndexViewModel> GetPagedCompanyTypeIndexModel(CompanyTypeFilterViewModel filter);
    }
}
