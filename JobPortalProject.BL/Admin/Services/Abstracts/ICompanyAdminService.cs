using JobPortalProject.BL.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.Services.Abstracts
{
    public interface ICompanyAdminService
    {
        public Task<CompanyDetailsAdminViewModel> GetDetailsAdminViewModel(string userId, int languageId);
    }
}
