using JobPortalProject.BL.ViewModels.CountryViewModels;
using JobPortalProject.BL.ViewModels.LanguageViewModels;
using JobPortalProject.BL.ViewModels.Pagination;
using JobPortalProject.BL.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortalProject.BL.Admin.ViewModels
{
    public class UserPagedIndexViewModel
    {
        public UserFilterViewModel? Filter { get; set; }
        public PagedResultModel<UserViewModel> Users { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
        public List<string> Roles { get; set; } = [];
    }

    public class CompanyPagedIndexViewModel
    {
        public CompanyUserFilterViewModel? Filter { get; set; }
        public PagedResultModel<CompanyUserViewModel> Users { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
    }

    public class CandidatePagedIndexViewModel
    {
        public CandidateUserFilterViewModel? Filter { get; set; }
        public PagedResultModel<CandidateUserViewModel> Users { get; set; } = null!;
        public List<LanguageViewModel> Languages { get; set; } = [];
    }
}
